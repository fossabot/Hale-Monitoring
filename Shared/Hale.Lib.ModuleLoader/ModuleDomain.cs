namespace Hale.Lib.ModuleLoader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Actions;
    using Hale.Lib.Modules.Alerts;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using NLog;
    using NLog.Config;
    using HaleModule = Hale.Lib.Modules.Module;

    internal class ModuleDomain : MarshalByRefObject
    {
        private ILogger log;

        public void InitLogging()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(appPath, "nlog.config"));

            this.log = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);
            this.log.Debug($"AppDomain Sandbox for \"{AppDomain.CurrentDomain.FriendlyName}\" created.");
        }

        public T GetProvider<T>(string dll, out HaleModule module)
            where T : class, IModuleProviderBase
        {
            var providerInterface = typeof(T);

            var assembly = Assembly.LoadFile(dll);

            Type type = null;
            foreach (var t in assembly.ExportedTypes)
            {
                if (providerInterface.IsAssignableFrom(t))
                {
                    type = t;
                    break;
                }
            }

            if (type == null)
            {
                module = null;
                return null;
            }

            foreach (var ta in type.CustomAttributes.Where(ca => ca.AttributeType == typeof(HaleModuleAttribute)))
            {
                this.log.Debug($"[{ta.AttributeType.Name}]");
                foreach (var ca in ta.ConstructorArguments)
                {
                    this.log.Debug($"  Constructor Argument <{ca.ArgumentType.Name}> = {ca.Value}");
                }

                foreach (var na in ta.NamedArguments)
                {
                    this.log.Debug($"  Named Argument <{na.TypedValue.ArgumentType.Name}> {na.MemberName} = {na.TypedValue.Value}");
                }
            }

            foreach (var mm in type.GetMethods())
            {
                if (mm.IsSpecialName)
                {
                    continue;
                }

                this.log.Debug($"Public method \"{mm.Name}\"");
                foreach (var ma in mm.CustomAttributes)
                {
                    if (!typeof(ModuleFunctionAttribute).IsAssignableFrom(ma.AttributeType))
                    {
                        continue;
                    }

                    this.log.Debug($"[{ma.AttributeType.Name}]");
                    foreach (var ca in ma.ConstructorArguments)
                    {
                        this.log.Debug($"  Constructor Argument <{ca.ArgumentType.Name}> = {ca.Value}");
                    }

                    foreach (var na in ma.NamedArguments)
                    {
                        this.log.Debug($"  Named Argument <{na.TypedValue.ArgumentType.Name}> {na.MemberName} = {na.TypedValue.Value}");
                    }
                }
            }

            var instance = Activator.CreateInstance(type);
            var provider = (T)instance;
            module = (HaleModule)instance;

            this.log.Debug($"Loaded module <{VersionedIdentifier.ToString(module)}> {module.Name}");

            return provider;
        }

        public CheckFunctionResult ExecuteCheckFunction(string dll, string name, CheckSettings settings)
        {
            this.InitLogging();
            var type = GetHaleModule(dll);
            var method = type.GetMethods().SingleOrDefault(mi =>
                mi.GetCustomAttributes().Any(ca =>
                    ca is ModuleFunctionAttribute
                    && ((ModuleFunctionAttribute)ca).Identifier == name));

            var module = Activator.CreateInstance(type) as HaleModule;
            var checkResult = method.Invoke(module, new object[] { settings }) as CheckFunctionResult;

            return this.AddVersionInfo(checkResult, module) as CheckFunctionResult;
        }

        public ActionFunctionResult ExecuteActionFunction(string dll, string name, ActionSettings settings)
        {
            this.InitLogging();
            HaleModule module;
            var action = this.GetProvider<IActionProvider>(dll, out module);
            action.InitializeActionProvider(settings);
            var result = action.ExecuteActionFunction(name, settings);

            return this.AddVersionInfo(result, module) as ActionFunctionResult;
        }

        public InfoFunctionResult ExecuteInfoFunction(string dll, string name, InfoSettings settings)
        {
            this.InitLogging();
            HaleModule module;
            var info = this.GetProvider<IInfoProvider>(dll, out module);
            info.InitializeInfoProvider(settings);
            var result = info.ExecuteInfoFunction(name, settings);
            return this.AddVersionInfo(result, module) as InfoFunctionResult;
        }

        public AlertFunctionResult ExecuteAlertFunction(string dll, string name, AlertSettings settings)
        {
            this.InitLogging();
            HaleModule module;
            var alert = this.GetProvider<IAlertProvider>(dll, out module);
            alert.InitializeAlertProvider(settings);
            var result = alert.ExecuteAlertFunction(name, settings);
            return this.AddVersionInfo(result, module) as AlertFunctionResult;
        }

        public ModuleRuntimeInfo GetModuleInfo(string dll)
        {
            var mri = ModuleRuntimeInfo.Empty;

            this.InitLogging();
            var type = GetHaleModule(dll);

            foreach (var typeAttribute in type.GetCustomAttributes())
            {
                switch (typeAttribute)
                {
                    case HaleModuleAttribute ca:
                        mri.Module = new VersionedIdentifier(ca.Identifier, ca.Version);
                        break;
                    case HaleModuleNameAttribute ca:
                        mri.Name = ca.Name;
                        break;
                    case HaleModuleDescriptionAttribute ca:
                        mri.Description = ca.Description;
                        break;
                    case HaleModuleAuthorAttribute ca:
                        mri.Author = ca.Author;
                        mri.Organization = ca.Organization;
                        break;
                }
            }

            foreach (var mm in type.GetMethods())
            {
                if (mm.IsSpecialName)
                {
                    continue;
                }

                var mfri = new ModuleFunctionRuntimeInfo();
                List<string> idents = new List<string>();
                ModuleFunctionType functype = ModuleFunctionType.None;

                foreach (var ma in mm.GetCustomAttributes())
                {
                    switch (ma)
                    {
                        case ModuleFunctionAttribute ca:

                            if (!string.IsNullOrEmpty(ca.Identifier))
                            {
                                idents.Add(ca.Identifier);
                            }

                            if (ca.Default)
                            {
                                idents.Add("default");
                            }

                            mfri.Name = ca.Name;
                            mfri.Description = ca.Description;
                            functype = ca.Type;
                            break;

                        case ReturnUnitAttribute ca:
                            mfri.Returns.Add(ca.Identifier, new ModuleFunctionReturnRuntimeInfo()
                            {
                                Description = ca.Description,
                                Name = ca.Name,
                                Precision = ca.Precision,
                                Type = ca.Type
                            });
                            break;
                    }
                }

                if (functype == ModuleFunctionType.None)
                {
                    continue;
                }

                this.log.Debug($"Found module function <{functype}> \"{mm.Name}\" ({string.Join(", ", idents)}) => [{string.Join(", ", mfri.Returns.Keys)}] ");

                foreach (var ident in idents)
                {
                    if (mri.Functions[functype].ContainsKey(ident))
                    {
                        throw new Exception("Two module functions of the same type cannot use the same identifier");
                    }
                    else
                    {
                        mri.Functions[functype].Add(ident, mfri);
                    }
                }
            }

            return mri;
        }

        private static Type GetHaleModule(string dll)
        {
            var assembly = Assembly.LoadFile(dll);

            var type = assembly.ExportedTypes
                .SingleOrDefault(t => t.CustomAttributes.Where(ca => ca.AttributeType == typeof(HaleModuleAttribute)).Count() > 0);

            if (type == null)
            {
                throw new Exception("Assembly does not contain a valid Module class");
            }

            return type;
        }

        private ModuleFunctionResult AddVersionInfo(ModuleFunctionResult result, HaleModule module)
        {
            if (result.Module == null)
            {
                result.Module = new VersionedIdentifier(module);
            }

            return result;
        }
    }
}
