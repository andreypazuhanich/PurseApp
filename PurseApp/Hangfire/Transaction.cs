using System;

namespace PurseApp
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        
        public Guid AccountSourceId { get; set; }
        
        public Guid AccountDestinationId { get; set; }
        
        public decimal Amount { get; set; }
        
        public TransactionStatus Status { get; set; }
    }
}