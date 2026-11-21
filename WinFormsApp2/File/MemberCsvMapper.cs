using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Request;

namespace WinFormsApp2.File
{
    public class MemberCsvMapper
    {
        public static RequestRegist MapMemberRow(string[] cols)
        {
            RequestRegist request = new RequestRegist();
            request["MemberId"] = cols[0].Trim();
            request["name"] = cols[1].Trim();
            request["age"] = cols[2].Trim();
            request["gender"] = cols[3].Trim();
            return request;
        }
    }
}
