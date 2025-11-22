using System;
using System.Globalization;

namespace CalculatorApp
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
            if (number == "0" && _display.GetCurrentValue() == 0)
                return;
            _display.AppendNumber(number);
        }

        public void ApplyOperator(string op)
        {
            try
            {
                double currentValue = _display.GetCurrentValue();
                _calc.ApplyOperator(currentValue, OperatorStringToStandardOperation(op));

                // Show the result after operation and the operator in equation label
                _display.ShowOperator(GetOperatorSymbol(op), _calc.Result);
                _display.ShowResult(_calc.Result);
            }
            catch (DivideByZeroException)
            {
                _display.ShowError("Cannot divide by zero");
                _calc.Reset();
            }
            catch (Exception ex)
            {
                _display.ShowError("Error");
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
                string prepared_striog = op switch
                {
                    "1/x" => "1/(" + value + ")",
                    "x^2" => "sqr(" + value + ")",
                    "sqrt" => "√(" + value + ")",
                    "%" => "(" + value + ")%",
                    _ => value.ToString()
                };
                _display.ShowExactlyOnDisplay(prepared_striog+"=", prepared_striog, unary.ToString(CultureInfo.CurrentCulture));
                _display.ShowResult(unary);
            }
            catch (DivideByZeroException)
            {
                _display.ShowError("Cannot divide by zero");
            }
            catch (ArgumentException)
            {
                _display.ShowError("Invalid input");
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
                double right = _display.GetCurrentValue();
                double left = _calc.LeftOperand;
                string opSymbol = CurrentOperatorString();
                if (opSymbol == "")
                {
                    // No operation to perform
                    _display.ShowExactlyOnDisplay($"{right}=", right.ToString(CultureInfo.CurrentCulture), right.ToString(CultureInfo.CurrentCulture));
                    _display.ShowResult(right);
                    return;
                }
                double res = _calc.CalculateFinal(right);

                _display.ShowEqualsResult(
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

        public void ToggleSign() => _display.ToggleSign();
        public void AppendDecimal() => _display.AppendDecimal();
        public void ChangeBase(string b) { }
        public bool IsDigitAllowed(string text) => true;

        // Convert operator symbol to display symbol
        private string GetOperatorSymbol(string op)
        {
            return op switch
            {
                "*" => "×",
                "/" => "÷",
                _ => op
            };
        }

        private string CurrentOperatorString()
        {
            return _calc.CurrentOperator switch
            {
                StandardOperation.Add => "+",
                StandardOperation.Subtract => "-",
                StandardOperation.Multiply => "×",
                StandardOperation.Divide => "÷",
                _ => ""
            };
        }

        private StandardOperation OperatorStringToStandardOperation(string op)
        {
            return op switch
            {
                "+" => StandardOperation.Add,
                "-" => StandardOperation.Subtract,
                "*" or "×" => StandardOperation.Multiply,
                "/" or "÷" => StandardOperation.Divide,
                _ => StandardOperation.None
            };
        }
    }
}