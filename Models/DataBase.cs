using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication2.Models
{
    public class DataBase
    {
        public IConfiguration configuration { get; }
        string connectionString = configuration.GetConnectionStrnig.ConnectionString["DefaultConnection"];
        public List<Product> GetListProducts()
        {
            List<Product> products = new List<Product>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "select * from Product";
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                adapter.Fill(dtProducts);
                connection.Close();
                foreach(DataRow dr in dtProducts.Rows)
                {
                    products.Add(new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),                        
                        Info = dr["Info"].ToString(),                        
                        images = dr["Image"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"])
                    }); ;
                }
            }
            return products;
        }
    }
}
