using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MY_OLX.Models;
using System.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MY_OLX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _appEnvironment;
        private IndexModel indexModel;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment appEnvironment)
        {
            indexModel = new IndexModel();
            _logger = logger;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index() => View(indexModel);
        public IActionResult NewProduct() => View();
        public IActionResult Del(int id)
        {
            //находим нашу картинку на сервере
            string path = ProductRep.GetProducts().Find(i => i.id == id).img;
            string fullPath = _appEnvironment.WebRootPath + path.Trim('~');
            if (!System.IO.File.Exists(fullPath))
            {
                Debug.WriteLine("not fond");
                return Redirect("/Home/Index");
            }
            // удаляем картинку
            try {  System.IO.File.Delete(fullPath); }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Redirect("/Home/Index");
            }
            // удаляем продукт
            ProductRep.Delete(id);
            //обновляем модель индекса с продуктами
            indexModel.UpdateProducts();
            // возвращаемся на главную
            return Redirect("/Home/Index");
        }
        #region edit product
        public IActionResult EditView(int id)=> View(indexModel.products.Find(i => i.id == id));

        public IActionResult EditProduct(Product product)
        {
            //валидация измененного товара
            if (CheckProduct(product))
                ProductRep.UpdateProducts(product);// обновление товара
            //обновляем модель индекса с продуктами
            indexModel.UpdateProducts();
            return Redirect("/Home/Index");
        }
        #endregion

        #region add product
       
        public async Task<IActionResult> Add(Product product, IFormFile uploadedFile)
        {

            if (uploadedFile == null)
                return Redirect("/Home/NewProduct");
           if( !IsImg(uploadedFile.FileName))
                return Redirect("/Home/NewProduct");
            if (!CheckProduct(product))
                return Redirect("/Home/NewProduct");
            #region file         
            // путь к папке Image
            string path = "/Image/" + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
          // ~ нам говорит о том что мы попадаем в папку wwwroot
            product.img = "~" + path;
            #endregion
            //  добавляем продукт         
            ProductRep.AddProducts(product);
            //обновляем модель индекса с продуктами
            indexModel.UpdateProducts();
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
                                    rez = true;
            return rez;
        }
        private bool IsImg(string path)
        {
            bool rez;
            if (path.EndsWith(".png") || path.EndsWith(".jpg") || path.EndsWith(".JPG") || path.EndsWith(".PNG"))
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
