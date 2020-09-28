using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TenisRanking.Models
{
    public class Player : IdentityUser
    {
        public DateTime LastPlayedMatch { get; set; }

        public int Rank { get; set; }

        public string PlayerName { get; set; }
    }
}
