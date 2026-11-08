using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Request;

namespace WinFormsApp2.Validation.Rules
{
    public class RangeRule : IValidationRule
    {
        public string Key { get; }
        private readonly decimal _min;
        private readonly decimal _max;
        private readonly string _message;

        public RangeRule(string key, decimal min, decimal max, string message)
        {
            Key = key;
            _min = min;
            _max = max;
            _message = message;
        }

        public string? Validate(RequestRegist request)
        {
            decimal value = request.Get<decimal>(Key);

            if (value < _min)
            {
                return _message;
            }

            if (value > _max)
            {
                return _message;
            }

            return null;   // OK
        }
    }
}
