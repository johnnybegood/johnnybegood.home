using System.Collections.Generic;
using System.Linq;

namespace JOHHNYbeGOOD.Home.Resources.Connectors
{
    public static class ConnectionStringHelper
    {
        public const char DefaultSeparator = ';';
        private const char KeyValueSeparator = '=';

        /// <summary>
        /// Read connectionstring
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static IDictionary<string, string> Read(string connectionString, char separator = DefaultSeparator)
        {
           return connectionString.Split(separator)
                .Select(x => x.Split(KeyValueSeparator))
                .ToDictionary(x => x[0], x => x[1]);
        }
    }
}
