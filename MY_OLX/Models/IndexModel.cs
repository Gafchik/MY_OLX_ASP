using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MY_OLX.Models
{
    public class IndexModel
    {
        public List<Product> products;
        public List<FileModel> files;
        public IndexModel()
        {
            products = ProductRep.GetProducts();
            files = new List<FileModel>();
        }
        public void UpdateProducts()=> products = ProductRep.GetProducts();
    }
}
