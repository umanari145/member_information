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
            LoadAllMembers("", 0, int.MaxValue, null);
        }

        private void LoadAllMembers(string nameKeyword, int ageFrom, int ageTo, string? gender)
        {

            var conditions = new List<string>();
            var parameters = new List<SqlParameter>();

            string sql = "SELECT MemberId, Name, Age, Gender FROM Members ";

            if (!string.IsNullOrEmpty(nameKeyword))
            {
                conditions.Add("Name LIKE @name");
                parameters.Add(new SqlParameter("@name", "%" + nameKeyword + "%"));
            }

            if (ageFrom > 0)
            {
                conditions.Add("Age >= @ageFrom");
                parameters.Add(new SqlParameter("@ageFrom", ageFrom));
            }

            if (ageTo < int.MaxValue)
            {
                conditions.Add("Age <= @ageTo");
                parameters.Add(new SqlParameter("@ageTo", ageTo));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                conditions.Add("Gender = @gender");
                parameters.Add(new SqlParameter("@gender", gender));
            }

            if (conditions.Count > 0)
            {
                sql += " WHERE " + string.Join(" AND ", conditions);
            }

            sql += " ORDER BY MemberId";

            try
            {
                using var conn = new SqlConnection(_connStr);
                using var adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());

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
            string nameKeyword = name.Text.Trim();

            int ageFrom = (int)age_from.Value;
            int ageTo = (int)age_to.Value;

            string? gender = null;
            if (man_radio_button.Checked)
            {
                gender = "男性";
            }
            else if (woman_radio_button.Checked)
            {
                gender = "女性";
            }



            LoadAllMembers(
                nameKeyword,
                ageFrom,
                ageTo,
                gender
            );
        }
    }
}
