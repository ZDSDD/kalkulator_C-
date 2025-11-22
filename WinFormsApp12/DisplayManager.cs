using System;
using System.Globalization;
using System.Windows.Forms;

namespace CalculatorApp
{
    public class DisplayManager
    {
        private readonly TextBox _display;
        private readonly Label _equationLabel;
        private readonly HistoryManager _historyManager; // Dependency for logging

        private bool _shouldClearOnNextInput = true;
        private const int MaxInputLength = 20;

        public NumberBase CurrentBase { get; set; } = NumberBase.Decimal;
        public CalculatorMode CurrentMode { get; set; } = CalculatorMode.Standard;

        public DisplayManager(TextBox display, Label equationLabel, HistoryManager historyManager)
        {
            _display = display;
            _equationLabel = equationLabel;
            _historyManager = historyManager;
        }

        #region Output Methods

        public void ShowError(string message)
        {
            _display.Text = message;
            _equationLabel.Text = string.Empty;
            _shouldClearOnNextInput = true;
        }

        public void ShowResult(double result)
        {
            _display.Text = result.ToString(CultureInfo.CurrentCulture);
            _shouldClearOnNextInput = true;
        }

        public void ShowIntegerResult(long result)
        {
            _display.Text = ConvertToString(result, CurrentBase);
            _shouldClearOnNextInput = true;
        }

        public void ShowOperator(string operatorSymbol, double leftValue)
        {
            _equationLabel.Text = $"{leftValue.ToString(CultureInfo.CurrentCulture)} {operatorSymbol}";
            _shouldClearOnNextInput = true;
        }

        public void ShowOperator(string operatorSymbol, long leftValue)
        {
            string leftStr = ConvertToString(leftValue, CurrentBase);
            _equationLabel.Text = $"{leftStr} {operatorSymbol}";
            _shouldClearOnNextInput = true;
        }

        public void ShowEqualsResult(double left, string op, double right, double result)
        {
            string leftStr = left.ToString(CultureInfo.CurrentCulture);
            string rightStr = right.ToString(CultureInfo.CurrentCulture);
            string resultStr = result.ToString(CultureInfo.CurrentCulture);

            UpdateDisplayAfterCalculation(leftStr, op, rightStr, resultStr);
        }

        public void ShowEqualsIntegerResult(long left, string op, long right, long result)
        {
            string leftStr = ConvertToString(left, CurrentBase);
            string rightStr = ConvertToString(right, CurrentBase);
            string resultStr = ConvertToString(result, CurrentBase);

            UpdateDisplayAfterCalculation(leftStr, op, rightStr, resultStr);
        }

        private void UpdateDisplayAfterCalculation(string left, string op, string right, string result)
        {
            // 1. Update Screen
            _display.Text = result;
            _equationLabel.Text = $"{left} {op} {right} =";

            // 2. Log to History
            _historyManager.AddEntry(left, op, right, result);

            _shouldClearOnNextInput = true;
        }

        #endregion

        #region Input Methods

        public void AppendNumber(string number)
        {
            PrepareForInput();

            if (_display.Text.Length >= MaxInputLength) return;

            if (_display.Text == "0" && number != ".")
                _display.Text = number;
            else
                _display.Text += number;
        }

        public void AppendDecimal()
        {
            if (CurrentMode == CalculatorMode.Programmer) return;

            string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (_shouldClearOnNextInput)
            {
                _display.Text = "0" + sep;
                _shouldClearOnNextInput = false;
                return;
            }

            if (_display.Text.Length >= MaxInputLength) return;
            if (_display.Text.Contains(sep)) return;

            _display.Text += sep;
        }

        public void ToggleSign()
        {
            if (CurrentMode == CalculatorMode.Programmer) return;
            if (_display.Text == "0") return;

            if (_shouldClearOnNextInput) _shouldClearOnNextInput = false;

            if (_display.Text.StartsWith("-"))
                _display.Text = _display.Text.Substring(1);
            else
                _display.Text = "-" + _display.Text;
        }

        public void Clear()
        {
            _display.Text = "0";
            _equationLabel.Text = string.Empty;
            _shouldClearOnNextInput = true;
        }

        public void ClearHistory()
        {
            _historyManager.Clear();
        }

        private void PrepareForInput()
        {
            if (_shouldClearOnNextInput)
            {
                _display.Text = string.Empty;
                _shouldClearOnNextInput = false;
            }
        }

        #endregion

        #region Helpers (Conversion)

        public double GetCurrentValue()
        {
            if (double.TryParse(_display.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double val))
                return val;
            return 0;
        }

        public long GetCurrentInteger()
        {
            string text = _display.Text;
            if (string.IsNullOrEmpty(text)) return 0;

            try
            {
                return CurrentBase switch
                {
                    NumberBase.Hexadecimal => Convert.ToInt64(text, 16),
                    NumberBase.Octal => Convert.ToInt64(text, 8),
                    NumberBase.Binary => Convert.ToInt64(text, 2),
                    _ => Convert.ToInt64(text, 10),
                };
            }
            catch { return 0; }
        }

        private string ConvertToString(long value, NumberBase nBase)
        {
            return nBase switch
            {
                NumberBase.Hexadecimal => Convert.ToString(value, 16).ToUpper(),
                NumberBase.Octal => Convert.ToString(value, 8),
                NumberBase.Binary => Convert.ToString(value, 2),
                _ => value.ToString()
            };
        }

        #endregion
    }

    public enum CalculatorMode
    {
        Programmer,
        Standard
    }
}