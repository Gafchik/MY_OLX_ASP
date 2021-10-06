using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MY_OLX.Models
{
    public class Product
    {
        public int id { get; set; }
        public string category { get; set; }
        public string salesman { get; set; }
        public string product { get; set; }
        public string model { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public string img { get; set; }
        
    }
}
