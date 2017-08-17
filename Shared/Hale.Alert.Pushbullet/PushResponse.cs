namespace Hale.Alert.Pushbullet
{
    public class PushResponse
    {
        public string Iden { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public double Created { get; set; }

        public double Modified { get; set; }

        public bool Active { get; set; }

        public bool Dismissed { get; set; }

        public string SenderIden { get; set; }

        public string SenderEmail { get; set; }

        public string SenderEmailNormalized { get; set; }

        public string ReceiverIden { get; set; }

        public string ReceiverEmail { get; set; }

        public string ReceiverEmailNormalized { get; set; }
    }
}
