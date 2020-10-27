using EarTube.Models;
using System.Threading.Tasks;

namespace EarTube.Repository
{
    public interface ICommentRepository
    {
        void AddNewComment(Comment comment);
        Comment CommentById(int? id);
        Task<bool> CommentDislike(Comment model, string userId);
        Task<bool> CommentLike(Comment model, string userId);
        void Save();
    }
}