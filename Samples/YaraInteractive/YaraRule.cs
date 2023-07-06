using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class YaraRule
    {
        public double Score { get;set; }

        public string RuleId { get; set; }

        public string RuleType { get; set;  }
    }
}
