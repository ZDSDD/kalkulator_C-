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
        private string currentExpression = "";

        public DisplayManager(TextBox display, Label history, ListBox historyList)
        {
            this.display = display;
            this.history = history;
            this.historyList = historyList;
        }

        public void AppendNumber(string number)
        {
            if (shouldClearOnNextInput)
            {
                display.Text = "0";
                shouldClearOnNextInput = false;
            }

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
            if (shouldClearOnNextInput)
            {
                display.Text = "0";
                shouldClearOnNextInput = false;
            }

            if (!display.Text.Contains("."))
            {
                display.Text += ".";
            }
        }

        public void ToggleSign()
        {
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
            currentExpression = $"{leftValue} {operatorSymbol} ";
            history.Text = currentExpression;
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

            // Add to history list
            historyList.Items.Add(calculation);
            historyList.TopIndex = historyList.Items.Count - 1; // Auto-scroll to bottom

            currentExpression = "";
            shouldClearOnNextInput = true;
        }

        public double GetCurrentValue()
        {
            if (string.IsNullOrEmpty(display.Text))
                return 0;

            return double.TryParse(display.Text, out double value) ? value : 0;
        }

        public void Clear()
        {
            display.Text = "0";
            history.Text = "";
            currentExpression = "";
            shouldClearOnNextInput = true;
        }

        public void ClearHistory()
        {
            historyList.Items.Clear();
        }
    }
}