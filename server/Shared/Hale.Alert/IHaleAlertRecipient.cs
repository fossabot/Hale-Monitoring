namespace Hale.Alert
{
    using System;

    public interface IHaleAlertRecipient
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}
