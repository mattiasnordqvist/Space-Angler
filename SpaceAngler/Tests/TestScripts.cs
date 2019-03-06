using Core;

namespace Tests
{
    public class TestScripts 
    {
        public static Script Read(string scriptName)
        {
            return new Script(Resources<TestScripts>.Read($"Tests.{scriptName}.sql"));
        }
    }
}
