﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class MatchViewModel
    {
        public string MatchId { get; set; }

        public string Challanger { get; set; }

        public string Deffender { get; set; }

        public MatchResultType Type { get; set; }

        public int FirstSetDefender { get; set; }

        public int FirstSetTieBreakDeffender { get; set; }

        public int SecondSetDefender { get; set; }

        public int SecondSetTieBreakDeffender { get; set; }

        public int FirstSetChellanger { get; set; }

        public int FirstSetTieBreakChallanger { get; set; }

        public int SecondSetChellanger { get; set; }

        public int SecondSetTieBreakChallanger { get; set; }
    }
}
