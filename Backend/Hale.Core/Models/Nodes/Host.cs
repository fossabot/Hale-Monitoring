using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Hale.Core.Models.Nodes
{
    /// <summary>
    /// Corresponds to the database table Nodes.Hosts
    /// </summary>
    public class Host
    {
        /// <summary>
        /// Corresponds to the table column Hosts.Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Name
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.HostName
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///  Corresponds to the table column Hosts.LastSeen
        /// </summary>
        public DateTime? LastConnected { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Updated
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Added
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.Guid
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Corresponds to the table column Hosts.RsaKey.
        /// Is not serialized by the JsonApiAdapter.
        /// </summary>
        [IgnoreDataMember]
        public byte[] RsaKey { get; set; }

        /// <summary>
        /// Wrapper containing data in a one-to-many relationship to Nodes.HostDetails.
        /// </summary>
        public List<HostDetail> HostDetails { get; set; }
    }

    

    
}