using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prueba_tecnica_agroexport.Models
{
    public class ReceptionModel
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public int WoodTypeId { get; set; }

        public decimal AverageWeight { get; set; }
        public int PieceCount { get; set; }
        public decimal TotalWeight { get; set; }
        public DateTime ReceptionDate { get; set; }
    }
}
