using EarTube.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Like { get; set; }
        public int SongLike { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string SongUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<Comment> Comment { get; set; }


    }
}
