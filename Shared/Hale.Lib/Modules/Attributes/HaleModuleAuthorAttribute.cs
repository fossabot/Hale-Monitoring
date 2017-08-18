namespace Hale.Lib.Modules.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class HaleModuleAuthorAttribute : Attribute
    {
        public HaleModuleAuthorAttribute(string author)
        {
            this.Author = author;
        }

        public string Author { get; }

        public string Organization { get; set; }
    }
}