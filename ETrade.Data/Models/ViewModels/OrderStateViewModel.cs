using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETrade.Data.Models.ViewModels
{
    public class OrderStateViewModel
    {
        public int OrderID { get; set;}
        public string OrderNumber { get; set;}
        public bool IsCompleted { get; set;}

    }
}
