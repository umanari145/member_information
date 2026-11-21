using System.Data;
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
                       ?? throw new InvalidOperationException(string.Format(modCommon.Messages.DBError.CannotOpenFile, "Members"));
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

        /// <summary>
        /// values の Key をカラム名として INSERT を実行します。
        /// idColumn(自動採番列など)は除外します。
        /// </summary>
        public int Insert(string tableName, Dictionary<string, object?> values, string idColumn = "Id")
        {
            var columns = new List<string>();
            var placeholders = new List<string>();
            var parameters = new Dictionary<string, object?>();

            foreach (var item in values)
            {
                if (string.Equals(item.Key, idColumn, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                columns.Add(QuoteIdentifier(item.Key));
                placeholders.Add("@" + item.Key);
                parameters["@" + item.Key] = item.Value;
            }

            if (columns.Count == 0)
            {
                throw new ArgumentException(modCommon.Messages.DBError.NotExistInsertColumn, nameof(values));
            }

            string sql = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2})",
                QuoteIdentifier(tableName),
                string.Join(", ", columns),
                string.Join(", ", placeholders));

            return Execute(sql, parameters);
        }

        /// <summary>
        /// values の Key をカラム名として UPDATE を実行します。
        /// idColumn は SET に含めず、WHERE 条件にのみ使用します。
        /// </summary>
        public int Update(string tableName, Dictionary<string, object?> values,
                          string idColumn, object idValue)
        {
            var sets = new List<string>();
            var parameters = new Dictionary<string, object?>();

            foreach (var item in values)
            {
                if (string.Equals(item.Key, idColumn, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                sets.Add(string.Format("{0} = @{1}", QuoteIdentifier(item.Key), item.Key));
                parameters["@" + item.Key] = item.Value;
            }

            if (sets.Count == 0)
            {
                throw new ArgumentException(modCommon.Messages.DBError.NotExistUpdateColumn, nameof(values));
            }

            // WHERE 句用のパラメータを追加
            parameters["@" + idColumn] = idValue;

            string sql = string.Format(
                "UPDATE {0} SET {1} WHERE {2} = @{3}",
                QuoteIdentifier(tableName),
                string.Join(", ", sets),
                QuoteIdentifier(idColumn),
                idColumn);

            return Execute(sql, parameters);
        }

        // DB クラス
        public int DeleteByIds(string tableName, string idColumn, IReadOnlyList<int> ids)
        {
            if (ids.Count == 0)
            {
                return 0;
            }

            var parameters = new Dictionary<string, object?>();
            var paramNames = new List<string>();

            for (int i = 0; i < ids.Count; i++)
            {
                string paramName = "@id" + i;
                paramNames.Add(paramName);
                parameters[paramName] = ids[i];
            }

            string sql = string.Format(
                "DELETE FROM {0} WHERE {1} IN ({2})",
                QuoteIdentifier(tableName),
                QuoteIdentifier(idColumn),
                string.Join(", ", paramNames));

            return Execute(sql, parameters);
        }

        /// <summary>
        /// テーブル名・列名を [ ] で囲み、SQLインジェクションを防ぎます。
        /// </summary>
        private static string QuoteIdentifier(string name)
        {
            return "[" + name.Replace("]", "]]") + "]";
        }
    }
}
