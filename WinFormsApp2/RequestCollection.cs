using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2
{
    public class RequestCollection : Dictionary<string, object?>
    {
        public Dictionary<string, object> GetRequestData(Control parent)
        {
            var result = new Dictionary<string, object?>();

            var radios = parent.Controls.OfType<RadioButton>().ToList();
            if (radios.Count > 0)
            {
                var selected = radios.FirstOrDefault(r => r.Checked);
                // キーは親コントロール名(例: flowLayoutPanel1)
                result[parent.Name] = selected?.Tag ?? selected?.Text;
            }

            foreach (Control c in parent.Controls)
            {
                switch (c)
                {
                    case TextBox t:
                        result[t.Name] = t.Text;
                        break;
                    case NumericUpDown n:
                        result[n.Name] = n.Value;
                        break;
                    case RadioButton r:
                        result[r.Name] = r.Checked;
                        break;
                    case CheckBox cb:
                        result[cb.Name] = cb.Checked;
                        break;
                    case ComboBox combo:
                        result[combo.Name] = combo.SelectedItem;
                        break;
                    default:
                        // Panel / FlowLayoutPanel / GroupBox などの入れ物は再帰
                        if (c.HasChildren)
                        {
                            foreach (var kv in GetRequestData(c))
                                result[kv.Key] = kv.Value;
                        }
                        break;
                }
            }
            return result;
        }

    }
}
