using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp
{
    public interface ICalculatorStrategy
    {
        void AppendNumber(string number);
        void ApplyOperator(string op);
        void ApplyUnary(string op);
        void CalculateResult();
        void Clear();
        void ToggleSign();
        void AppendDecimal();
        void ChangeBase(string baseName);
        bool IsDigitAllowed(string text);
    }


}
