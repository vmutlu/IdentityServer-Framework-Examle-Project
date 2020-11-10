using IdentityServerProject.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServerProject.API1.Controllers
{
    [Route("api/[controller]/[action]")] //controller isminden sonra method ismine göre endpointlere ulaşmak istiyorum
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        //api/products/getproducts
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product { Id = 1, Name = "Deneme Ürün -1", Price = 100, Stock = 50 },
                new Product { Id = 2, Name = "Deneme Ürün -2", Price = 200, Stock = 55 },
                new Product { Id = 3, Name = "Deneme Ürün -3", Price = 300, Stock = 60 },
                new Product { Id = 4, Name = "Deneme Ürün -4", Price = 400, Stock = 65 },
                new Product { Id = 5, Name = "Deneme Ürün -5", Price = 500, Stock = 70 }
            };

            return Ok(productList);
        }

        //startup.cs sınıfında yetkilendirmeler mevcuttur bakınız.

        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"Id'si {id} olan ürün güncellenmiştir.");
        }

        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult Create(Product product)
        {
            return Ok(product);
        }

    }
}
