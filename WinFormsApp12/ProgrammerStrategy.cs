using System;

namespace WinFormsApp12
{
    public class ProgrammerStrategy : ICalculatorStrategy
    {
        private readonly ProgrammerCalculator _calc;
        private readonly DisplayManager _display;
        private NumberBase _currentBase = NumberBase.Decimal;

        public string CurrentBaseName => _currentBase switch
        {
            NumberBase.Hexadecimal => "HEX",
            NumberBase.Octal => "OCT",
            NumberBase.Binary => "BIN",
            _ => "DEC"
        };

        public ProgrammerStrategy(ProgrammerCalculator calc, DisplayManager display)
        {
            _calc = calc;
            _display = display;
        }

        public void AppendNumber(string number) => _display.AppendNumber(number);

        private ProgrammerOperation MapOperator(string op)
        {
            return op switch
            {
                "+" => ProgrammerOperation.Add,
                "-" => ProgrammerOperation.Subtract,
                "*" => ProgrammerOperation.Multiply,
                "/" => ProgrammerOperation.Divide,
                "&" => ProgrammerOperation.And,
                "|" => ProgrammerOperation.Or,
                "^" => ProgrammerOperation.Xor,
                "<<" => ProgrammerOperation.LeftShift,
                ">>" => ProgrammerOperation.RightShift,

                "Lsh" => ProgrammerOperation.LeftShift,
                "Rsh" => ProgrammerOperation.RightShift,
                "AND" => ProgrammerOperation.And,
                "OR" => ProgrammerOperation.Or,
                "XOR" => ProgrammerOperation.Xor,

                _ => ProgrammerOperation.None
            };
        }

        public void ApplyOperator(string op)
        {
            try
            {
                long val = _display.GetCurrentInteger();

                _calc.ApplyOperator(val, MapOperator(op));
                _display.ShowIntegerResult(_calc.LeftOperand);
            }
            catch (Exception ex)
            {
                _display.ShowError(ex.Message);
                _calc.Reset();
            }
        }

        public void ApplyUnary(string op)
        {
            if (op == "NOT") op = "~";

            long val = _display.GetCurrentInteger();
            long res = _calc.ApplyUnary(op, val);

            _display.ShowIntegerResult(res);
        }

        public void CalculateResult()
        {
            long right = _display.GetCurrentInteger();
            long res = _calc.CalculateFinal(right);

            _display.ShowEqualsIntegerResult(
                _calc.LeftOperand,
                _calc.CurrentOperator.ToString(),
                right, res);
        }

        public void Clear()
        {
            _calc.Reset();
            _display.Clear();
            _display.ClearHistory();
        }

        public void ToggleSign() { }
        public void AppendDecimal() { }

        public void ChangeBase(string baseName)
        {
            long val = _display.GetCurrentInteger();

            _currentBase = baseName switch
            {
                "HEX" => NumberBase.Hexadecimal,
                "DEC" => NumberBase.Decimal,
                "OCT" => NumberBase.Octal,
                "BIN" => NumberBase.Binary,
                _ => _currentBase
            };

            _display.CurrentBase = _currentBase;
            _display.ShowIntegerResult(val);
        }
        public bool IsDigitAllowed(string text)
        {
            if (_currentBase == NumberBase.Hexadecimal)
                return true;

            return _currentBase switch
            {
                NumberBase.Decimal => "0123456789".Contains(text),
                NumberBase.Octal => "01234567".Contains(text),
                NumberBase.Binary => "01".Contains(text),
                _ => false
            };
        }
    }
}
