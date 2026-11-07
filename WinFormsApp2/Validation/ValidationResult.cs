using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp2.Validation
{
    public class ValidationResult
    {
        private readonly List<(string Key, string Message)> _errors = new();

        public bool IsValid => _errors.Count == 0;
        public IReadOnlyList<(string Key, string Message)> Errors => _errors;

        public void Add(string key, string message) => _errors.Add((key, message));

        /// <summary>全エラーを改行区切りで返します。</summary>
        public string ToMessage() =>
            string.Join(Environment.NewLine, _errors.Select(e => e.Message));
    }
}
