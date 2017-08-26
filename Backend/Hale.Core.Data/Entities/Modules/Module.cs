#pragma warning disable CS3003 // Type is not CLS-compliant

namespace Hale.Core.Data.Entities.Modules
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Hale.Lib.Modules;
    using Semver;

    public class Module
    {
        private int? major;
        private int? minor;
        private int? revision;

        public Module()
        {
        }

        public Module(VersionedIdentifier vi)
        {
            this.Identifier = vi.Identifier;
            this.Version = vi.Version;
        }

        public int Id { get; set; }

        public string Identifier { get; set; }

        [NotMapped]
        public SemVersion Version { get; set; }

        public int Major
        {
            get { return this.Version?.Major ?? 0; }
            set { this.UpdateVersion(major: value); }
        }

        public int Minor
        {
            get { return this.Version?.Minor ?? 0; }
            set { this.UpdateVersion(minor: value); }
        }

        public int Revision
        {
            get { return this.Version?.Patch ?? 0; }
            set { this.UpdateVersion(revision: value); }
        }

        private void UpdateVersion(int? major = null, int? minor = null, int? revision = null)
        {
            if (major.HasValue)
            {
                this.major = major;
            }

            if (minor.HasValue)
            {
                this.minor = minor;
            }

            if (revision.HasValue && revision.Value >= 0)
            {
                this.revision = revision;
            }

            if (this.major.HasValue && this.minor.HasValue && this.revision.HasValue)
            {
                this.Version = new SemVersion(this.major.Value, this.minor.Value, this.revision.Value);
            }
        }
    }
}
