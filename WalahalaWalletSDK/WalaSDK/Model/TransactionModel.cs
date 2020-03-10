using System;

namespace WalaSDK.Model
{
    public class TransactionModel
    {
        public int TxId { get; set; }
        public string TxHash { get; set; }
        public string ParentHash { get; set; }
        public string SenderPubKey { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Signature { get; set; }
        public int? VerificationCount { get; set; }
    }
}
