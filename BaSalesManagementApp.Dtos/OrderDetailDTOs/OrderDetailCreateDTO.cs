﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaSalesManagementApp.Dtos.OrderDetailDTOs
{
    public class OrderDetailCreateDTO
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
