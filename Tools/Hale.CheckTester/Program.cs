namespace Hale.CheckTester
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CommandLine;
    using CommandLine.Text;
    using Hale.Lib.ModuleLoader;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Alerts;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using NLog;
    using YamlDotNet.Serialization;

    internal class Program : MarshalByRefObject
    {
        private static CheckSettings checkSettings;
        private static Options opts;

        private static ILogger log = LogManager.GetLogger("HaleModuleTester");

        private static void Main(string[] args)
        {
            Console.Title = "Hale Module Tester";

            var v = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine($"Hale Module Tester v{v.Major}.{v.Minor} build {v.Build} release {v.Revision}, copyright (c) 20{v.Build / 1000}, Hale Project.\n");

            opts = new Options();

            var parser = new Parser(config => config.HelpWriter = Console.Out);

            if (parser.ParseArguments(args, opts))
            {
                if (string.IsNullOrEmpty(opts.ModulePath))
                {
                    opts.ModulePath = Path.GetDirectoryName(Path.GetFullPath(opts.Dll));
                    opts.Dll = Path.GetFileName(opts.Dll);
                }

                if (HasAnyTargets(opts))
                {
                    opts.CheckTargets.Add("default:default");
                }

                if (opts.CheckTargets.Count > 0)
                {
                    log.Info("Running module check tasks:");
                    var checkTargets = ProcessTargets(opts.CheckTargets);

                    foreach (var checkTarget in checkTargets)
                    {
                        checkSettings = new CheckSettings();
                        checkSettings.Thresholds = new CheckThresholds() { Critical = 1.0F, Warning = 0.5F };
                        RunCheckTask(checkTarget.Key, checkTarget.Value, checkSettings);
                    }
                }

                if (opts.InfoTargets.Count > 0)
                {
                    log.Info("Running module info tasks:");

                    var infoTargets = ProcessTargets(opts.InfoTargets);
                    foreach (var infoTarget in infoTargets)
                    {
                        var infoSettings = new InfoSettings();
                        RunInfoTask(infoTarget.Key, infoTarget.Value, infoSettings);
                    }
                }

                if (opts.ActionTargets.Count > 0)
                {
                    log.Info("Running module action tasks:");

                    var actionTargets = ProcessTargets(opts.ActionTargets);
                    foreach (var actionTarget in actionTargets)
                    {
                        var actionSettings = new ActionSettings();
                        RunActionTask(actionTarget.Key, actionTarget.Value, actionSettings);
                    }
                }

                if (opts.AlertTargets.Count > 0)
                {
                    log.Info("Running module alert tasks:");

                    var alertTargets = ProcessTargets(opts.AlertTargets);
                    foreach (var alertTarget in alertTargets)
                    {
                        var actionSettings = new AlertSettings();
                        RunAlertTask(alertTarget.Key, alertTarget.Value, actionSettings);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error parsing command line arguments.");
                var help = new HelpText();
                if (opts.LastParserState.Errors.Any())
                {
                    var errors = help.RenderParsingErrorsText(opts, 2); // indent with two spaces

                    if (!string.IsNullOrEmpty(errors))
                    {
                        help.AddPreOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                        help.AddPreOptionsLine(errors);
                    }
                }

                Console.WriteLine(help);
            }

            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static bool HasAnyTargets(Options options)
        {
            return options.CheckTargets.Count < 1
                && options.ActionTargets.Count < 1
                && options.InfoTargets.Count < 1
                && options.AlertTargets.Count < 1;
        }

        private static Dictionary<string, List<string>> ProcessTargets(List<string> rawTargets)
        {
            var results = new Dictionary<string, List<string>>();

            foreach (var rawTarget in rawTargets)
            {
                string name = "default";
                string target = "default";
                if (rawTarget.IndexOf(':') >= 0)
                {
                    var targetParts = rawTarget.Split(':');
                    name = targetParts[0];
                    target = targetParts[1];
                }
                else
                {
                    target = rawTarget;
                }

                if (!results.ContainsKey(name))
                {
                    results.Add(name, new List<string>());
                }

                results[name].Add(target);
            }

            return results;
        }

        private static void RunTask<T>(string name, string type, object settings)
        {
            var checkName = Path.GetFileNameWithoutExtension(opts.Dll);
            var taskName = $"{checkName}({name})";

            Console.Title = taskName + " - Hale Module Tester";

            log.Info($"Executing {type} task {taskName}...");

            try
            {
                var added = DateTime.UtcNow;

                var checkPath = opts.ModulePath;
                var dll = Path.GetFullPath(Path.Combine(checkPath, opts.Dll));

                if (!File.Exists(dll))
                {
                    throw new FileNotFoundException($"Module DLL \"{dll}\" not found!");
                }

                if (typeof(T) == typeof(CheckSettings))
                {
                    var result = ModuleLoader.ExecuteFunction<CheckResultSet>(opts.Dll, opts.ModulePath, name, settings as CheckSettings);
                    if (result == null)
                    {
                        throw new Exception("Returned null result!");
                    }

                    if (!result.RanSuccessfully)
                    {
                        throw result.FunctionException;
                    }

                    foreach (var cr in result.CheckResults)
                    {
                        log.Info($"Check {taskName}:\"{cr.Key}\" returned raw values {cr.Value.RawValues}.");
                        log.Info($"  -> {cr.Value.Message}");
                    }
                }
                else if (typeof(T) == typeof(InfoSettings))
                {
                    var result = ModuleLoader.ExecuteFunction<InfoResultSet>(opts.Dll, opts.ModulePath, name, settings as InfoSettings);
                    if (result == null)
                    {
                        throw new Exception("Returned null result!");
                    }

                    if (!result.RanSuccessfully)
                    {
                        throw result.FunctionException;
                    }

                    foreach (var ir in result.InfoResults)
                    {
                        log.Info($"Info {taskName}:\"{ir.Key}\" returned info items {ir.Value.ItemsAsString()}.");
                        log.Info($"  -> {ir.Value.Message}");
                    }
                }
                else if (typeof(T) == typeof(ActionSettings))
                {
                    var result = ModuleLoader.ExecuteFunction<ActionResultSet>(opts.Dll, opts.ModulePath, name, settings as ActionSettings);
                    if (result == null)
                    {
                        throw new Exception("Returned null result!");
                    }

                    if (!result.RanSuccessfully)
                    {
                        throw result.FunctionException;
                    }

                    foreach (var ar in result.ActionResults)
                    {
                        log.Info($"Action {taskName}:\"{ar.Key}\" executed.");
                        log.Info($"  -> {ar.Value.Message}");
                    }
                }
                else if (typeof(T) == typeof(AlertSettings))
                {
                    var result = ModuleLoader.ExecuteFunction<AlertResultSet>(opts.Dll, opts.ModulePath, name, settings as AlertSettings);
                    if (result == null)
                    {
                        throw new Exception("Returned null result!");
                    }

                    if (!result.RanSuccessfully)
                    {
                        throw result.FunctionException;
                    }

                    foreach (var ar in result.AlertResults)
                    {
                        log.Info($"Alert {taskName}:\"{ar.Key}\" executed.");
                        log.Info($"  -> {ar.Value.Message}");
                    }
                }

                var completed = DateTime.UtcNow;
                log.Info($"The {type} task {taskName} completed in {(completed - added).TotalSeconds.ToString("F2")} second(s)");
            }
            catch (Exception x)
            {
                log.Error($"Error running {type} task {taskName}: {x.Message}");
                if (x.InnerException != null)
                {
                    log.Error($" - Inner exception: {x.InnerException.Message}");
                }

                log.Warn("Stacktrace:\n" + x.StackTrace);
            }
        }

        private static void RunCheckTask(string name, List<string> targets, CheckSettings settings)
        {
            var ed = new Dictionary<string, string>();
            foreach (var target in targets)
            {
                settings.TargetSettings.Add(target, ed);
            }

            RunTask<CheckSettings>(name, "check", settings);
        }

        private static void RunInfoTask(string name, List<string> targets, InfoSettings settings)
        {
            var ed = new Dictionary<string, string>();
            foreach (var target in targets)
            {
                settings.TargetSettings.Add(target, ed);
            }

            RunTask<InfoSettings>(name, "info", settings);
        }

        private static void RunActionTask(string name, List<string> targets, ActionSettings settings)
        {
            var ed = new Dictionary<string, string>();
            foreach (var target in targets)
            {
                settings.TargetSettings.Add(target, ed);
            }

            RunTask<ActionSettings>(name, "action", settings);
        }

        private static void RunAlertTask(string name, List<string> targets, AlertSettings settings)
        {
            var yds = new DeserializerBuilder().WithNamingConvention(new YamlDotNet.Serialization.NamingConventions.CamelCaseNamingConvention()).Build();

            settings.Message = "Foo and Bar is Baz!";
            settings.SourceModule = new VersionedIdentifier("com.example.dummy", new Version(1, 0, 16054, 99));
            settings.SourceFunctionType = ModuleFunctionType.Check;
            settings.SourceFunction = "DummyCheck";
            settings.SourceTarget = "DummyTarget";

            var moduleConfig = Path.Combine(opts.ModulePath, "alert_" + name + ".yaml");

            if (File.Exists(moduleConfig))
            {
                using (var sr = File.OpenText(moduleConfig))
                {
                    var conf = yds.Deserialize<Dictionary<string, string>>(sr);
                    foreach (var vkp in conf)
                    {
                        settings[vkp.Key] = vkp.Value;
                    }
                }
            }

            foreach (var target in targets)
            {
                var targetConfig = Path.Combine(opts.ModulePath, $"alert_{name}_{target}.yaml");
                var targetSettings = new Dictionary<string, string>();
                if (File.Exists(targetConfig))
                {
                    using (var sr = File.OpenText(targetConfig))
                    {
                        var conf = yds.Deserialize<Dictionary<string, string>>(sr);
                        foreach (var vkp in conf)
                        {
                            targetSettings[vkp.Key] = vkp.Value;
                        }
                    }
                }

                settings.TargetSettings.Add(target, targetSettings);
            }

            RunTask<AlertSettings>(name, "alert", settings);
        }
    }
}
