using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hale.Lib.Modules;
using Hale.Lib.Modules.Checks;
using Hale.Lib.Modules.Alerts;
using Hale.Lib.Modules.Actions;
using Hale.Lib.Modules.Info;
using HaleModule= Hale.Lib.Modules.Module;
using System.Linq;
using Hale.Lib.Modules.Attributes;

namespace Hale.Lib.ModuleLoader
{
    class ModuleDomain: MarshalByRefObject
    {
        ILogger _log;

        public void InitLogging()
        {
            var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(appPath, "nlog.config"));

            _log = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);
            _log.Debug($"AppDomain Sandbox for \"{AppDomain.CurrentDomain.FriendlyName}\" created.");
        }

        public T GetProvider<T>(string dll, out HaleModule module) where T: class, IModuleProviderBase
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

            foreach(var ta in type.CustomAttributes.Where(ca => ca.AttributeType == typeof(HaleModuleAttribute)))
            {
                _log.Debug($"[{ta.AttributeType.Name}]");
                foreach(var ca in ta.ConstructorArguments)
                {
                    _log.Debug($"  Constructor Argument <{ca.ArgumentType.Name}> = {ca.Value}");
                }
                foreach (var na in ta.NamedArguments)
                {
                    _log.Debug($"  Named Argument <{na.TypedValue.ArgumentType.Name}> {na.MemberName} = {na.TypedValue.Value}");
                }
            }

            foreach(var mm in type.GetMethods())
            {
                if (mm.IsSpecialName) continue;

                _log.Debug($"Public method \"{mm.Name}\"");
                foreach(var ma in mm.CustomAttributes)
                {
                    if (!typeof(ModuleFunctionAttribute).IsAssignableFrom(ma.AttributeType)) continue;
                    _log.Debug($"[{ma.AttributeType.Name}]");
                    foreach (var ca in ma.ConstructorArguments)
                    {
                        _log.Debug($"  Constructor Argument <{ca.ArgumentType.Name}> = {ca.Value}");
                    }
                    foreach (var na in ma.NamedArguments)
                    {
                        _log.Debug($"  Named Argument <{na.TypedValue.ArgumentType.Name}> {na.MemberName} = {na.TypedValue.Value}");
                    }
                }
            }

            var instance = Activator.CreateInstance(type);
            var provider = (T)instance;
            module = (HaleModule)instance;

            _log.Debug($"Loaded module <{VersionedIdentifier.ToString(module)}> {module.Name}");

            return provider;
        }

        private ModuleFunctionResult _addVersionInfo(ModuleFunctionResult result, HaleModule module)
        {
            if(result.Module == null)
            {
                result.Module = new VersionedIdentifier(module);
            }
            return result;
        }

        public CheckFunctionResult ExecuteCheckFunction(string dll, string name, CheckSettings settings)
        {
            InitLogging();
            HaleModule module;
            var check = GetProvider<ICheckProvider>(dll, out module);
            check.InitializeCheckProvider(settings);
            var result = check.ExecuteCheckFunction(name, settings);
            return _addVersionInfo(result, module) as CheckFunctionResult;
        }


        public ActionFunctionResult ExecuteActionFunction(string dll, string name, ActionSettings settings)
        {
            InitLogging();
            HaleModule module;
            var action = GetProvider<IActionProvider>(dll, out module);
            action.InitializeActionProvider(settings);
            var result = action.ExecuteActionFunction(name, settings);
            return _addVersionInfo(result, module) as ActionFunctionResult;
        }


        public InfoFunctionResult ExecuteInfoFunction(string dll, string name, InfoSettings settings)
        {
            InitLogging();
            HaleModule module;
            var info = GetProvider<IInfoProvider>(dll, out module);
            info.InitializeInfoProvider(settings);
            var result = info.ExecuteInfoFunction(name, settings);
            return _addVersionInfo(result, module) as InfoFunctionResult;
        }

        public AlertFunctionResult ExecuteAlertFunction(string dll, string name, AlertSettings settings)
        {
            InitLogging();
            HaleModule module;
            var alert = GetProvider<IAlertProvider>(dll, out module);
            alert.InitializeAlertProvider(settings);
            var result = alert.ExecuteAlertFunction(name, settings);
            return _addVersionInfo(result, module) as AlertFunctionResult;
        }

        public ModuleRuntimeInfo GetModuleInfo(string dll)
        {
            var mri = ModuleRuntimeInfo.Empty;

            InitLogging();

            var assembly = Assembly.LoadFile(dll);

            var type = assembly.ExportedTypes
                .SingleOrDefault(t => t.CustomAttributes.Where(ca => ca.AttributeType == typeof(HaleModuleAttribute)).Count() > 0);


            if (type == null)
            {
                throw new Exception("Assembly does not contain a valid Module class");
            }

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
                if (mm.IsSpecialName) continue;

                var mfri = new ModuleFunctionRuntimeInfo();
                List<string> idents = new List<string>();
                ModuleFunctionType functype = ModuleFunctionType.None;

                foreach (var ma in mm.GetCustomAttributes())
                {

                    switch (ma)
                    {
                        case ModuleFunctionAttribute ca:

                            if (!string.IsNullOrEmpty(ca.Identifier))
                                idents.Add(ca.Identifier);
                            if (ca.Default)
                                idents.Add("default");
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

                if (functype == ModuleFunctionType.None) continue;

                _log.Debug($"Found module function <{functype}> \"{mm.Name}\" ({string.Join(", ", idents)}) => [{string.Join(", ", mfri.Returns.Keys)}] ");

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

    }
}
