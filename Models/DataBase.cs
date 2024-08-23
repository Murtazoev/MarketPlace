using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;

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
        public int NextProductID()
        {
            int nextId = 0;
            using (SqlConnection connection = new SqlConnection (connectionString))
            {
                string query = "SELECT TOP 1 id FROM Product ORDER BY id DESC";
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    connection.Open();
                    nextId = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }
            nextId++;
            return nextId;
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
        public void AddClient(Client client)
        {
            Console.WriteLine(client.Contact_Number);
            Console.WriteLine("this is contact number");
            using (SqlConnection connection = new SqlConnection (connectionString))
            {
                string query = "INSERT INTO Clients (Id , Name , Surname , Email , Contact , Password , Avatar) VALUES (@Id , @Name , @Surname , @Email , @Contact , @Password , @Avatar)";
                using (SqlCommand command = new SqlCommand(query , connection))
                {
                    command.Parameters.AddWithValue("@Id", client.Id);
                    command.Parameters.AddWithValue("@Name", client.Name);
                    command.Parameters.AddWithValue("@Surname", client.Surname);
                    command.Parameters.AddWithValue("@Email", client.Email);
                    command.Parameters.AddWithValue("@Contact", client.Contact_Number);
                    command.Parameters.AddWithValue("@Password", client.Password);
                    command.Parameters.AddWithValue("@Avatar", client.AvatarLocation);
                    connection.Open();
                    int ok = command.ExecuteNonQuery();
                    if (ok == 0)
                        Console.WriteLine("Couldn't upload the Client to the Client Table !!!");
                    connection.Close();
                }
            }
        }

        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client> ();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clients";
                SqlCommand command = new SqlCommand(query , connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataType = new DataTable();
                connection.Open();
                adapter.Fill(dataType);
                connection.Close();
                foreach(DataRow i in dataType.Rows)
                {
                    clients.Add(new Client
                    {
                        Id = Convert.ToInt32(i["Id"]),
                        Name = i["Name"].ToString(),
                        Surname = i["Surname"].ToString(),
                        AvatarLocation = i["Avatar"].ToString(),
                        Contact_Number = i["Contact"].ToString(),
                        Email = i["Email"].ToString(),
                        Password = i["Password"].ToString()
                    }); ; ;
                }
            }
            return clients;
        }
        public int NumberOfClients()
        {
            int NumberOfClients;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(Id) FROM Clients";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    NumberOfClients = (int)command.ExecuteScalar(); // ExecuteScalar returns the value from the query
                    connection.Close();
                }
            }
            return NumberOfClients;
        }
        public int NextClientID()
        {
            int nextId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 id FROM Clients ORDER BY id DESC";
                using (SqlCommand command = new SqlCommand(query , connection))
                {
                    connection.Open();
                    nextId = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }
            nextId++;
            return nextId;
        }
        public Client SearchClient(string Name)
        {
            Client newClient = new Client();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Clients WHERE Name = '" + Name + "';";
                using (SqlCommand command = new SqlCommand(query , connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataType = new DataTable();
                    connection.Open();
                    adapter.Fill(dataType);
                    connection.Close();
                    if (adapter == null)
                        return newClient;
                    newClient = new Client {
                        Id = Convert.ToInt32(dataType.Rows[0]["ID"]),
                        Name = dataType.Rows[0]["Name"].ToString() ,
                        Surname = dataType.Rows[0]["Surname"].ToString() ,
                        Email = dataType.Rows[0]["Email"].ToString(),
                        AvatarLocation = dataType.Rows[0]["Avatar"].ToString(),
                        Contact_Number = dataType.Rows[0]["Contact"].ToString(),
                        Password = dataType.Rows[0]["Password"].ToString()
                    } ;
                }
            }
            return newClient;
        }
    }
}
