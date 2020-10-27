using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Email
{
    public class EmailOptions
    {
        public bool Enabled { get; set; }

        public string SendGridKeyApi { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }
    }
}
