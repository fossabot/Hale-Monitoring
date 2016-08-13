using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Contexts
{
    public class HaleDBConvention: Convention
    {
        public HaleDBConvention()
        {
            IPluralizationService ps =
                new EnglishPluralizationService();


            Types().Configure(
                c => c.ToTable(
                    // Table name is the pluralized name of the class
                    ps.Pluralize(c.ClrType.Name),

                    // Schema name is the name of the container namespace
                    c.ClrType.Namespace.Substring(c.ClrType.Namespace.LastIndexOf('.') + 1)
                )
            );
        }
    }
}
