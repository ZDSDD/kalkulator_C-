using System.Numerics;

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
        private NumberBase currentBase = NumberBase.Decimal;
        public NumberBase CurrentBase
        {
            set { this.currentBase = value; }
        }
        public DisplayManager(TextBox display, Label history, ListBox historyList)
        {
            this.display = display;
            this.history = history;
            this.historyList = historyList;
        }

        public void ShowError(string message)
        {
            display.Text = message;
            history.Text = ""; // Clear pending operations
            shouldClearOnNextInput = true; // Ready for new input
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
            display.Text = ConvertToString(result, currentBase);
            shouldClearOnNextInput = true;
        }
        public void ShowEqualsIntegerResult(long leftValue, string operatorSymbol, long rightValue, long result)
        {
            // Format all parts of the calculation in the current base
            string leftStr = ConvertToString(leftValue, currentBase);
            string rightStr = ConvertToString(rightValue, currentBase);
            string resultStr = ConvertToString(result, currentBase);

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
                // Parse the string using the correct base
                switch (currentBase)
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
            catch (Exception)
            {
                // Handle invalid format (e.g., "FF" in Decimal mode)
                return 0;
            }
        }
        public void AppendNumber(string number)
        {
            // TODO: You can add base-validation here
            // e.g., if (currentBase == NumberBase.Binary && number != "0" && number != "1") return;
            // e.g., if (currentBase == NumberBase.Octal && int.Parse(number) > 7) return;

            PrepareForInput();  

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
            // Use culture-specific decimal separator
            string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            // If starting fresh, handle sign preservation and add leading zero with decimal
            if (shouldClearOnNextInput)
            {
                // Preserve a leading negative sign if the display currently contains it
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

            // If there's only a sign (e.g. user toggled sign), start with -0<sep>
            if (display.Text == "-")
            {
                display.Text = "-0" + sep;
                return;
            }

            // Append decimal separator if not already present
            if (!display.Text.Contains(sep))
            {
                display.Text += sep;
            }
        }

        public void ToggleSign()
        {
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
            historyList.TopIndex = historyList.Items.Count -1;

            shouldClearOnNextInput = true;
        }

        public double GetCurrentValue()
        {
            // Normalize possible decimal separators so parsing succeeds regardless of whether text contains '.' or ',':
            string text = display.Text ?? string.Empty;
            var nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string sep = nfi.NumberDecimalSeparator;

            if (sep == ",")
            {
                // Replace any '.' with ',':
                text = text.Replace('.', ',');
            }
            else
            {
                // Replace any ',' with '.':
                text = text.Replace(',', '.');
            }

            if (double.TryParse(text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out double value))
            {
                return value;
            }
            return 0;
        }

        public void Clear()
        {
            display.Text = "0";
            history.Text = "";
            shouldClearOnNextInput = true;
        }

        public void ClearHistory()
        {
            historyList.Items.Clear();
        }
    }
}