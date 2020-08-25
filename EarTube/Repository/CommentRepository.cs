using EarTube.Data;
using EarTube.Models;
using EarTube.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Repository
{

    public class CommentRepository : IComment
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddNewComment(Comment comment)
        {
            _db.Comment.Add(comment);
        }

        public Comment CommentById(int? id)
        {
            return _db.Comment.FirstOrDefault(x => x.Id == id);
        }

        public  bool CommentDislike(Comment model, string userId)
        {
            var dislikeComment = false;
            var userCommentDislike =  _db.UserCommentDislike.Any(u => u.UserId == userId && u.CommentId == model.Id);
            if (!userCommentDislike)
            {
                var newComment =  _db.Comment.FirstOrDefault(x => x.Id == model.Id);
                var userCommentLike =  _db.UserCommentLike.Any(u => u.UserId == userId && u.CommentId == model.Id);
                if (!userCommentLike)
                {
                    newComment.CommentDisikes += 1;
                }
                else
                {
                    var likeData =  _db.UserCommentLike.FirstOrDefault(u => u.UserId == userId && u.CommentId == model.Id);
                    _db.UserCommentLike.Remove(likeData);
                    newComment.CommentLikes -= 1;
                    newComment.CommentDisikes += 1;
                }

                _db.Comment.Update(newComment);
                _db.UserCommentDislike.Add(new UserCommentDislike { UserId = userId, CommentId = model.Id });
                 _db.SaveChangesAsync();

                dislikeComment = true;
            }
            return dislikeComment;
        }

        public  bool CommentLike(Comment model, string userId)
        {
            var likeComment = false;
            var userCommentLike =  _db.UserCommentLike.Any(u => u.UserId == userId && u.CommentId == model.Id);
            if (!userCommentLike)
            {
                var newComment =  _db.Comment.Where(x => x.Id == model.Id).FirstOrDefault();
                var userCommentDislike =  _db.UserCommentDislike.Any(u => u.UserId == userId && u.CommentId == model.Id);
                if (!userCommentDislike)
                {
                    newComment.CommentLikes += 1;
                }
                else
                {
                    var dislikeData =  _db.UserCommentDislike.FirstOrDefault(u => u.UserId == userId && u.CommentId == model.Id);
                    _db.UserCommentDislike.Remove(dislikeData);
                    newComment.CommentDisikes -= 1;
                    newComment.CommentLikes += 1;
                }

                _db.Comment.Update(newComment);
                _db.UserCommentLike.Add(new UserCommentLike { UserId = userId, CommentId = model.Id });
                 _db.SaveChangesAsync();

                likeComment = true;
            }
            return likeComment;
        }

        public void Save()
        {
            _db.SaveChangesAsync();
        }
    }
}
