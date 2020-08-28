using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Data
{
    public class AccountSubscriber
    {
        public int Id { get; set; }
        public string AccountUserId { get; set; }
        public string SubscribeUserId { get; set; }
        public string SubscribeUserEmail { get; set; }
    }
}
