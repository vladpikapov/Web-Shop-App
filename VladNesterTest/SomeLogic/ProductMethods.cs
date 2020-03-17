﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace VladNesterTest.SomeLogic
{
    public class ProductMethods
    {
        public static void AddOrDropeOneProduct(string procedure, int id, string connecionString)
        {
            using (SqlConnection connection = new SqlConnection(connecionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
