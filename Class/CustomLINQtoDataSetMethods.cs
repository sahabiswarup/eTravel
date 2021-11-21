using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace e_Travel.Class
{
    public static class CustomLINQtoDataSetMethods
    {
        public static DataTable CopyToDataTableExt<T>(this IEnumerable<T> source)
        {
            return new ObjectShredder<T>().Shred(source, null, null);
        }

        public static DataTable CopyToDataTableExt<T>(this IEnumerable<T> source,
                                                    DataTable table, LoadOption? options)
        {
            return new ObjectShredder<T>().Shred(source, table, options);
        }

    }
}
