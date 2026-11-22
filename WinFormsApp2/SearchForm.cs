using WinFormsApp2.Factory;
using WinFormsApp2.File;
using WinFormsApp2.Request;

namespace WinFormsApp2
{
    public partial class SearchForm : Form
    {
        private const string _tableName = "Members";
        private const string _keyColumnName = "MemberId";
        private DB _db = new DB();
        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchFormLoad(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            InitializeGridColumns();   // 列の追加は一度だけ
            dataGridView1.CellContentClick += DataGridView1CellContentClick;
            LoadAllMembers(new MemberRequestSearch());
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

        private void DataGridView1CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            // ヘッダー行(RowIndex = -1)のクリックは無視
            if (e.RowIndex < 0) return;

            // 編集ボタン列以外は無視
            if (dataGridView1.Columns[e.ColumnIndex].Name != "EditButton") return;

            // クリックされた行の MemberId を取得
            int memberId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[_keyColumnName].Value);

            bool saved = false;
            try
            {
                // 編集画面を開く
                using var form = new RegistForm(memberId);
                saved = form.ShowDialog() == DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.SearchForm.EditOpenError + "\n" + ex.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (saved)
            {
                LoadAllMembers(new MemberRequestSearch());
            }
        }

        private void LoadAllMembers(MemberRequestSearch memberRequestSearch)
        {
            try
            {
                memberRequestSearch.Build();

                string sql = "SELECT MemberId, Name, Age, Gender FROM " + _tableName
                           + memberRequestSearch.ToWhereClause()
                           + " ORDER BY " + _keyColumnName;

                dataGridView1.DataSource = _db.Query(sql, memberRequestSearch.Parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.SelectError + "\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            var memberSearchClass = new MemberRequestSearch
            {
                Name = name.Text.Trim(),
                AgeFrom = (int)age_from.Value,
                AgeTo = (int)age_to.Value,
                Gender = GetSelectedGender()
            };

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

        private void RegisterMenuItemClick(object sender, EventArgs e)
        {
            using var form = new RegistForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadAllMembers(new MemberRequestSearch());
            }
        }

        private void BulkMemberRegistToolStripMenuItemClick(object sender, EventArgs e)
        {
            string? filePath = SelectCsvFile();
            if (filePath == null)
            {
                return;
            }

            try
            {
                CSVImport importer = new CSVImport(MemberValidatorFactory.Create());
                FileResult fileResult = importer.ImportCsv(
                    new File.FileInfo(filePath, 4, _tableName),
                    _keyColumnName,
                    MemberCsvMapper.MapMemberRow
                );

                MessageBox.Show(fileResult.BuildResultMessage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.ListMember.CsvImportError + "\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadAllMembers(new MemberRequestSearch());
        }

        private string? SelectCsvFile()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "CSVファイルを選択してください";
                dialog.Filter = "CSVファイル (*.csv)|*.csv|すべてのファイル (*.*)|*.*";

                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        private void AllCheckClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["colCheck"].Value = true;
            }
        }

        private void AllUncheckClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["colCheck"].Value = false;
            }
        }

        private void DeleteClick(object sender, EventArgs e)
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

            LoadAllMembers(new MemberRequestSearch());
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
