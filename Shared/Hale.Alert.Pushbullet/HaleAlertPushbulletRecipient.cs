namespace Hale.Alert.Pushbullet
{
    using System;

    public class HaleAlertPushbulletRecipient : IHaleAlertRecipient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Target { get; set; }

        public PushbulletPushTarget TargetType { get; set; }

        public string AccessToken { get; set; }
    }
}
