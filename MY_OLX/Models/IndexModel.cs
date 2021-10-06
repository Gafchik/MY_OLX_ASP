using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MY_OLX.Models
{
    public class IndexModel
    {
        public List<Product> products = ProductRep.GetProducts();
    }
}
