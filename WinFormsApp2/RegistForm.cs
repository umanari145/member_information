using WinFormsApp2.Factory;
using WinFormsApp2.Request;

namespace WinFormsApp2
{
    public partial class RegistForm : Form
    {
        private readonly DB _db = new DB();

        private string? _memberId = null;

        private readonly string _tableName = "Members";
        public RegistForm()
        {
            InitializeComponent();
            Shown += RegistFormShown;
        }

        // 編集用
        public RegistForm(int memberId) : this()
        {
            _memberId = memberId.ToString();
            Text = "会員編集";
            // コンストラクタ内でDBアクセスをさせない
            // オブジェクトが作れないので例外の扱いが難しい
            // 責務の分離が難しい
        }

        private void RegistFormLoad(object sender, EventArgs e)
        {
            if (_memberId != null)
            {
                LoadMember(_memberId);
            }
        }

        private void RegistFormShown(object sender, EventArgs e)
        {
            if (_memberId == null)
            {
                man_check.Checked = false;
                woman_check.Checked = false;
                name.Focus();
            }
        }

        private void LoadMember(string memberId)
        {
            try
            {
                var parameters = new Dictionary<string, object?>();
                parameters.Add("@MemberId", memberId);

                var table = _db.Query(
                    $"SELECT Name, Age, Gender FROM {_tableName} WHERE MemberId = @MemberId",
                    parameters);

                if (table.Rows.Count == 0) return;

                var row = table.Rows[0];
                name.Text = row["Name"].ToString();
                age.Value = Convert.ToDecimal(row["Age"]);
                if (row["Gender"].ToString() == "男性")
                {
                    man_check.Checked = true;
                }
                else if (row["Gender"].ToString() == "女性")
                {
                    woman_check.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.RegistForm.LoadFailed + "\n" + ex.Message,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // これでformの戻り値の設定される
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            var request = new RequestRegist();
            request.Collect(this);

            var validator = MemberValidatorFactory.Create();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                MessageBox.Show(result.ToMessage());
                return;
            }

            if (SaveMember(request))
            {
                Close();
            }
        }

        private bool SaveMember(RequestRegist request)
        {
            try
            {
                string message = string.Empty;
                request.Collect(this);

                if (_memberId == null)
                {
                    _db.Insert(_tableName, request, "MemberId");
                    message = modCommon.Messages.RegistForm.InsertSuccess;
                }
                else
                {
                    _db.Update(_tableName, request, "MemberId", _memberId);
                    message = modCommon.Messages.RegistForm.UpdateSuccess;
                }

                MessageBox.Show(message, "完了",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(modCommon.Messages.RegistForm.RegistFailed + "\n" + ex.StackTrace,
                                "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
    }
}
