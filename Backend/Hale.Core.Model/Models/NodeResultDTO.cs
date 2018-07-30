namespace Hale.Core.Controllers
{
    public class NodeResultDTO
    {
        public int NodeId { get; set; }

        public string Target { get; set; }

        public int FunctionId { get; set; }

        public string ModuleIdentifier { get; set; }

        public string FunctionName { get; set; }

        public string Message { get; set; }
    }
}