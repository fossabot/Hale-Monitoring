using Hale.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Agent.Modules
{
    partial class AgentScheduler
    {
        private void internalTaskPersistResults()
        {
            var resultStorage = ServiceProvider.GetService<IResultStorage>();
            if (resultStorage == null) return;

            resultStorage.Persist();
        }
    }
}
