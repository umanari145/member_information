using System.Data;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WinFormsApp2.Factory;
using WinFormsApp2.File;
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
            dataGridView1.AllowUserToAddRows = false;
            InitializeGridColumns();   // 列の追加は一度だけ
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            LoadAllMembers(new MemeberRequestSearch());
        }

        /// <summary>
        /// チェック列と編集ボタン列を追加します(Load で一度だけ呼ぶ)。
        /// </summary>
        private void InitializeGridColumns()
        {
            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "colCheck",
                HeaderText = "選択",
                Width = 50
            };
            dataGridView1.Columns.Add(chk);

            var btn = new DataGridViewButtonColumn
            {
                Name = "EditButton",
                HeaderText = "",
                Text = "編集",
                UseColumnTextForButtonValue = true,
                Width = 60
            };
            dataGridView1.Columns.Add(btn);
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // ヘッダー行(RowIndex = -1)のクリックは無視
            if (e.RowIndex < 0) return;

            // 編集ボタン列以外は無視
            if (dataGridView1.Columns[e.ColumnIndex].Name != "EditButton") return;

            // クリックされた行の MemberId を取得
            int memberId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MemberId"].Value);
            
            try
            {
                // 編集画面を開く
                using var form = new RegistForm(memberId); // ① 編集画面を作る
                if (form.ShowDialog() == DialogResult.OK)   // ② 画面を表示して、閉じるまで待つ
                {
                    LoadAllMembers(new MemeberRequestSearch()); // ③ 保存されていたら一覧を再読み込み
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(modCommon.Messages.SearchForm.EditOpenError + "\n" + ex.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllMembers(MemeberRequestSearch memberRequestSearch)
        {
            try
            {
                memberRequestSearch.build();

                string sql = "SELECT MemberId, Name, Age, Gender FROM Members"
                           + memberRequestSearch.ToWhereClause()
                           + " ORDER BY MemberId";

                var table = _db.Query(sql, memberRequestSearch.Parameters);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.SelectError + "\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void search_button_Click(object sender, EventArgs e)
        {
            var memberSearchClass = new MemeberRequestSearch();
            memberSearchClass.Name = name.Text.Trim();
            memberSearchClass.AgeFrom = (int)age_from.Value;
            memberSearchClass.AgeTo = (int)age_to.Value;
            memberSearchClass.Gender = GetSelectedGender();

            LoadAllMembers(memberSearchClass);
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
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                LoadAllMembers(new MemeberRequestSearch());   // 登録されたら一覧を再読み込み
            }
        }

        private void bulk_member_regist_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "CSVファイルを選択してください";
            dialog.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                dialog.Dispose();
                return;
            }

            try
            {
                string filePath = dialog.FileName;
                dialog.Dispose();

                CSVImport importer = new CSVImport(MemberValidatorFactory.Create());
                FileResult fileResult = importer.ImportCsv(
                    new File.FileInfo(filePath, 4, "Members"),
                    "MemberId",
                    MapMemberRow
                );

                MessageBox.Show(fileResult.BuildResultMessage());
            }
            finally
            {
                dialog.Dispose();
                LoadAllMembers(new MemeberRequestSearch());
            }
        }


        private RequestRegist MapMemberRow(string[] cols)
        {
            RequestRegist request = new RequestRegist();
            request["MemberId"] = cols[0].Trim();
            request["name"] = cols[1].Trim();
            request["age"] = cols[2].Trim();
            request["gender"] = cols[3].Trim();
            return request;
        }

        private void all_check_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["colCheck"].Value = true;
            }
        }

        private void all_uncheck_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["colCheck"].Value = false;
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            List<int> targetIds = GetCheckedMemberIds();

            if (targetIds.Count == 0)
            {
                MessageBox.Show(modCommon.Messages.ListMember.NoDeleteData, "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = string.Format(modCommon.Messages.ListMember.DeleteConfirm, targetIds.Count);
            DialogResult result = MessageBox.Show(message, "削除の確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _db.DeleteByIds("Members", "MemberId", targetIds);
                MessageBox.Show(modCommon.Messages.ListMember.DeleteSuccess, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.DeleteError + "\nエラー: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadAllMembers(new MemeberRequestSearch());
        }

        private List<int> GetCheckedMemberIds()
        {
            dataGridView1.EndEdit();
            var ids = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["colCheck"].Value is true)
                {
                    ids.Add(Convert.ToInt32(row.Cells["MemberId"].Value));
                }
            }
            return ids;
        }
    }
}
