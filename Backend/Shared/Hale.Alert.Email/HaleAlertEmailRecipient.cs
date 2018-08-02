namespace Hale.Alert
{
    using System;

    public class HaleAlertEmailRecipient : IHaleAlertRecipient
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }
}
