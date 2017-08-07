using Hale.Core.Data.Entities;
using Hale.Core.Model.Models;
using System.Collections.Generic;
using Hale.Core.Models.Messages;

namespace Hale.Core.Model.Interfaces
{
    public interface INodesService
    {
        NodeDTO GetNodeById(int id);
        IList<NodeSummaryDTO> List();
        void Update(int id, NodeDTO nodeToSave, string currentUsername);
        IList<NodeCommentDTO> GetComments(int id);
        void SaveComment(int id, NewCommentDTO newComment, string currentUsername);
        void DeleteComment(int id, int commentId);
    }
}
