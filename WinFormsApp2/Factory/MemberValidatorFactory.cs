using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Validation;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2.Factory
{
    public static class MemberValidatorFactory
    {
        public static RequestValidator Create()
        {
            var validator = new RequestValidator();
            validator.Add(new RequiredRule("name_input", "名前を入力してください。"));
            validator.Add(new RangeRule("age_input", 1, 150, "年齢は1〜150の範囲で入力してください。"));
            validator.Add(new RequiredRule("gender_input", "性別を選択してください。"));
            return validator;
        }

    }
}
