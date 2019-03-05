using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Shared;

namespace Sql
{
    public class Script
    {
        private IEnumerable<string> batches;

        public Script(string sql)
        {
            batches = sql.Split("GO").Where(x => x != string.Empty);
        }

        public static Script Read<T>(string path)
        {
            return new Script(Resources<T>.Read($"Sql.{path}.sql"));
        }

        public Script Replace(string thisWith, string that)
        {
            batches = batches.Select(x => x.SqlReplace(thisWith, that)).ToList();
            return this;
        }

        public async Task ExecuteAsync(SqlConnection connection)
        {
            foreach(var batch in batches)
            {
                await connection.ExecuteAsync(batch);
            }
        }

        public override string ToString()
        {
            return string.Join($"{Environment.NewLine}GO{Environment.NewLine}", batches);
        }
    }
}
