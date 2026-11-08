using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WinFormsApp2.Request;

namespace WinFormsApp2
{
    public partial class SearchForm : Form
    {
        private readonly DB _db = new DB();
        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            LoadAllMembers(new RequestSearch());
        }

        private void LoadAllMembers(RequestSearch requestSearch)
        {
            try
            {
                var where = new WhereBuilder();
                where.AddLike("Name", requestSearch.Name);
                where.AddRange("Age", requestSearch.AgeFrom, requestSearch.AgeTo, 0, 150);
                where.AddEqual("Gender", requestSearch.Gender);

                string sql = "SELECT MemberId, Name, Age, Gender FROM Members"
                           + where.ToWhereClause()
                           + " ORDER BY MemberId";

                var table = _db.Query(sql, where.Parameters);
                dataGridView1.DataSource = table;
                
                if (!dataGridView1.Columns.Contains("EditButton"))
                {
                    var btnColumn = new DataGridViewButtonColumn();
                    btnColumn.Name = "EditButton";
                    btnColumn.HeaderText = "";
                    btnColumn.Text = "編集";
                    btnColumn.UseColumnTextForButtonValue = true;
                    btnColumn.Width = 60;

                    dataGridView1.Columns.Insert(0, btnColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("会員データの取得に失敗しました。\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            var searchClass = new RequestSearch();
            searchClass.Name = name.Text.Trim();
            searchClass.AgeFrom = (int)age_from.Value;
            searchClass.AgeTo = (int)age_to.Value;
            searchClass.Gender = GetSelectedGender();

            LoadAllMembers(searchClass);
        }

        private string? GetSelectedGender()
        {
            if (man_radio_button.Checked)
            {
                return "男性";
            }
            else if (woman_radio_button.Checked)
            {
                return "女性";
            }
            else
            {
                return null;
            }
        }

        private void register_menu_item_Click(object sender, EventArgs e)
        {
            using var form = new RegistForm();
            form.ShowDialog(this);
        }
    }
}
