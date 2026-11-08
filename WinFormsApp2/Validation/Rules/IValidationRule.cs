using System;
using System.Collections.Generic;
using System.Text;
using WinFormsApp2.Request;

namespace WinFormsApp2.Validation.Rules
{
    public interface IValidationRule
    {
        /// <summary>検証対象のキー(コントロール名)</summary>
        string Key { get; }

        /// <summary>OKなら null、エラーならメッセージを返す</summary>
        string? Validate(RequestRegist request);
    }
}
