namespace Hale.Modules
{
    using System;
    using System.IO;
    using System.Text;
    using Hale.Lib.Modules;
    using Hale.Lib.Modules.Attributes;
    using Hale.Lib.Modules.Checks;
    using Hale.Lib.Modules.Info;
    using Hale.Lib.Modules.Results;
    using Hale.Lib.Utilities;

    [HaleModule("com.itshale.core.storage", 0, 1, 1)]
    [HaleModuleName("Storage Module")]
    public sealed class StorageModule
    {
        [ModuleFunction(ModuleFunctionType.Check, Default = true, Identifier = "usage", TargetMode = TargetMode.Multiple)]
        public CheckResultSet CheckUsage(CheckSettings settings)
        {
            CheckResultSet result = new CheckResultSet();

            var sb = new StringBuilder();

            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in drives)
                {
                    var driveName = drive.Name.ToLower().Replace(":\\", string.Empty);
                    if (settings.Targetless || settings.Targets.Contains(driveName))
                    {
                        float diskPercentage = (float)drive.TotalFreeSpace / drive.TotalSize;

                        var cr = new CheckResult();
                        cr.Target = driveName;

                        cr.Message = $"{drive.Name} ({drive.VolumeLabel}) {StorageUnitFormatter.HumanizeStorageUnit(drive.TotalFreeSpace)}of {StorageUnitFormatter.HumanizeStorageUnit(drive.TotalSize)}free ({diskPercentage.ToString("P1")}).";

                        cr.RawValues.Add(new DataPoint("freePercentage", diskPercentage));
                        cr.RawValues.Add(new DataPoint("freeBytes", drive.TotalSize - drive.TotalFreeSpace));

                        cr.SetThresholds(diskPercentage, settings.Thresholds);

                        cr.RanSuccessfully = true;

                        result.CheckResults.Add(driveName, cr);
                    }
                }

                if (!settings.Targetless)
                {
                    foreach (var target in settings.Targets)
                    {
                        if (!result.CheckResults.ContainsKey(target))
                        {
                            result.CheckResults.Add(target, new CheckResult()
                            {
                                Message = $"Could not retrieve disk space for disk \"{target}\"",
                                ExecutionException = new Exception("Target not found."),
                                RanSuccessfully = false
                            });
                        }
                    }
                }

                result.Message = $"Retrieved space usage for {result.CheckResults.Count} drive(s).";
                result.RanSuccessfully = true;
            }
            catch (Exception e)
            {
                result.Message = "Failed to get disk space usage.";
                result.FunctionException = e;
                result.RanSuccessfully = false;
            }

            return result;
        }

        public InfoResultSet InfoListVolumes(InfoSettings settings)
        {
            var result = new InfoResultSet();
            result.FunctionException = new NotImplementedException();
            return result;
        }
    }
}
