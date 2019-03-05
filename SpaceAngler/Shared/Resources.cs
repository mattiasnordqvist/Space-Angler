using System;
using System.IO;
using System.Reflection;

namespace Shared
{
    public class Resources<T>
    {
        public static string Read(string resourceName)
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
