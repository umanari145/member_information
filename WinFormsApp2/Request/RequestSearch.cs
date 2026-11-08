using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.Request
{
    public class RequestSearch
    {
        public string? Name { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public string? Gender { get; set; }
    }
}
