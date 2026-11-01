using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    public  class WhereBuilder
    {
        private readonly List<string> _conditions = new();
        private readonly List<SqlParameter> _parameters = new();

        /// <summary>
        /// 「列 演算子 値」の条件を追加する。値が空なら何もしない。
        /// パラメーター名は "@列名 + suffix" になる。
        /// </summary>
        public void Add(string column, string op, object? value,
                        string suffix = "", params object[] ignoreValues)
        {
            if (IsEmpty(value)) return;
            if (ignoreValues.Any(v => Equals(v, value))) return;

            string paramName = "@" + column + suffix;
            _conditions.Add($"{column} {op} {paramName}");
            _parameters.Add(new SqlParameter(paramName, value));
        }

        public void AddEqual(string column, object? value)
            => Add(column, "=", value);

        public void AddLike(string column, string? keyword)
        {
            if (IsEmpty(keyword)) return;

            string paramName = "@" + column;
            _conditions.Add($"{column} LIKE {paramName}");
            _parameters.Add(new SqlParameter(paramName, "%" + keyword!.Trim() + "%"));
        }

        public void AddRange(string column, int? from, int? to,
                             int? ignoreFrom = null, int? ignoreTo = null)
        {
            if (from.HasValue && from != ignoreFrom)
                Add(column, ">=", from.Value, "From");

            if (to.HasValue && to != ignoreTo)
                Add(column, "<=", to.Value, "To");
        }

        public string ToWhereClause()
            => _conditions.Count == 0 ? "" : " WHERE " + string.Join(" AND ", _conditions);

        public SqlParameter[] Parameters => _parameters.ToArray();

        private static bool IsEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string s) return string.IsNullOrWhiteSpace(s);
            return false;
        }
    }
}
