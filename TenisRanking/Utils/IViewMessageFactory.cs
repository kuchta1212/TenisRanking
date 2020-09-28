using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenisRanking.Models;

namespace TenisRanking.Utils
{
    public interface IViewMessageFactory
    {
        ViewMessage Create(MessageStatus status, string message);
    }
}
