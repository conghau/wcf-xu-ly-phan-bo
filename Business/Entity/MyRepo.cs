﻿

using System;
using System.Runtime.Serialization;

namespace Business.Entity
{
    [DataContract]
    public class MyRepo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ReceiptDeliveryId { get; set; }
        [DataMember]
        public bool IsReceipt { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        
        public Product Product { get; set; }
        [DataMember]
        public int StockUnit { get; set; }
        [DataMember]
        public byte Repository { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Inventory { get; set; }
        [DataMember]
        public int ReceiptUnit { get; set; }
    }
}