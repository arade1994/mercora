using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mercora.Infrastructure
{
    public static class SqlTvpFactory
    {
        public static DataTable BuildOrderLinesTvp(IEnumerable<(int VariantId, int Quantity)> lines)
        {
            var table = new DataTable();
            table.Columns.Add("VariantId", typeof(int));
            table.Columns.Add("Quantity", typeof(int));

            foreach (var (variantId, qty) in lines)
                table.Rows.Add(variantId, qty);

            return table;
        }
    }
}
