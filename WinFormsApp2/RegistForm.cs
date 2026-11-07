using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
            var requestCollection = new RequestCollection();
            var request = requestCollection.GetRequestData(this);

            string name = (string)request["name_input"]!;
            int age = (int)(decimal)request["age_input"]!;
            string? sex = (string?)request["sex_group"];

            MessageBox.Show($"名前: {name}\n年齢: {age}\n性別: {sex}", "登録情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
