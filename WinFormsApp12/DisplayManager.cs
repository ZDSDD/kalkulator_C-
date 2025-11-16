using System;
using System.Globalization;
using System.Windows.Forms;

namespace WinFormsApp12
{
    public enum CalculatorMode
    {
        Standard,
        Programmer
    }

    public enum NumberBase
    {
        Hexadecimal,
        Decimal,
        Octal,
        Binary
    }

    /// <summary>
    /// Manages what shows on the screen
    /// </summary>
    public class DisplayManager
    {
        private readonly TextBox display;
        private readonly Label history;
        private readonly ListBox historyList;
        private bool shouldClearOnNextInput = true;

        private const int MaxInputLength = 20;

        public NumberBase CurrentBase { get; set; } = NumberBase.Decimal;

        public CalculatorMode CurrentMode { get; set; } = CalculatorMode.Standard;

        public DisplayManager(TextBox display, Label history, ListBox historyList)
        {
            this.display = display;
            this.history = history;
            this.historyList = historyList;
        }

        public void ShowError(string message)
        {
            display.Text = message;
            history.Text = string.Empty;
            shouldClearOnNextInput = true;
        }

        private void PrepareForInput()
        {
            if (shouldClearOnNextInput)
            {
                display.Text = "0";
                shouldClearOnNextInput = false;
            }
        }

        private string ConvertToString(long value, NumberBase nBase)
        {
            switch (nBase)
            {
                case NumberBase.Hexadecimal:
                    return Convert.ToString(value, 16).ToUpper();
                case NumberBase.Octal:
                    return Convert.ToString(value, 8);
                case NumberBase.Binary:
                    return Convert.ToString(value, 2);
                case NumberBase.Decimal:
                default:
                    return value.ToString();
            }
        }

        public void ShowIntegerResult(long result)
        {
            display.Text = ConvertToString(result, CurrentBase);
            shouldClearOnNextInput = true;
        }

        public void ShowEqualsIntegerResult(long leftValue, string operatorSymbol, long rightValue, long result)
        {
            string leftStr = ConvertToString(leftValue, CurrentBase);
            string rightStr = ConvertToString(rightValue, CurrentBase);
            string resultStr = ConvertToString(result, CurrentBase);

            string calculation = $"{leftStr} {operatorSymbol} {rightStr} = {resultStr}";
            history.Text = $"{leftStr} {operatorSymbol} {rightStr} =";
            display.Text = resultStr;

            historyList.Items.Add(calculation);
            historyList.TopIndex = historyList.Items.Count - 1;

            shouldClearOnNextInput = true;
        }

        public long GetCurrentInteger()
        {
            string text = display.Text ?? "0";
            if (string.IsNullOrEmpty(text)) return 0;

            try
            {
                switch (CurrentBase)
                {
                    case NumberBase.Hexadecimal:
                        return Convert.ToInt64(text, 16);
                    case NumberBase.Octal:
                        return Convert.ToInt64(text, 8);
                    case NumberBase.Binary:
                        return Convert.ToInt64(text, 2);
                    case NumberBase.Decimal:
                    default:
                        return Convert.ToInt64(text, 10);
                }
            }
            catch (FormatException)
            {
                return 0;
            }
            catch (OverflowException)
            {
                return long.MaxValue;
            }
        }

        public void AppendNumber(string number)
        {
            if (CurrentMode == CalculatorMode.Programmer)
            {
                if (CurrentBase == NumberBase.Binary && (number != "0" && number != "1")) return;
                if (CurrentBase == NumberBase.Octal && int.Parse(number) > 7) return;
            }

            PrepareForInput();

            if (display.Text.Length >= MaxInputLength) return;

            if (display.Text == "0")
            {
                if (number == "0") return;
                display.Text = number;
            }
            else
            {
                display.Text += number;
            }
        }

        public void AppendDecimal()
        {
            if (CurrentMode == CalculatorMode.Programmer) return;

            string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            if (shouldClearOnNextInput)
            {
                if (!string.IsNullOrEmpty(display.Text) && display.Text.StartsWith("-"))
                {
                    display.Text = "-0" + sep;
                }
                else
                {
                    display.Text = "0" + sep;
                }

                shouldClearOnNextInput = false;
                return;
            }

            if (display.Text.Length >= MaxInputLength) return;

            if (display.Text == "-")
            {
                display.Text = "-0" + sep;
                return;
            }

            if (!display.Text.Contains(sep))
            {
                display.Text += sep;
            }
        }

        public void ToggleSign()
        {
            if (CurrentMode == CalculatorMode.Programmer) return;

            if (shouldClearOnNextInput)
            {
                PrepareForInput();
            }

            if (display.Text == "0") return;

            if (display.Text.StartsWith("-"))
            {
                display.Text = display.Text.Substring(1);
            }
            else
            {
                display.Text = "-" + display.Text;
            }
        }

        public void Backspace()
        {
            if (shouldClearOnNextInput)
            {
                display.Text = "0";
                shouldClearOnNextInput = false;
                return;
            }

            if (display.Text.Length > 0)
            {
                display.Text = display.Text.Substring(0, display.Text.Length - 1);
            }

            if (string.IsNullOrEmpty(display.Text) || display.Text == "-")
            {
                display.Text = "0";
            }
        }

        public void ShowOperator(string operatorSymbol, double leftValue)
        {
            history.Text = $"{leftValue} {operatorSymbol}";
            shouldClearOnNextInput = true;
        }

        public void ShowResult(double result)
        {
            display.Text = result.ToString();
            shouldClearOnNextInput = true;
        }

        public void ShowEqualsResult(double leftValue, string operatorSymbol, double rightValue, double result)
        {
            string calculation = $"{leftValue} {operatorSymbol} {rightValue} = {result}";
            history.Text = $"{leftValue} {operatorSymbol} {rightValue} =";
            display.Text = result.ToString();

            historyList.Items.Add(calculation);
            historyList.TopIndex = historyList.Items.Count - 1;

            shouldClearOnNextInput = true;
        }

        public double GetCurrentValue()
        {
            string text = display.Text ?? string.Empty;

            if (double.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out double value))
            {
                return value;
            }
            return 0;
        }

        public void Clear()
        {
            display.Text = "0";
            history.Text = string.Empty;
            shouldClearOnNextInput = true;
        }

        public void ClearHistory()
        {
            historyList.Items.Clear();
        }
    }
}