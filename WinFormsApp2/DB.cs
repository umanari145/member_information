using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WinFormsApp2
{
    public class DB
    {
        private readonly string _connStr;

        public DB()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _connStr = config.GetConnectionString("Members")
                       ?? throw new InvalidOperationException("接続文字列 'Members' が appsettings.json に見つかりません。");

        }


        public DataTable Query(string sql, Dictionary<string, object?> parameters)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                }

                var table = new DataTable();
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
                return table;
            }
        }

        public int Execute(string sql, Dictionary<string, object?> parameters)
        {
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, conn))
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
                }

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }


}
