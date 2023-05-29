using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaModel.Orders
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
