using Hale.Agent.Communication;
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
        private void internalTaskUploadResults()
        {
            var nemesis = ServiceProvider.GetService<NemesisController>();
            if (nemesis == null) return;

            var resultStorage = ServiceProvider.GetService<IResultStorage>();
            if (resultStorage == null) return;

            var records = resultStorage.Fetch(20);
            var uploaded = nemesis.UploadResults(records);

            if (uploaded != null)
                resultStorage.Clear(uploaded);
        }
    }
}
