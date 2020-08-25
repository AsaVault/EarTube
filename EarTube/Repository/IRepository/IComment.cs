using EarTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Repository.IRepository
{
    public interface IComment
    {
        bool CommentDislike(Comment model, string userId);
        bool CommentLike(Comment model, string userId);
        Comment CommentById(int? id);

        void AddNewComment(Comment comment);

        void Save();
    }
}
