namespace Hale.Agent.Scheduler
{
    using System.Linq;
    using Hale.Lib;
    using Hale.Lib.Modules;

    internal class ModuleTask : TaskBase
    {
        public string Module { get; set; }

        public string Function { get; set; }

        public string[] Targets
            => this.Settings.Targets.ToArray();

        public ModuleFunctionType FunctionType { get; set; }

        public ModuleSettingsBase Settings { get; set; }

        public override string ToString()
            => $"<{this.Module}[{this.FunctionType}]{this.Function}({string.Join(",", this.Targets)})>";

        public override QueuedTask ToQueued()
        {
            return new QueuedModuleTask() { Task = this };
        }
    }
}
