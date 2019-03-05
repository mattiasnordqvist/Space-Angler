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
        private List<string> list;

        public Script(string sql)
        {
            batches = sql.Split("GO").Where(x => x != string.Empty);
        }

        public Script(List<string> list)
        {
            batches = list;
        }

        public static Script Read<T>(string path)
        {
            return new Script(Resources<T>.Read(path));
        }

        public Script New(string thisWith, string that)
        {
            
            return new Script(batches.Select(x => x.Replace(thisWith, that)).ToList());
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
