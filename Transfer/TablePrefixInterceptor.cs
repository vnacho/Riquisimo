using Ferpuser.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Ferpuser.Transfer
{
    internal class TablePrefixInterceptor : DbCommandInterceptor
    {
        private string databasePrefix;

        public TablePrefixInterceptor(string databasePrefix)
        {
            this.databasePrefix = databasePrefix;
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            // Manipulate the command text, etc. here...
            command.CommandText = command.CommandText.Replace(FerpuserContext._prefix, databasePrefix);
            return result;
        }
    }
}