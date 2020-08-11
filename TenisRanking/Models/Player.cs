﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TenisRanking.Models
{
    public class Player : IdentityUser
    {
        public string Name { get; set; }

        public DateTime LastPlayedMatch { get; set; }

        public string Rank { get; set; }
    }
}
