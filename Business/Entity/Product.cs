﻿

using System;
using System.Runtime.Serialization;
using System.Web;

namespace Business.Entity
{
    [DataContract]
    public class Product
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Category Category { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Warranty { get; set; }

        public string Image { get; set; }
        [DataMember]
        public int Inventory { get; set; }

        public bool Recycle { get; set; }

        [DataMember]
        public DateTime UpdateTime { get; set; }

        //extension

        public string CateName { get { return Category.Name; } }

        private string _strPrice;
        [DataMember]
        public string StrPrice
        {
            get
            {
                return string.IsNullOrEmpty(_strPrice) ? string.Format("{0:0,0 VND}", Price) : _strPrice;
            }
            set { _strPrice = value; }
        }

        [DataMember]
        public string ImageHost
        {
            get { return "http://localhost:1805/" + Image; }
            set { Image = value; }
        }


    }
}