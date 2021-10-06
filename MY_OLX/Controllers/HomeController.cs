using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MY_OLX.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MY_OLX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View(new IndexModel());
        public IActionResult NewProduct() => View();
        public IActionResult Del(int id)
        {
            ProductRep.Delete(id);
            // возвращаемся на главную
            return Redirect("/Home/Index");
        }
        public IActionResult EditView(int id)=> View(ProductRep.GetProducts().Find(i => i.id == id));

        public IActionResult EditProduct(Product product)
        {
            if (CheckProduct(product))
                ProductRep.UpdateProducts(product);
            return Redirect("/Home/Index");
        }
        #region add product
        public IActionResult Add(Product product)
        {
            // если форма не пустая добавляем продукт
            if (CheckProduct(product))
                ProductRep.AddProducts(product);
            // возвращаемся на главную
            return Redirect("/Home/NewProduct");
        }
        private bool CheckProduct(Product product)
        {
            bool rez = false;
            if (product.model != null && product.model != string.Empty)
                if (product.product != null && product.product != string.Empty)
                    if (product.salesman != null && product.salesman != string.Empty)
                        if (product.category != null && product.category != string.Empty)
                            if (product.description != null && product.description != string.Empty)
                                if (product.price != null)
                                    if (product.img != null && product.img != string.Empty && IsImg(product.img))
                                        rez = true;
            return rez;
        }
        private bool IsImg(string path)
        {
            bool rez;
            if (path.EndsWith(".png") || path.EndsWith(".jpg"))
                rez = true;
            else
                rez = false;
            return rez;
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
