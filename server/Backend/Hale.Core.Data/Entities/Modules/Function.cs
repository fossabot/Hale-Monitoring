namespace Hale.Core.Data.Entities.Modules
{
    using Hale.Lib.Modules;

    public class Function
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Description { get; set; }

        public ModuleFunctionType Type { get; set; }

        public Module Module { get; set; }
    }
}
