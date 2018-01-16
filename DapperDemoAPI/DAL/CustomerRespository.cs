using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DapperDemoAPI.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace DapperDemoAPI.DAL
{
    //https://www.jeremymorgan.com/blog/programming/how-to-dapper-web-api/
    public class CustomerRespository : ICustomerRespository
    {
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public bool DeleteCustomer(int customerId)
        {
            int rowsAffected = this.db.Execute(@"DELETE FROM [jeremy].[Customer] WHERE CustomerID = @CustomerID", new { CustomerID = customerId });

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public List<Customer> GetCustomers(int amount, string sort)
        {
            return this.db.Query<Customer>("SELECT TOP " + amount + " [CustomerID],[CustomerFirstName],[CustomerLastName],[IsActive] FROM [Customer] ORDER BY CustomerID " + sort).ToList();
        }

        public Customer GetSingleCustomer(int customerId)
        {
            return db.Query<Customer>("SELECT[CustomerID],[CustomerFirstName],[CustomerLastName],[IsActive] FROM [Customer] WHERE CustomerID =@CustomerID", new { CustomerID = customerId }).SingleOrDefault();
        }

        public bool InsertCustomer(Customer ourCustomer)
        {
            int rowsAffected = this.db.Execute(@"INSERT Customer([CustomerFirstName],[CustomerLastName],[IsActive]) values (@CustomerFirstName, @CustomerLastName, @IsActive)", new { CustomerFirstName = ourCustomer.CustomerFirstName, CustomerLastName = ourCustomer.CustomerLastName, IsActive = true });

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateCustomer(Customer ourCustomer)
        {
            int rowsAffected = this.db.Execute("UPDATE [Customer] SET [CustomerFirstName] = @CustomerFirstName ,[CustomerLastName] = @CustomerLastName, [IsActive] = @IsActive WHERE CustomerID = " + ourCustomer.CustomerID, ourCustomer);

            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}