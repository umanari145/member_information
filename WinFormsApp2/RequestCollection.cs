using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class RequestCollection : Dictionary<string, object?>
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
    }
}