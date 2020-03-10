using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalaSDK.Model
{
    public class UnitModel
    {
        public int UnitID { get; set; }
        public string TxHash { get; set; }
        public string ParentHash { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public Nullable<int> VerificationCount { get; set; }
        public string Status { get; set; }
        public string VerificationTime { get; set; }
    }
}
