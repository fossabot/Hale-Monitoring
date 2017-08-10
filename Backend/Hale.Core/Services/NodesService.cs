using Hale.Core.Model.Interfaces;
using Hale.Core.Model.Models;
using System.Data.Entity;
using System.Linq;
using Hale.Core.Data.Entities;
using System;
using System.Collections.Generic;
using Hale.Core.Models.Messages;
using Hale.Core.Data.Entities.Nodes;

namespace Hale.Core.Services
{
    public class NodesService: HaleBaseService, INodesService
    {
        public NodeDTO GetNodeById(int id)
        {
            var node = _db.Nodes
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
                    ModifiedBy = _db.Accounts.FirstOrDefault(a => a.Id == x.ModifiedBy),
                    Created = x.Created,
                    ConfiguredBy = _db.Accounts.FirstOrDefault(a => a.Id == x.ConfiguredBy),
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
            var nodeSummaries = _db.Nodes
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
            var user = _db.Accounts.First(x => x.UserName == currentUsername);
            var host = _db.Nodes.FirstOrDefault(x => x.Id == id);

            if (host == null)
                throw new ArgumentException();

            host.FriendlyName = nodeToSave.FriendlyName;
            host.Domain = nodeToSave.Domain;
            host.ModifiedBy = user.Id;
            host.ConfiguredBy = user.Id;

            host.Configured = !nodeToSave.Blocked;
            host.Blocked = nodeToSave.Blocked;

            _db.SaveChanges();
        }

        public IList<NodeCommentDTO> GetComments(int id)
        {
            var comments = _db.NodeComments
                .Include("User")
                .Where((x => x.Node.Id == id))
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
            var user = _db.Accounts.First(x => x.UserName == currentUsername);
            var node = _db.Nodes.FirstOrDefault(x => x.Id == id);

            if (node == null)
                throw new ArgumentException();

            var comment = new NodeComment()
            {
                Text = newComment.Text,
                Timestamp = DateTime.Now,
                User = user,
                Node = node
            };

            _db.NodeComments.Add(comment);
            _db.SaveChanges();
        }

        public void DeleteComment(int id, int commentId)
        {
            var comment = _db.NodeComments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null)
                throw new ArgumentException();

            _db.NodeComments.Remove(comment);
            _db.SaveChanges();
        }
    }
}
