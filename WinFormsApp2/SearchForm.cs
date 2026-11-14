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
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // ヘッダー行(RowIndex = -1)のクリックは無視
            if (e.RowIndex < 0) return;

            // 編集ボタン列以外は無視
            if (dataGridView1.Columns[e.ColumnIndex].Name != "EditButton") return;

            // クリックされた行の MemberId を取得
            int memberId = Convert.ToInt32(
                dataGridView1.Rows[e.RowIndex].Cells["MemberId"].Value);

            // 編集画面を開く
            var form = new RegistForm(memberId); // ① 編集画面を作る
            try
            {
                if (form.ShowDialog() == DialogResult.OK)   // ② 画面を表示して、閉じるまで待つ
                {
                    LoadAllMembers(new RequestSearch());    // ③ 保存されていたら一覧を再読み込み
                }
            }
            finally
            {
                form.Dispose();                       // ④ 画面を破棄する
            }
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
            var form = new RegistForm();
            try
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    LoadAllMembers(new RequestSearch());   // 登録されたら一覧を再読み込み
                }
            }
            finally
            {
                form.Dispose();                       // ④ 画面を破棄する
            }
        }

        private void bulk_member_regist_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            try
            {
                dialog.Filter = "すべてのファイル (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("選択されたファイル: " + dialog.FileName);
                }
            }
            finally
            {
                dialog.Dispose();
            }
        }
    }
}
