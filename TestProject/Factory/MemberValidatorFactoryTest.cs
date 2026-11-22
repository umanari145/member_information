using System.Windows.Forms;
using WinFormsApp2.Factory;
using WinFormsApp2.Request;

namespace TestProject.Factory
{
    [TestClass]
    public class MemberValidatorFactoryTest
    {
        private static object?[][] GetPatterns()
        {
            return new[]
            {
                new object?[] { "John", 20, "男性", new ValidExpected() },
                new object?[] { "", 0, null, new ValidExpected {
                    ErrorMessage = $"名前を入力してください。{Environment.NewLine}年齢は1〜150の範囲で入力してください。{Environment.NewLine}性別を選択してください。"
                }}
            };
        }


        [TestMethod]
        [DynamicData(nameof(GetPatterns))]
        public void 通常の登録処理のバリデーション(
            string name,
            int age,
            string? gender,
            ValidExpected validExpected
            )
        {
            var screen = Create(name, age, gender);
            var request = new RequestRegist();
            request.Collect(screen);

            var validator = MemberValidatorFactory.Create();
            var result = validator.Validate(request);
            // エラー有無に関しての真偽
            Assert.AreEqual(validExpected.IsValid(), result.IsValid);
            // エラーメッセージの一致
            Assert.AreEqual(validExpected.ErrorMessage, result.ToMessage());
        }

        private static Panel Create(
            string name = "",
            decimal age = 0,
            string? gender = null)   // "男性" / "女性" / null(未選択)
        {
            var root = new Panel();

            root.Controls.Add(new TextBox { Name = "name", Text = name });
            root.Controls.Add(new NumericUpDown { Name = "age", Maximum = 150, Value = age });
            var genderPanel = new FlowLayoutPanel { Name = "gender" };
            genderPanel.Controls.Add(new RadioButton
            {
                Name = "man_check",
                Text = "男性",
                Checked = gender == "男性"
            });
            genderPanel.Controls.Add(new RadioButton
            {
                Name = "woman_check",
                Text = "女性",
                Checked = gender == "女性"
            });
            root.Controls.Add(genderPanel);

            return root;
        }

        public class ValidExpected
        {
            public bool IsValid()
            {
                return string.IsNullOrEmpty(ErrorMessage);
            }

            public string ErrorMessage { get; set; } = string.Empty;
        }
    }
}
