using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsApp2.Validation;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2
{
    public partial class RegistForm : Form
    {
        public RegistForm()
        {
            InitializeComponent();
            Shown += RegistForm_Shown;
        }

        private void RegistForm_Load(object sender, EventArgs e)
        {

        }

        private void RegistForm_Shown(object sender, EventArgs e)
        {
            man_check.Checked = false;
            woman_check.Checked = false;
            name_input.Focus();
        }

        private void regist_button_Click(object sender, EventArgs e)
        {
            var request = new RequestCollection();
            request.Collect(this);

            var validator = new RequestValidator();
            validator.Add(new RequiredRule("name_input", "名前を入力してください。"));
            validator.Add(new RangeRule("age_input", 1, 150, "年齢は1〜150の範囲で入力してください。"));
            validator.Add(new RequiredRule("sex_group", "性別を選択してください。"));

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                MessageBox.Show(result.ToMessage());
                return;
            }

        }


    }
}
