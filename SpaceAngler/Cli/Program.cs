using System.IO;
using System.Reflection;
using System.Text;
using Sql;

namespace Cli
{
    public class Program
    {
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
            sb.AppendLine(Resources.ReadScript("alter-table"));
            sb.AppendLine("GO");
            sb.AppendLine(Resources.ReadScript("filler"));
            sb.AppendLine("GO");
            sb.AppendLine(Resources.ReadScript("triggers.delete"));
            sb.AppendLine("GO");
            sb.AppendLine(Resources.ReadScript("triggers.insert"));
            sb.AppendLine("GO");
            sb.AppendLine(Resources.ReadScript("triggers.update"));
            sb.AppendLine("GO");

            var concatenatedTemplateScript = sb.ToString();
            var scriptWithReplacedVariables = concatenatedTemplateScript
                .Replace(Escape(templateTableName), Escape(tableName))
                .Replace(Escape(templateIdColumnName), Escape(idColumnName))
                .Replace(Escape(templateParentIdColumnName), Escape(parentIdColumnName))
                .Replace(UnEscape(templateTableName)+"Trigger", UnEscape(tableName) + "Trigger");

            return scriptWithReplacedVariables;

        }

        private static string Escape(string tableName)
        {
            if (!tableName.StartsWith("["))
            {
                tableName = "[" + tableName;
            }

            if (!tableName.EndsWith("]"))
            {
                tableName = tableName + "]";
            }

            return tableName;
        }

        private static string UnEscape(string tableName)
        {
            if (tableName.StartsWith("["))
            {
                tableName = tableName.Substring(1);
            }

            if (tableName.EndsWith("]"))
            {
                tableName = tableName.Substring(0, tableName.Length - 1);
            }

            return tableName;
        }
    }
}
