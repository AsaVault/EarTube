using EarTube.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CommentLikes { get; set; }

    }
}
