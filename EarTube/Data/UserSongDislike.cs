using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Data
{
    public class UserSongDislike
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string UserId { get; set; }
    }
}
