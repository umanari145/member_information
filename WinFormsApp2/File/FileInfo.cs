using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.File
{
    public class FileInfo
    {
        public string FilePath { get; set; }
        public int ColLength { get; set; }
        public string TableName { get; set; }

        public FileInfo(string filePath, int colLength, string tableName)
        {
            this.FilePath = filePath;
            this.ColLength = colLength;
            this.TableName = tableName;
        }
    }
}
