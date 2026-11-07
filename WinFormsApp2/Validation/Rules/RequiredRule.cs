using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.Validation.Rules
{
    public class RequiredRule : IValidationRule
    {
        public string Key { get; }
        private readonly string _message;

        public RequiredRule(string key, string message)
        {
            Key = key;
            _message = message;
        }

        public string? Validate(RequestCollection request)
        {
            object? value = request.Get<object>(Key);

            if (value == null)
            {
                return _message;
            }

            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return _message;
            }

            return null;   // OK
        }
    }
}
