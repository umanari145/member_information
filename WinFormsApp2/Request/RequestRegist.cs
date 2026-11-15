using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2.Request
{
    public class RequestRegist : Dictionary<string, object?>
    {
        /// <summary>
        /// コントロールから値を読み取り、このインスタンスに保持します。
        /// </summary>
        public void Collect(Control parent)
        {
            // 直下にラジオボタンがあれば、親名をキーに選択値を保持
            var radios = parent.Controls.OfType<RadioButton>().ToList();
            if (radios.Count > 0)
            {
                var selected = radios.FirstOrDefault(r => r.Checked);
                this[parent.Name] = selected?.Tag ?? selected?.Text;
            }

            foreach (Control c in parent.Controls)
            {
                switch (c)
                {
                    case TextBox t:
                        this[t.Name] = t.Text;
                        break;
                    case NumericUpDown n:
                        this[n.Name] = n.Value;
                        break;
                    case RadioButton:
                        // 親名でまとめて保持済みなので個別には入れない
                        break;
                    case CheckBox cb:
                        this[cb.Name] = cb.Checked;
                        break;
                    case ComboBox combo:
                        this[combo.Name] = combo.SelectedItem;
                        break;
                    default:
                        if (c.HasChildren)
                            Collect(c);   // 同じインスタンスに書き込む
                        break;
                }
            }
        }

        /// <summary>
        /// 保持している値を型付きで取得します。存在しない・変換できない場合は既定値を返します。
        /// </summary>
        public T? Get<T>(string key, T? defaultValue = default)
        {
            if (!TryGetValue(key, out var value) || value is null)
                return defaultValue;

            if (value is T typed)
                return typed;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        private Dictionary<string, object?> ToColumnValues()
        {
            var result = new Dictionary<string, object?>();

            foreach (var item in this)
            {
                string columnName = item.Key;
                result[columnName] = item.Value;
            }

            return result;
        }

        public (string Sql, Dictionary<string, object?> Parameters) BuildInsert(string tableName)
        {
            var values = ToColumnValues();

            var columns = new List<string>();
            var placeholders = new List<string>();
            var parameters = new Dictionary<string, object?>();

            foreach (var item in values)
            {
                columns.Add(item.Key);
                placeholders.Add("@" + item.Key);
                parameters["@" + item.Key] = item.Value;
            }

            string sql = "INSERT INTO " + tableName +
                         " (" + string.Join(", ", columns) + ")" +
                         " VALUES (" + string.Join(", ", placeholders) + ")";

            return (sql, parameters);
        }

        public (string Sql, Dictionary<string, object?> Parameters) BuildUpdate(string tableName, string idColumn, string idValue)
        {
            var values = ToColumnValues();
            var sets = new List<string>();
            var parameters = new Dictionary<string, object?>();

            foreach (var item in values)
            {
                // WHERE 句のキー列は SET に含めない
                if (string.Equals(item.Key, idColumn, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                sets.Add(item.Key + " = @" + item.Key);
                parameters["@" + item.Key] = item.Value;
            }

            // WHERE 句用のパラメータを追加
            parameters["@" + idColumn] = idValue;

            string sql = "UPDATE " + tableName +
                         " SET " + string.Join(", ", sets) +
                         " WHERE " + idColumn + " = @" + idColumn;

            return (sql, parameters);
        }
    }
}