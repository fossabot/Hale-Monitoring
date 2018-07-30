namespace Hale.Alert.Pushbullet
{
    using System;
    using System.Net;
    using Newtonsoft.Json;
    using GenericRequest = System.Collections.Generic.Dictionary<string, object>;

    public class PushbulletApi
    {
        private static readonly string UrlApiBase = "https://api.pushbullet.com/";

        public PushResponse Push(string title, string body, HaleAlertPushbulletRecipient recipient)
        {
            var request = new GenericRequest()
            {
                { "title", title },
                { "body", body },
                { "type", "note" }
            };
            switch (recipient.TargetType)
            {
                case PushbulletPushTarget.Device:
                    request.Add("device_iden", recipient.Target);
                    break;
                case PushbulletPushTarget.Email:
                    request.Add("email", recipient.Target);
                    break;
                case PushbulletPushTarget.Channel:
                    request.Add("channel_tag", recipient.Target);
                    break;
                case PushbulletPushTarget.Client:
                    request.Add("client_iden", recipient.Target);
                    break;
            }

            return this.MakeRequest<PushResponse>("pushes", recipient.AccessToken, request);
        }

        public object MakeRequest(string route, string accessToken, object request = null, string version = "v2")
        {
            return this.MakeRequest<object>(route, accessToken, request, version);
        }

        public T MakeRequest<T>(string route, string accessToken, object request = null, string version = "v2")
        {
            WebClient wc = new WebClient();

            wc.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + accessToken);
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            var uri = new Uri(string.Concat(UrlApiBase, version, "/", route));

            try
            {
                string requestJson = request == null ? string.Empty : JsonConvert.SerializeObject(request);

                var responseJson = string.IsNullOrEmpty(requestJson) ?
                    wc.DownloadString(uri) :
                    wc.UploadString(uri, requestJson);

                return JsonConvert.DeserializeObject<T>(responseJson);
            }
            catch (WebException x)
            {
                /*
                  HTTP Responses from Pushbullet API documentation
                    200 OK - Everything worked as expected.
                    400 Bad Request - Usually this results from missing a required parameter.
                    401 Unauthorized - No valid access token provided.
                    403 Forbidden - The access token is not valid for that request.
                    404 Not Found - The requested item doesn't exist.
                    429 Too Many Requests - You have been ratelimited for making too many requests to the server.
                    5XX Server Error - Something went wrong on Pushbullet's side.
                 */

                switch (((HttpWebResponse)x.Response).StatusCode)
                {
                    case HttpStatusCode.OK:
                        throw new Exception("Internal error. Success treated as exception.");

                    case HttpStatusCode.BadRequest:
                        throw new Exception("Invalid request or missing parameter.");

                    case HttpStatusCode.Unauthorized:
                        throw new Exception("Invalid access token.");

                    case HttpStatusCode.Forbidden:
                        throw new Exception("The access token is not valid for that request.");

                    case HttpStatusCode.NotFound:
                        throw new Exception("The requested item doesn't exist.");

                    case (HttpStatusCode)429:
                        throw new Exception("Too many requests.");

                    case HttpStatusCode.InternalServerError:
                        throw new Exception("Pushbullet internal error.");

                    default:
                        throw;
                }
            }
        }
    }
}
