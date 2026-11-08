using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WinFormsApp2.Validation;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2
{
    public partial class RegistForm : Form
    {
        private readonly DB _db = new DB();

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
            validator.Add(new RequiredRule("gender_input", "性別を選択してください。"));

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                MessageBox.Show(result.ToMessage());
                return;
            }

            if (insert_member(request))
            {
                Close();
            }

        }

        private bool insert_member(RequestCollection request)
        {
            try
            {
                var (sql, parameters) = request.BuildInsert("Members");
                _db.Execute(sql, parameters);

                MessageBox.Show("登録しました。", "完了",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("会員データの登録に失敗しました。\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

        }

    }
}
