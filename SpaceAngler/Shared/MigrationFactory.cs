using System.Reflection;
using System.Text;

namespace Core
{
    public class MigrationFactory
    {
        public static Script CreateScript(string tableName, string idColumnName, string parentIdColumnName, string templateTableName = "%_Node_%", string templateIdColumnName = "%_Id_%", string templateParentIdColumnName = "%_Parent_Id_%")
        {
            var sb = new StringBuilder();
            sb.AppendLine($"-- SPACE ANGLER v {Assembly.GetEntryAssembly().GetName().Version} see https://github.com/mattiasnordqvist/Space-Angler");
            sb.AppendLine(Script.Read<Script>("alter-table").ToString());
            sb.AppendLine("GO");
            sb.AppendLine(Script.Read<Script>("filler").ToString());
            sb.AppendLine("GO");
            sb.AppendLine($"-- SPACE ANGLER v {Assembly.GetEntryAssembly().GetName().Version} see https://github.com/mattiasnordqvist/Space-Angler");
            sb.AppendLine(Script.Read<Script>("triggers.delete").ToString());
            sb.AppendLine("GO");
            sb.AppendLine($"-- SPACE ANGLER v {Assembly.GetEntryAssembly().GetName().Version} see https://github.com/mattiasnordqvist/Space-Angler");
            sb.AppendLine(Script.Read<Script>("triggers.insert").ToString());
            sb.AppendLine("GO");
            sb.AppendLine($"-- SPACE ANGLER v {Assembly.GetEntryAssembly().GetName().Version} see https://github.com/mattiasnordqvist/Space-Angler");
            sb.AppendLine(Script.Read<Script>("triggers.update").ToString());
            sb.AppendLine("GO");

            var concatenatedTemplateScript = new Script(sb.ToString());
            var scriptWithReplacedVariables = concatenatedTemplateScript
                .New(templateTableName, SqlStuff.UnEscape(tableName))
                .New(templateIdColumnName, SqlStuff.UnEscape(idColumnName))
                .New(templateParentIdColumnName, SqlStuff.UnEscape(parentIdColumnName));

            return scriptWithReplacedVariables;
        }
    }
}
