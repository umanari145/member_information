using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Request;

namespace WinFormsApp2.Validation.Rules
{
    public class OptionalNumericRule : IValidationRule
    {
        public string Key { get; }
        private readonly string _message;

        public OptionalNumericRule(string key, string message)
        {
            Key = key;
            _message = message;
        }

        public string? Validate(RequestRegist request)
        {
            string? rawValue = request.Get<string?>(Key);

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return null;
            }

            //TryParse で「数値に変換できるか」を判定しつつ、成功すれば decimal に変換する
            if (!decimal.TryParse(rawValue, out decimal value))
            {
                return _message; // 数値ではない場合はエラーメッセージを返す
            }

            return null;
        }
    }
}
