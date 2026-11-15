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
            validator.Add(new RequiredRule("name", "名前を入力してください。"));
            validator.Add(new RangeRule("age", 1, 150, "年齢は1〜150の範囲で入力してください。"));
            validator.Add(new RequiredRule("gender", "性別を選択してください。"));

            // CSV固有のルール（必須ではないが、入力されていたら数値であること）
            validator.Add(new OptionalNumericRule("MemberId", "会員IDは数値で入力してください。"));
            return validator;
        }
    }
}
