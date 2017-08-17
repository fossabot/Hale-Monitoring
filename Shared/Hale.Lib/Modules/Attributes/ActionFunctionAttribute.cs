namespace Hale.Lib.Modules.Attributes
{
    public sealed class ActionFunctionAttribute : ModuleFunctionAttribute
    {
        public ActionFunctionAttribute()
            : base(ModuleFunctionType.Action)
        {
        }
    }
}
