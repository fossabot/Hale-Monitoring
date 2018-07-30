namespace Hale.Core.Data.Contexts
{
    /*
    using System.Data.Entity.Infrastructure.Pluralization;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Runtime.InteropServices;

    [ComVisible(false)]
    public class HaleDBConvention : Convention
    {
        public HaleDBConvention()
        {
            IPluralizationService ps =
                new EnglishPluralizationService();

            this.Types().Configure(
                c => c.ToTable(
                    ps.Pluralize(c.ClrType.Name),
                    c.ClrType.Namespace.Substring(c.ClrType.Namespace.LastIndexOf('.') + 1)));
        }
    }
    */
}
