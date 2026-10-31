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
            LoadAllMembers();
        }

        private void LoadAllMembers()
        {
            string sql = "SELECT MemberId, Name, Age, Gender FROM Members ORDER BY MemberId";

            try
            {
                using var conn = new SqlConnection(_connStr);
                using var adapter = new SqlDataAdapter(sql, conn);

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

        private void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
