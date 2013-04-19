using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexTrade
{
    class UnacceptableRiskException : System.ApplicationException
    {
        public RiskFilter sender { get; set; }
        public List<String> riskFilterMessages { get; set; }

        public UnacceptableRiskException(RiskFilter s, List<String> m)
        {
            sender = s;
            riskFilterMessages = m;
        }
    }
}
