namespace Hale.Core.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Hale.Core.Controllers;
    using Hale.Core.Model.Interfaces;

    public class NodeResultsService : HaleBaseService, INodeResultsService
    {
        public IList<NodeResultDTO> List(int id)
        {
            var results = this.Db.Results
                .Where(x => x.HostId == id)
                .GroupBy(x => new { x.HostId, x.FunctionId, x.Target })
                .Select(x => x.OrderByDescending(r => r.ExecutionTime).FirstOrDefault())
                .Select(x => new NodeResultDTO
                {
                    NodeId = x.HostId,
                    FunctionId = x.FunctionId,
                    ModuleIdentifier = this.Db.Modules.FirstOrDefault(r => r.Id == x.ModuleId).Identifier,
                    FunctionName = this.Db.Functions.FirstOrDefault(r => r.Id == x.FunctionId).Name,
                    Message = x.Message,
                    Target = x.Target,
                })
                .ToList();
            return results;
        }
    }
}
