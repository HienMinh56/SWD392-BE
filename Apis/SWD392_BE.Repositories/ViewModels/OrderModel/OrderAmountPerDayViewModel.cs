﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.ViewModels.OrderModel
{
    public class OrderAmountPerDayViewModel
    {
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
