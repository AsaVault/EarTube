using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Data
{
    public class UserCommentLike
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
    }
}
