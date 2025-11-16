namespace WinFormsApp12
{
    /// <summary>
    /// Manages what shows on the screen
    /// </summary>
    public class DisplayManager
    {
        private readonly TextBox display;
        private readonly Label history;
        private readonly ListBox historyList;
        private bool shouldClearOnNextInput = true;

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

        public void AppendNumber(string number)
        {
            PrepareForInput(); // Replaces duplicated code

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