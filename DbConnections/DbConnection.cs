using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prueba_tecnica_agroexport.DbConnections
{
    public class DbConnection
    {
        public async Task<SqlConnection> Connection()
        {
            string connectionString = "Data Source=DESKTOP-A30C9EP\\SQLEXPRESS;Initial Catalog=MublesAgroexport;Trusted_Connection=true; TrustServerCertificate=true;";
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
