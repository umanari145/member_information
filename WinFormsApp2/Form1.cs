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

            var conditions = new List<string>();
            var parameters = new List<SqlParameter>();

            string sql = "SELECT MemberId, Name, Age, Gender FROM Members ";

            if (!string.IsNullOrEmpty(search.Name))
            {
                conditions.Add("Name LIKE @name");
                parameters.Add(new SqlParameter("@name", "%" + search.Name + "%"));
            }

            if (search.AgeFrom > 0)
            {
                conditions.Add("Age >= @ageFrom");
                parameters.Add(new SqlParameter("@ageFrom", search.AgeFrom));
            }

            if (search.AgeTo < int.MaxValue)
            {
                conditions.Add("Age <= @ageTo");
                parameters.Add(new SqlParameter("@ageTo", search.AgeTo));
            }

            if (!string.IsNullOrEmpty(search.Gender))
            {
                conditions.Add("Gender = @gender");
                parameters.Add(new SqlParameter("@gender", search.Gender));
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
