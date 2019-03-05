using System;
using System.IO;
using System.Reflection;
using System.Text;
using Shared;
using Sql;

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
            File.WriteAllText("out.sql", CreateScript(tableName, idColumnName, parentIdColumnName));
        }

        private static string CreateScript(string tableName, string idColumnName, string parentIdColumnName, string templateTableName = "[Node]", string templateIdColumnName = "[Id]", string templateParentIdColumnName = "[Parent_Id]")
        {
            var sb = new StringBuilder();
            sb.AppendLine($"-- SPACE ANGLER v {Assembly.GetExecutingAssembly().GetName().Version} see https://github.com/mattiasnordqvist/Space-Angler");
            sb.AppendLine(Script.Read<Script>("Sql.alter-table.sql").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("Sql.filler.sql").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("Sql.triggers.delete.sql").ToString());
            sb.AppendLine("GO");                           
            sb.AppendLine(Script.Read<Script>("Sql.triggers.insert.sql").ToString());
            sb.AppendLine("GO");                           
            sb.AppendLine(Script.Read<Script>("Sql.triggers.update.sql").ToString());
            sb.AppendLine("GO");

            var concatenatedTemplateScript = sb.ToString();
            var scriptWithReplacedVariables = concatenatedTemplateScript
                .SqlReplace(templateTableName, tableName)
                .SqlReplace(templateIdColumnName, idColumnName)
                .SqlReplace(templateParentIdColumnName, parentIdColumnName)
                .Replace(SqlStuff.UnEscape(templateTableName)+"Trigger", SqlStuff.UnEscape(tableName) + "Trigger");

            return scriptWithReplacedVariables;
        }
    }
}
