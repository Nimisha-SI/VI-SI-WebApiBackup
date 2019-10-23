using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApis.BOL
{
    public static class JSONHelper
    {
        public static string FromDataTable(DataTable dt)
        {
            string rowDelimiter = "";

            StringBuilder result = new StringBuilder(" [ ");
            foreach (DataRow row in dt.Rows)
            {
                result.Append(rowDelimiter);
                result.Append(FromDataRow(row));
                rowDelimiter = ",";
            }
            result.Append(" ] ");

            return result.ToString();
        }
        public static string FromDataRow(DataRow row)
        {
            DataColumnCollection cols = row.Table.Columns;
            string colDelimiter = "";

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < cols.Count; i++)
            { // use index rather than foreach, so we can use the index for both the row and cols collection
                result.Append(colDelimiter).Append("\"")
                      .Append(cols[i].ColumnName).Append("\":")
                      .Append(JSONValueFromDataRowObject(row[i], cols[i].DataType));

                colDelimiter = ",";
            }
            result.Append("}");
            return result.ToString();
        }
        private static Type[] numeric = new Type[] {typeof(byte), typeof(decimal), typeof(double),
                                     typeof(Int16), typeof(Int32), typeof(SByte), typeof(Single),
                                     typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        // I don't want to rebuild this value for every date cell in the table
        private static long EpochTicks = new DateTime(1970, 1, 1).Ticks;
        private static string JSONValueFromDataRowObject(object value, Type DataType)
        {

            // null
            if (value == DBNull.Value) return "null";


            // numeric
            if (Array.IndexOf(numeric, DataType) > -1)
                return value.ToString(); // TODO: eventually want to use a stricter format. Specifically: separate integral types from floating types and use the "R" (round-trip) format specifier

            // boolean
            if (DataType == typeof(bool))
                return ((bool)value) ? "true" : "false";

            // date -- see http://weblogs.asp.net/bleroy/archive/2008/01/18/dates-and-json.aspx
            if (DataType == typeof(DateTime))
                return "\"\\/Date(" + new TimeSpan(((DateTime)value).ToUniversalTime().Ticks - EpochTicks).TotalMilliseconds.ToString() + ")\\/\"";

            if (DataType == typeof(Byte[]))
            {
                //BinaryFormatter bf = new BinaryFormatter();
                //MemoryStream ms = new MemoryStream();
                //bf.Serialize(ms, value);

                Byte[] temp = (Byte[])(value);

                return System.Text.Encoding.UTF8.GetString(temp);
            }

            // TODO: add Timespan support
            // TODO: add Byte[] support

            //TODO: this would be _much_ faster with a state machine
            //TODO: way to select between double or single quote literal encoding
            //TODO: account for database strings that may have single \r or \n line breaks
            // string/char  
            return "\"" + value.ToString().Replace(@"\", @"\\").Replace(Environment.NewLine, @"\n").Replace("\"", @"\""") + "\"";
        }
    }
}
