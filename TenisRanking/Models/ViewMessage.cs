using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TenisRanking.Models
{
    public class ViewMessage
    {
        public MessageStatus Status { get; set; }

        public string Message { get; set; }
    }

    public enum MessageStatus
    {
        NONE, ERROR, WARNING, SUCCESS
    }
}
