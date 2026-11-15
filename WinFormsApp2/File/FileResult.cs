using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinFormsApp2.File
{
    public class FileResult
    {
        public int InsertCount { get; set; }
        public int UpdateCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public string BuildResultMessage()
        {
            List<string> parts = new List<string>();

            if (this.InsertCount > 0)
            {
                parts.Add("登録行: " + this.InsertCount + "行");
            }

            if (this.UpdateCount > 0)
            {
                parts.Add("更新行: " + this.UpdateCount + "行");
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