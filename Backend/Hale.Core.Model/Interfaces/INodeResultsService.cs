namespace Hale.Core.Model.Interfaces
{
    using System.Collections.Generic;
    using Hale.Core.Controllers;

    public interface INodeResultsService
    {
        IList<NodeResultDTO> List(int id);
    }
}
