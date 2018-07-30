namespace Hale.Core.Data.Entities.Users
{
    using Newtonsoft.Json;

    /// <summary>
    /// Corresponds to the database table Security.AccountDetails
    /// </summary>
    public class AccountDetail
    {
        /// <summary>
        /// Corresponds to the table column AccountDetails.Id
        /// </summary>
        [JsonProperty("_id")]
        public int Id { get; set; }

        /// <summary>
        /// Corresponds to the table column AccountDetails.UserId
        /// </summary>
        [JsonProperty("userId")]
        public int UserId { get; set; }

        /// <summary>
        /// Corresponds to the table column AccountDetails.Key
        /// </summary>
        [JsonProperty("id")]
        public string Key { get; set; }

        /// <summary>
        /// Corresponds to the table column AccountDetails.Value
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}