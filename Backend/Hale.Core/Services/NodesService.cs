namespace Hale.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hale.Core.Data.Entities;
    using Hale.Core.Data.Entities.Nodes;
    using Hale.Core.Model.Interfaces;
    using Hale.Core.Model.Models;
    using Hale.Core.Models.Messages;
    using Microsoft.EntityFrameworkCore;

    public class NodesService : HaleBaseService, INodesService
    {
        public NodeDTO GetNodeById(int id)
        {
            var node = this.Db.Nodes
                .Where(x => x.Id == id)
                .Include(h => h.NodeDetails)
                .Select(x => new NodeDTO
                {
                    Id = x.Id,
                    FriendlyName = x.FriendlyName,
                    HostName = x.HostName,
                    Domain = x.Domain,
                    OperatingSystem = x.OperatingSystem,
                    NicSummary = x.NicSummary,
                    HardwareSummary = x.HardwareSummary,
                    Ip = x.Ip,
                    Status = x.Status,
                    LastConnected = x.LastConnected,
                    Modified = x.Modified,
                    ModifiedBy = this.Db.Accounts.FirstOrDefault(a => a.Id == x.ModifiedBy),
                    Created = x.Created,
                    ConfiguredBy = this.Db.Accounts.FirstOrDefault(a => a.Id == x.ConfiguredBy),
                    Guid = x.Guid,
                    NodeDetails = x.NodeDetails,
                    Configured = x.Configured,
                    Blocked = x.Blocked
                })
                .FirstOrDefault();

            return node;
        }

        public IList<NodeSummaryDTO> List()
        {
            var nodeSummaries = this.Db.Nodes
                .Include(h => h.NodeDetails)
                .Select(x => new NodeSummaryDTO
                {
                    Id = x.Id,
                    FriendlyName = x.FriendlyName,
                    HostName = x.HostName,
                    LastChange = x.Modified,
                    Status = x.Status
                })
                .ToList();

            return nodeSummaries;
        }

        public void Update(int id, NodeDTO nodeToSave, string currentUsername)
        {
            var user = this.Db.Accounts.First(x => x.UserName == currentUsername);
            var host = this.Db.Nodes.FirstOrDefault(x => x.Id == id);

            if (host == null)
            {
                throw new ArgumentException();
            }

            host.FriendlyName = nodeToSave.FriendlyName;
            host.Domain = nodeToSave.Domain;
            host.ModifiedBy = user.Id;
            host.ConfiguredBy = user.Id;

            host.Configured = !nodeToSave.Blocked;
            host.Blocked = nodeToSave.Blocked;

            this.Db.SaveChanges();
        }

        public IList<NodeCommentDTO> GetComments(int id)
        {
            var comments = this.Db.NodeComments
                .Include("User")
                .Where(x => x.Node.Id == id)
                .Select(x => new NodeCommentDTO
                {
                    Id = x.Id,
                    Node = x.Node,
                    User = x.User,
                    Timestamp = x.Timestamp,
                    Text = x.Text
                })
                .ToList();

            return comments;
        }

        public void SaveComment(int id, NewCommentDTO newComment, string currentUsername)
        {
            var user = this.Db.Accounts.First(x => x.UserName == currentUsername);
            var node = this.Db.Nodes.FirstOrDefault(x => x.Id == id);

            if (node == null)
            {
                throw new ArgumentException();
            }

            var comment = new NodeComment()
            {
                Text = newComment.Text,
                Timestamp = DateTime.UtcNow,
                User = user,
                Node = node
            };

            this.Db.NodeComments.Add(comment);
            this.Db.SaveChanges();
        }

        public void DeleteComment(int id, int commentId)
        {
            var comment = this.Db.NodeComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
            {
                throw new ArgumentException();
            }

            this.Db.NodeComments.Remove(comment);
            this.Db.SaveChanges();
        }
    }
}
