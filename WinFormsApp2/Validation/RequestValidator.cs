using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Validation.Rules;

namespace WinFormsApp2.Validation
{
    public class RequestValidator
    {
        private readonly List<IValidationRule> _rules = new List<IValidationRule>();

        public void Add(IValidationRule rule)
        {
            _rules.Add(rule);
        }

        public ValidationResult Validate(RequestCollection request)
        {
            var result = new ValidationResult();

            foreach (IValidationRule rule in _rules)
            {
                string? error = rule.Validate(request);

                if (error != null)
                {
                    result.Add(rule.Key, error);
                }
            }

            return result;
        }
    }
}
