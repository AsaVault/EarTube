using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Data
{
    public class AccountUnsubscriber
    {
        public int Id { get; set; }
        public string AccountUserId { get; set; }
        public string UnSubscribeUserId { get; set; }
        public string UnSubscribeUserEmail { get; set; }
    }
}
