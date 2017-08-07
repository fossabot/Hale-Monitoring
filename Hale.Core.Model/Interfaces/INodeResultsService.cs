using Hale.Core.Controllers;
using System.Collections.Generic;

namespace Hale.Core.Model.Interfaces
{
    public interface INodeResultsService
    {
        IList<NodeResultDTO> List(int id);
    }
}
