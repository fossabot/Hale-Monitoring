namespace Hale.Lib.Modules.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class ReturnUnitAttribute : Attribute
    {
        public ReturnUnitAttribute(string identifier, UnitType unitType)
        {
            this.Identifier = identifier;
            this.Type = unitType;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public UnitType Type { get; set; }

        public string Precision { get; set; }

        public string Identifier { get; internal set; }
    }
}
