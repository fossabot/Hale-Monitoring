namespace Hale.Core.Model.Interfaces
{
    using System.Collections.Generic;
    using Hale.Core.Models;

    public interface IConfigService
    {
        string GetConfigById(int id);

        IList<ConfigSummaryDTO> List();

        int SaveSerialized(int id, string serialized, string currentUsername);
    }
}
