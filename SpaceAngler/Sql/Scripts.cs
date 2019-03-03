namespace Sql
{
    public class Scripts : Shared.Resources<Scripts>
    {
        public override string Read(string scriptName)
        {
            return base.Read($"Sql.{scriptName}.sql");
        }
    }
}
