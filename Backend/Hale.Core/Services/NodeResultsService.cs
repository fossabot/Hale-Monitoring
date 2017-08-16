using System;
using System.Collections.Generic;
using Hale.Core.Controllers;
using Hale.Core.Model.Interfaces;
using System.Linq;

namespace Hale.Core.Services
{
    public class NodeResultsService : HaleBaseService, INodeResultsService
    {
        public IList<NodeResultDTO> List(int id)
        {
            var results = _db.Results
                .Where(x => x.HostId == id)
                .GroupBy(x => new { x.HostId, x.FunctionId, x.Target })
                .Select(x => x.OrderByDescending(r => r.ExecutionTime).FirstOrDefault())
                .Select(x => new NodeResultDTO
                {
                    NodeId = x.HostId,
                    FunctionId = x.FunctionId,
                    ModuleIdentifier = _db.Modules.FirstOrDefault(r => r.Id == x.ModuleId).Identifier,
                    FunctionName = _db.Functions.FirstOrDefault(r => r.Id == x.FunctionId).Name,
                    Message = x.Message,
                    Target = x.Target,
                })
                .ToList();
            return results;
        }
    }
}
