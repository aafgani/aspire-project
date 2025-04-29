using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Test.IntegrationTest.Infrastructure.Helpers
{
    public sealed class DbConnectionFactory(string connectionString, string database)
    {
        public DbConnection MasterDbConnection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}
