namespace Hale.Lib.Modules.Attributes
{
    public sealed class InfoFunctionAttribute : ModuleFunctionAttribute
    {
        public InfoFunctionAttribute()
            : base(ModuleFunctionType.Info)
        {
        }
    }
}
