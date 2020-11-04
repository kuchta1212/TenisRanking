using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenisRanking.Models
{
    public class MatchResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public List<MatchSet> Sets { get; set; }

        public string Winner { get; set; }

        public MatchResultType Type { get; set; }

        public string CreatedBy { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var set in this.Sets)
            {
                sb.Append(set);
                sb.Append(", ");
            }

            return sb.ToString();
        }
    }
}
