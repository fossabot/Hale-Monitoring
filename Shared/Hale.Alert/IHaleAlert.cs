namespace Hale.Alert
{
    using System;
    using Config = System.Collections.Generic.Dictionary<string, string>;

    public interface IHaleAlert
    {
        string Name { get; }

        Version Version { get; }

        decimal TargetApi { get; }

        bool Ready { get; }

        // Initialize should attempt to verfiy the configureation and throw an exception if problems were detected.
        void Initialize(Config config);

        void Send(string message, string source, IHaleAlertRecipient[] recipients);
    }
}
