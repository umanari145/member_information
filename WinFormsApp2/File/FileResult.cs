using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsApp2.File
{
    public class FileResult
    {
        public int InsertCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public string BuildResultMessage()
        {
            List<string> parts = new List<string>();

            // 登録行(0行の場合は非表示)
            if (this.InsertCount > 0)
            {
                parts.Add("登録行: " + this.InsertCount + "行");
            }

            // エラーを改行つなぎで表示
            if (this.Errors.Count > 0)
            {
                parts.Add(string.Join(Environment.NewLine, this.Errors));
            }

            return string.Join(Environment.NewLine, parts);
        }
    }
}