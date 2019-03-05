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
            sb.AppendLine(Script.Read<Script>("alter-table").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("filler").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("triggers.delete").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("triggers.insert").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("triggers.update").ToString());
            sb.AppendLine("GO");

            var concatenatedTemplateScript = new Script(sb.ToString());
            var scriptWithReplacedVariables = concatenatedTemplateScript
                .New(SqlStuff.Escape(templateTableName), SqlStuff.Escape(tableName))
                .New(SqlStuff.Escape(templateIdColumnName), SqlStuff.Escape(idColumnName))
                .New(SqlStuff.Escape(templateParentIdColumnName), SqlStuff.Escape(parentIdColumnName))
                .New(SqlStuff.UnEscape(templateTableName) + "Trigger", SqlStuff.UnEscape(tableName) + "Trigger");

            return scriptWithReplacedVariables.ToString();
        }
    }
}
