﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using VladNesterTest.Models;
using VladNesterTest.SomeLogic;

namespace VladNesterTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        string ConnectionString { get; set; }
        public ProductController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        [HttpGet]
        public List<Product> GetProducts()
        {
            return ProductMethods.GetProducts(ConnectionString);
        }

        [HttpPost]
        public void AddProduct(Product product)
        {
            Product prodFromList = ProductMethods.GetProductById(product, ConnectionString);
            if (prodFromList == null)
            {
                if (product.Name.Length <= 200)
                    ProductMethods.AddProduct(product, ConnectionString);
            }
            else
            {
                ProductMethods.UpdateProduct(ConnectionString, $"update PRODUCTS set ProductCount += {product.Count} where Id = {prodFromList.Id};");
            }
        }
        [HttpPut("add/{id}")]
        public void AddOneProduct(int id)
        {
            ProductMethods.AddOrDropeOneProduct($"update PRODUCTS set ProductCount += 1 where Id = {id}", ConnectionString);
        }

        [HttpPut("drop/{id}")]
        public void DropOneProduct(int id)
        {
            ProductMethods.AddOrDropeOneProduct($"update PRODUCTS set ProductCount -= 1 where Id = {id}", ConnectionString);
        }

    }
}