using EarTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.ViewModel
{
    public class SongCommentViewModel
    {
        public Song SongVM { get; set; }
        public Comment CommentVM { get; set; }
        public List<Comment> CommentsVM { get; set; }
    }
}
