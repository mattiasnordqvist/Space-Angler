using System.Data.SqlClient;
using System.IO;
using Dapper;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FillerScriptTests
    {
        private string DBName = "SpaceAnglerTestDb";

        [Test]
        public void OneSingleTest()
        {
            string connectionString = $@"
                Server=.; 
                Integrated Security=true;
                Initial Catalog=master;
            ";
            var connection = new SqlConnection(connectionString);
            connection.Open();
            connection.Execute($@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{DBName}')
                BEGIN
                    CREATE DATABASE [{DBName}]
                END");
            connection.ChangeDatabase(DBName);
            connection.Close();
            connection.Dispose();
        }
    }
}
