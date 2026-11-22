using WinFormsApp2.Validation;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2.Factory
{
    public static class MemberValidatorFactory
    {
        public static RequestValidator Create()
        {
            var validator = new RequestValidator();
            validator.Add(new RequiredRule("name", modCommon.Messages.RegistForm.RequiredName));
            validator.Add(new RangeRule("age", 1, 150, modCommon.Messages.RegistForm.RangeAge));
            validator.Add(new RequiredRule("gender", modCommon.Messages.RegistForm.RequiredGender));

            // CSV固有のルール（必須ではないが、入力されていたら数値であること）
            validator.Add(new OptionalNumericRule("MemberId", modCommon.Messages.RegistForm.OptionalRequiredMemberId));
            return validator;
        }
    }
}
