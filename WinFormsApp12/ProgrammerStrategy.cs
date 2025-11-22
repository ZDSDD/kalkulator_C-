using System;
using System.Globalization;

namespace CalculatorApp
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

        public void ApplyOperator(string op)
        {
            try
            {
                long val = _display.GetCurrentInteger();
                ProgrammerOperation operation = MapOperator(op);

                _calc.ApplyOperator(val, operation);

                // Show the result and operator
                _display.ShowOperator(GetOperatorSymbol(operation), _calc.Result);
                _display.ShowIntegerResult(_calc.Result);
            }
            catch (DivideByZeroException)
            {
                _display.ShowError("Cannot divide by zero");
                _calc.Reset();
            }
            catch (OverflowException)
            {
                _display.ShowError("Overflow");
                _calc.Reset();
            }
            catch (Exception)
            {
                _display.ShowError("Error");
                _calc.Reset();
            }
        }

        public void ApplyUnary(string op)
        {
            try
            {
                if (op == "NOT") op = "~";

                long val = _display.GetCurrentInteger();
                long res = _calc.ApplyUnary(op, val);

                _display.ShowIntegerResult(res);
            }
            catch (Exception)
            {
                _display.ShowError("Error");
            }
        }

        public void CalculateResult()
        {
            try
            {
                long right = _display.GetCurrentInteger();
                long left = _calc.LeftOperand;
                string opSymbol = GetOperatorSymbol(_calc.CurrentOperator);
                if (opSymbol == "")
                {
                    // No operation to perform
                    _display.ShowExactlyOnDisplay($"{right}=", right.ToString(CultureInfo.CurrentCulture), right.ToString(CultureInfo.CurrentCulture));
                    _display.ShowResult(right);
                    return;
                }
                long res = _calc.CalculateFinal(right);

                _display.ShowEqualsIntegerResult(
                    left,
                    opSymbol,
                    _calc.LastRightOperand,
                    res
                );
            }
            catch (DivideByZeroException)
            {
                _display.ShowError("Cannot divide by zero");
                _calc.Reset();
            }
            catch (OverflowException)
            {
                _display.ShowError("Overflow");
                _calc.Reset();
            }
            catch (Exception)
            {
                _display.ShowError("Error");
                _calc.Reset();
            }
        }

        public void Clear()
        {
            _calc.Reset();
            _display.Clear();
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

        private ProgrammerOperation MapOperator(string op)
        {
            return op switch
            {
                "+" => ProgrammerOperation.Add,
                "-" => ProgrammerOperation.Subtract,
                "*" => ProgrammerOperation.Multiply,
                "/" => ProgrammerOperation.Divide,
                "&" or "AND" => ProgrammerOperation.And,
                "|" or "OR" => ProgrammerOperation.Or,
                "^" or "XOR" => ProgrammerOperation.Xor,
                "<<" or "Lsh" => ProgrammerOperation.LeftShift,
                ">>" or "Rsh" => ProgrammerOperation.RightShift,
                _ => ProgrammerOperation.None
            };
        }

        private string GetOperatorSymbol(ProgrammerOperation op)
        {
            return op switch
            {
                ProgrammerOperation.Add => "+",
                ProgrammerOperation.Subtract => "-",
                ProgrammerOperation.Multiply => "×",
                ProgrammerOperation.Divide => "÷",
                ProgrammerOperation.And => "AND",
                ProgrammerOperation.Or => "OR",
                ProgrammerOperation.Xor => "XOR",
                ProgrammerOperation.LeftShift => "Lsh",
                ProgrammerOperation.RightShift => "Rsh",
                _ => ""
            };
        }
    }
}