using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication2.Models
{
    public class DataBase : DbContext
    {
        private readonly IConfiguration _configuration;
        string connectionString;
        public DataBase(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("connectionString");
        }
    
        public List<Product> GetListProducts()
        {
            // var connectionString = _configuration.GetConnectionString("connectionString");
            // string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Amazon;Integrated Security=True";
            // Console.WriteLine(connectionString + " ! ");
            List<Product> products = new List<Product>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
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
        public void AddProduct(Product product)
        {
            Console.WriteLine(product.Id + " " + product.Name + " " + product.Info);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Product (Id , Name , Info , Price , Image) VALUES (@Id , @Name , @Info , @Price , @Image)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue ("@Name", product.Name);
                    command.Parameters.AddWithValue("@Info" , product.Info);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Image", product.images);
                    connection.Open();
                    int ok = command.ExecuteNonQuery();
                    if (ok == 0)
                        Console.WriteLine("Couldn't upload the product");
                    connection.Close();
                }
            }
        }
        /// Delete holi bud nest
        public void DeleteProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection (connectionString))
            {
                string query;
                query = "DELETE FROM Product WHERE (Id = " + product.Id + ")";
                using (SqlCommand command = new SqlCommand(query , connection))
                {
                    connection.Open();
                    int ok = command.ExecuteNonQuery();
                    if (ok == 0)
                        Console.WriteLine("Couldn't delete the product");
                    connection.Close();
                }
            }
        }
        public Product SearchProduct(int Id)
        {
            Product product = new Product();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // string query = "SELECT * FROM Product WHERE (Id == " + Id.ToString() + ")" ;
                SqlCommand query = connection.CreateCommand();
                // query.CommandType = CommandType.Text;
                query.CommandText = "SELECT * FROM Product WHERE (Id = " + Id.ToString() + ")";
                SqlDataAdapter adapter = new SqlDataAdapter(query);
                DataTable searchProduct = new DataTable();
                connection.Open();
                adapter.Fill(searchProduct);
                connection.Close();
                foreach (DataRow dr in searchProduct.Rows)
                {
                    product = (new Product
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = dr["Name"].ToString(),
                        Info = dr["Info"].ToString(),
                        images = dr["Image"].ToString(),
                        Price = Convert.ToDecimal(dr["Price"])
                    }); ;
                }
            }
            return product;
        }
        public int NumberOfProducts()
        {
            int NumberOfProducts;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(Id) FROM Product";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    NumberOfProducts = (int)command.ExecuteScalar(); // ExecuteScalar returns the value from the query
                    connection.Close();
                }
            }
            return NumberOfProducts;
        }

        public void Update(int id , Product product)
        {
            var newProduct = new Product();
            newProduct = SearchProduct(id);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Product SET Name = @Name , Info = @Info , Price = @Price , Image = @Image WHERE Id = @Id ";
                using (SqlCommand command = new SqlCommand(query , connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Info", product.Info);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    // command.Parameters.AddWithValue("@Image", (object)product.images ?? DBNull.Value);
                    if (product.images == null)
                        command.Parameters.AddWithValue("@Image", newProduct.images);
                    else
                        command.Parameters.AddWithValue("@Image", product.images);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    int ok = command.ExecuteNonQuery();
                    if (ok == 0)
                        Console.WriteLine("Couldn't not update the product data !");
                    connection.Close();
                }
            }
        }
    }
}
