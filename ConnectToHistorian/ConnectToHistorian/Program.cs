using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace ConnectToHistorian
{
    class Program
    {
        static void Main(string[] args)
        {
            string query =
                @"--Quering the WideHistory table in INSQL liked server
                DECLARE @tagNames NVARCHAR(MAX) = '',
	                @openQuery NVARCHAR(MAX),
	                @tsql NVARCHAR(MAX),
	                @linkedServer NVARCHAR(MAX) = 'INSQL';

                            SELECT @tagNames += QUOTENAME(TagName, '[') + ','
                FROM TagRef
                WHERE TagName LIKE '%Mixer%';

                            SET @tagNames = LEFT(@tagNames, LEN(@tagNames) - 1)
                SET @tsql = 'SELECT DateTime, ' + @tagNames + ' 
                             FROM WideHistory

                             WHERE DateTime > DATEADD(HH, -4, GETDATE())';
                SET @openQuery = 'SELECT * 
                                  FROM OPENQUERY(' + @linkedServer + ','' ' + @tsql + ' '' ) 
                                  ORDER BY DateTime ASC';

                EXECUTE(@openQuery);";

            string filePath =  string.Format("{0}{1}{2}{3}", "C:\\Users\\wyle.cordero\\Desktop\\", "Test", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ".csv");            
            DataTable dt = QueryToDataTable(query);
            DataTableToCsv(dt, filePath);
        }

        static SqlConnection DbConnection()
        {
            StringBuilder connString = new StringBuilder();
            connString.Append("Data Source=WIN-UIH9QHSEMDC;");
            connString.Append("Initial Catalog=Runtime;");
            connString.Append("Integrated Security=false;");
            connString.Append("User Id=sa;");
            connString.Append("Password=Password_1");           
            var conn = new SqlConnection(connString.ToString());
            return conn;
        }

        public static DataTable QueryToDataTable(string query)
        {
            var conn = DbConnection();
            var dt = new DataTable();
            var cmd = new SqlCommand(query, conn);
            var da = new SqlDataAdapter(cmd);

            try
            {
                da.Fill(dt);
                conn.Close();
                return dt;
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex);
                conn.Close();
                return null;
            }
        }

        public static void DataTableToCsv(DataTable dt, string filePath)
        {
            if (dt != null)
            {
                var lines = new List<string>();
                string[] columnNames = dt.Columns.Cast<DataColumn>().
                                            Select(column => column.ColumnName).
                                            ToArray();               
                string headers = string.Join(",", columnNames);
                lines.Add(headers);

                EnumerableRowCollection<string> valueLines = dt.AsEnumerable().
                                                                 Select(row => string.Join(",", row.ItemArray));
                lines.AddRange(valueLines);                
                File.WriteAllLines(filePath, lines);
            }
        }
    }
}
