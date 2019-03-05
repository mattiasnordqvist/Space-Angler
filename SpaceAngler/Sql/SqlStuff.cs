﻿namespace Sql
{
    public static class SqlStuff
    {
        public static string SqlReplace(this string t, string template, string value)
        {
            return t.Replace(Escape(template), Escape(value));
        }
        public static string Escape(this string tableName)
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

        public static string UnEscape(this string tableName)
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
