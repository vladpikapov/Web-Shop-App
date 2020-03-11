﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VladNesterTest.Models;

namespace VladNesterTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IConfiguration Configuration { get; set; }
        public ProductController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM PRODUCTS", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = reader.GetString(2),
                            Country = reader.GetString(3)
                        });

                    }
                }
                reader.Close();
            }
            return products;
        }

        [HttpPost]
        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("AddProduct", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Type", product.Type);
                command.Parameters.AddWithValue("@Country", product.Country);
                command.ExecuteNonQuery();
            }
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(int id)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DeleteProduct", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

    }
}