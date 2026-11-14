using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WinFormsApp2.Factory;
using WinFormsApp2.Request;
using WinFormsApp2.Validation;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2
{
    public partial class RegistForm : Form
    {
        private readonly DB _db = new DB();

        private string? _memberId = null;

        public RegistForm()
        {
            InitializeComponent();
            Shown += RegistForm_Shown;
        }

        // 編集用
        public RegistForm(int memberId) : this()
        {
            _memberId = memberId.ToString();
            Text = "会員編集";
            LoadMember(_memberId);
        }

        private void RegistForm_Load(object sender, EventArgs e)
        {

        }

        private void RegistForm_Shown(object sender, EventArgs e)
        {
            if (_memberId == null)
            {
                man_check.Checked = false;
                woman_check.Checked = false;
                name_input.Focus();
            }
        }

        private void LoadMember(string memberId)
        {
            var parameters = new Dictionary<string, object?>();
            parameters.Add("@MemberId", memberId);

            var table = _db.Query(
                "SELECT Name, Age, Gender FROM Members WHERE MemberId = @MemberId",
                parameters);

            if (table.Rows.Count == 0) return;

            var row = table.Rows[0];
            name_input.Text = row["Name"].ToString();
            age_input.Value = Convert.ToDecimal(row["Age"]);
            if (row["Gender"].ToString() == "男性")
            {
                man_check.Checked = true;
            }
            else if (row["Gender"].ToString() == "女性")
            {
                woman_check.Checked = true;
            }
        }


        private void regist_button_Click(object sender, EventArgs e)
        {
            var request = new RequestRegist();
            request.Collect(this);

            var validator = MemberValidatorFactory.Create();

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

        private bool insert_member(RequestRegist request)
        {
            try
            {
                if (_memberId == null)
                {
                    var (sql, parameters) = request.BuildInsert("Members");
                    _db.Execute(sql, parameters);
                }
                else
                {
                    var (sql, parameters) = request.BuildUpdate("Members", "MemberId", _memberId);
                    _db.Execute(sql, parameters);
                }

                MessageBox.Show("登録しました。", "完了",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
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
