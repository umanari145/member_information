using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private readonly string? _connStr;
        public Form1()
        {
            InitializeComponent();
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _connStr = config.GetConnectionString("Members")
                       ?? throw new InvalidOperationException("接続文字列 'Members' が appsettings.json に見つかりません。");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllMembers(new SearchClass());
        }

        private void LoadAllMembers(SearchClass search)
        {
            var where = new WhereBuilder();
            where.AddLike("Name", search.Name);
            where.AddRange("Age", search.AgeFrom, search.AgeTo, 0, 150);
            where.AddEqual("Gender", search.Gender);

            string sql = "SELECT MemberId, Name, Age, Gender FROM Members"
                       + where.ToWhereClause()
                       + " ORDER BY MemberId";

            try
            {
                using var conn = new SqlConnection(_connStr);
                using var adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddRange(where.Parameters);

                var table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("会員データの取得に失敗しました。\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            var searchClass = new SearchClass();
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
    }
}
