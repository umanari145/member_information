
namespace WinFormsApp2.Request
{
    public class RequestSearch
    {
        /// <summary>
        /// 「列 演算子 値」の条件を追加する。値が空なら何もしない。
        /// パラメーター名は "@列名 + suffix" になる。
        /// </summary>
        private readonly List<string> _conditions = new List<string>();
        private readonly Dictionary<string, object?> _parameters = new Dictionary<string, object?>();

        // Db.Query にそのまま渡せる
        public Dictionary<string, object?> Parameters
        {
            get { return _parameters; }
        }

        public void AddLike(string column, string? value)
        {
            if (IsEmpty(value)) return;

            _conditions.Add(column + " LIKE @" + column);
            _parameters["@" + column] = "%" + value + "%";
        }

        public void AddEqual(string column, object? value)
        {
            if (value == null) return;

            _conditions.Add(column + " = @" + column);
            _parameters["@" + column] = value;
        }

        public void AddRange(string column, int? from, int? to, int min, int max)
        {
            // 下限は min より大きいときだけ条件にする
            if (from.HasValue && from.Value > min)
            {
                _conditions.Add(column + " >= @" + column + "From");
                _parameters["@" + column + "From"] = from.Value;
            }

            // 上限は max より小さいときだけ条件にする
            if (to.HasValue && to.Value < max)
            {
                _conditions.Add(column + " <= @" + column + "To");
                _parameters["@" + column + "To"] = to.Value;
            }
        }

        public string ToWhereClause()
        {
            if (_conditions.Count == 0) return "";
            return " WHERE " + string.Join(" AND ", _conditions);
        }

        private static bool IsEmpty(object? value)
        {
            if (value == null) return true;
            if (value is string s) return string.IsNullOrWhiteSpace(s);
            return false;

        }
    }
}
