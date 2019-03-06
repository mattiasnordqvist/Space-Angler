using System.IO;
using Core;

namespace Cli
{
    public class Program
    {
        static Resources<Script> scripts = new Resources<Script>();
        static void Main(string[] args)
        {
            var p = new Params(args);
            var tableName = p.Get("table name", "-t");
            var idColumnName = p.Get("id column name", "-i");
            var parentIdColumnName = p.Get("parent id column name", "-p");
            File.WriteAllText("out.sql", MigrationFactory.CreateScript(tableName, idColumnName, parentIdColumnName).ToString());
        }

      
    }
}
