﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenisRanking.Email
{
    public interface IEmailController
    {
        Task<bool> SendChallangeEmail();

        Task<bool> SendChallangeAcceptedEmail();

        Task<bool> SendChallangeRefusedEmail();

        Task<bool> SendRegisterConfirmationEmail(string to, string name, string subject, string body);
    }
}
