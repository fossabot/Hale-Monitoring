namespace Hale.Alert
{
    using System;
    using System.Net.Mail;
    using Config = System.Collections.Generic.Dictionary<string, string>;

    public class HaleAlertEmail : IHaleAlert
    {
        private readonly Version version = new Version(0, 1, 1);

        private Config config;
        private SmtpClient smtp;

        public string Name
            => "Email";

        public Version Version
            => this.Version;

        public decimal TargetApi
            => 0.1M;

        public string InitResponse { get; private set; }

        public bool Ready { get; private set; }

        public void Initialize(Config config)
        {
            this.config = config;
            try
            {
                var useSsl = this.config.ContainsKey("smtp_ssl") && bool.Parse(this.config["smtp_ssl"]);
                var smtpHost = this.config["smtp_host"];
                var smtpPort = int.Parse(this.config["smtp_port"]);

                if (!useSsl)
                {
                    string response;
                    if (!SmtpHelper.TestConnection(smtpHost, smtpPort, out response))
                    {
                        this.InitResponse = response;
                        throw new HaleAlertInitializeException(new Exception("Cannot verify SMTP connection."));
                    }

                    this.InitResponse = response;
                }

                this.smtp = new SmtpClient(smtpHost, smtpPort) { EnableSsl = useSsl };

                this.Ready = true;
            }
            catch (Exception x)
            {
                throw new HaleAlertInitializeException(x);
            }
        }

        public void Send(string message, string source, IHaleAlertRecipient[] recipients)
        {
        }
    }
}
