using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sql
{
    public static class Resources
    {
        public static string ReadScript(string scriptName)
        {
            var assembly = Assembly.GetAssembly(typeof(Resources));
            var resourceName = $"Sql.{scriptName}.sql";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
