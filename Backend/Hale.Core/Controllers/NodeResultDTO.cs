using System.Linq;
using Hale.Core.Models.Modules;
using System.Collections.Generic;

namespace Hale.Core.Controllers
{
    internal class NodeResultDTO
    {
        public int NodeId { get; set; }
        public string Target { get; set; }
        public int FunctionId { get; set; }
        public string ModuleIdentifier { get; set; }
        public string FunctionName { get; set; }
    }
}