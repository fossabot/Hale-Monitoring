namespace Hale.Lib.Modules.Info
{
    using Hale.Lib.Modules.Results;

    public delegate InfoResultSet InfoFunction(InfoSettings settings);

    public delegate InfoResult SingleResultInfoFunction(InfoSettings settings);

    public interface IInfoProvider : IModuleProviderBase
    {
        void InitializeInfoProvider(InfoSettings settings);
    }
}
