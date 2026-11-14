using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.File
{
    public class FileInfo
    {
        public string filePath { get; set; }
        public int colLength { get; set; }
        public  string tableName { get; set; }

        public FileInfo(string filePath, int colLength, string tableName)
        {
            this.filePath = filePath;
            this.colLength = colLength;
            this.tableName = tableName;
        }
    }
}
