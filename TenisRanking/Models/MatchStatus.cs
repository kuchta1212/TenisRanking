﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public enum MatchStatus
    {
        Challanged,

        Accepted,

        WaitingForConfirmation,

        Played,

        Refused
    }
}
