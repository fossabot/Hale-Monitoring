namespace Hale.Lib.Modules.Attributes
{
    public sealed class CheckFunctionAttribute : ModuleFunctionAttribute
    {
        public CheckFunctionAttribute()
            : base(ModuleFunctionType.Check)
        {
        }
    }
}
