using Hale.Core.Models;
using System.Collections.Generic;

namespace Hale.Core.Model.Interfaces
{
    public interface IConfigService
    {
        string GetConfigById(int id);
        IList<ConfigSummaryDTO> List();
        int SaveSerialized(int id, string serialized, string currentUsername);
    }
}
