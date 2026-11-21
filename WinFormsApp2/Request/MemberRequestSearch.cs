using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.Request
{
    public class MemberRequestSearch : RequestSearch
    {
        public string? Name { get; set; }
        public int? AgeFrom { get; set; }
        public int? AgeTo { get; set; }
        public string? Gender { get; set; }

        public void build()
        {
            AddLike("Name", this.Name);
            AddRange("Age", this.AgeFrom, this.AgeTo, 0, 150);
            AddEqual("Gender", this.Gender);
        }
    }
}
