using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp12
{
    public class StandardStrategy : ICalculatorStrategy
    {
        private readonly Calculator _calc;
        private readonly DisplayManager _display;

        public StandardStrategy(Calculator calc, DisplayManager display)
        {
            _calc = calc;
            _display = display;
        }

        public void AppendNumber(string number)
        {
            _display.AppendNumber(number);
        }

        public void ApplyOperator(string op)
        {
            try
            {
                double currentValue = _display.GetCurrentValue();

                _calc.ApplyOperator(currentValue, OperatorStringToStandardOperation(op));

                _display.ShowOperator(op, _calc.LeftOperand);
                _display.ShowResult(_calc.Result);
            }
            catch (Exception ex)
            {
                _display.ShowError(ex.Message);
                _calc.Reset();
            }
        }

        public void ApplyUnary(string op)
        {
            try
            {
                double value = _display.GetCurrentValue();
                double unary = op switch
                {
                    "1/x" => Calculator.CalculateOneDivX(value),
                    "x^2" => Calculator.CalculateSquare(value),
                    "sqrt" => Calculator.CalculateSqrt(value),
                    "%" => Calculator.CalculatePercentage(value),
                    _ => value
                };

                _display.ShowResult(unary);
            }
            catch (Exception ex)
            {
                _display.ShowError(ex.Message);
                _calc.Reset();
            }
        }
        public string CurrentOperatorString(StandardOperation standardOperation)
        {
            return standardOperation switch
            {
                StandardOperation.Add => "+",
                StandardOperation.Subtract => "-",
                StandardOperation.Multiply => "×",
                StandardOperation.Divide => "÷",
                StandardOperation.None => "",
                _ => ""
            };
        }
        public string CurrentOperatorString()
        {
            return CurrentOperatorString(_calc.CurrentOperator);
        }

        public StandardOperation OperatorStringToStandardOperation(string op)
        {
            return op switch
            {
                "+" => StandardOperation.Add,
                "-" => StandardOperation.Subtract,
                "×" => StandardOperation.Multiply,
                "÷" => StandardOperation.Divide,
                _ => StandardOperation.None
            };
        }
        public void CalculateResult()
        {
            try
            {
                double right = _display.GetCurrentValue();
                double res = _calc.CalculateFinal(right);

                _display.ShowEqualsResult(
                    _calc.LeftOperand,
                    CurrentOperatorString(),
                    right,
                    res
                );
            }
            catch (Exception ex)
            {
                _display.ShowError(ex.Message);
                _calc.Reset();
            }
        }

        public void Clear()
        {
            _calc.Reset();
            _display.Clear();
            _display.ClearHistory();
        }

        public void ToggleSign() => _display.ToggleSign();
        public void AppendDecimal() => _display.AppendDecimal();
        public void ChangeBase(string b) { }
        public bool IsDigitAllowed(string text) => true;
    }


}
