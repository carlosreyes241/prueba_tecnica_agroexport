using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prueba_tecnica_agroexport.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemId { get; set; }
        public int CustomerId { get; set; }
    }
}
