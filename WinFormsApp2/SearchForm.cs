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
            LoadAllMembers(new RequestSearch());
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // ヘッダー行(RowIndex = -1)のクリックは無視
            if (e.RowIndex < 0) return;

            // 編集ボタン列以外は無視
            if (dataGridView1.Columns[e.ColumnIndex].Name != "EditButton") return;

            // クリックされた行の MemberId を取得
            int memberId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MemberId"].Value);

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
                

                if (!dataGridView1.Columns.Contains("colCheck"))
                {
                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                    chk.Name = "colCheck";
                    chk.HeaderText = "選択";
                    chk.Width = 50;
                    dataGridView1.Columns.Insert(0, chk);
                }

                if (!dataGridView1.Columns.Contains("EditButton"))
                {
                    DataGridViewButtonColumn btnColumn1 = new DataGridViewButtonColumn();
                    btnColumn1.Name = "EditButton";
                    btnColumn1.HeaderText = "";
                    btnColumn1.Text = "編集";
                    btnColumn1.UseColumnTextForButtonValue = true;
                    btnColumn1.Width = 60;

                    dataGridView1.Columns.Insert(1, btnColumn1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.SelectError + "\n" + ex.Message,
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
                LoadAllMembers(new RequestSearch());
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
            dataGridView1.EndEdit();

             List<int> targetIds = new List<int>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // 最下行の新規入力行などはスキップ
                if (row.IsNewRow) continue;

                var isChecked = row.Cells["colCheck"].Value;
                if (isChecked != null && (bool)isChecked == true)
                {
                    string? memberIdVal = row.Cells["MemberId"].Value?.ToString();
                    if (!string.IsNullOrEmpty(memberIdVal))
                    {
                        targetIds.Add(Convert.ToInt32(memberIdVal));
                    }
                }
            }

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
                Dictionary<string, object?> parameters = new Dictionary<string, object?>();
                List<string> paramNames = new List<string>();

                for (int i = 0; i < targetIds.Count; i++)
                {
                    string paramName = $"@id{i}";          // "@id0", "@id1", "@id2" ...
                    paramNames.Add(paramName);             // リストに保持
                    parameters.Add(paramName, targetIds[i]); // 辞書にパラメータ名と値を追加
                }

                string ids = string.Join(",", targetIds);
                string deleteSql = $"DELETE FROM Members WHERE MemberId IN ({string.Join(",", paramNames)})";

                _db.Execute(deleteSql, parameters);
                MessageBox.Show(modCommon.Messages.ListMember.DeleteSuccess, "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.DeleteError + "\nエラー: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            LoadAllMembers(new RequestSearch());
        }
    }
}
