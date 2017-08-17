namespace Hale.Lib.Modules.Alerts
{
    using System;

    public interface IAlertRecipient
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}
