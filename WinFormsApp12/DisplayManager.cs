namespace WinFormsApp12
{
    /// <summary>
    /// Manages what shows on the screen
    /// </summary>
    public class DisplayManager
    {
        private readonly TextBox display;
        private readonly Label history;
        private bool shouldClearOnNextInput = false;
        private string currentExpression = "";

        public DisplayManager(TextBox display, Label history)
        {
            this.display = display;
            this.history = history;
        }

        public void AppendNumber(string number)
        {
            if (shouldClearOnNextInput)
            {
                display.Text = "";
                shouldClearOnNextInput = false;
            }
            display.Text += number;
        }

        public void ShowOperator(string operatorSymbol, int leftValue)
        {
            currentExpression = $"{leftValue} {operatorSymbol} ";
            history.Text = currentExpression;
            shouldClearOnNextInput = true;
        }

        public void ShowResult(int result)
        {
            display.Text = result.ToString();
            shouldClearOnNextInput = true;
        }

        public void ShowEqualsResult(int leftValue, string operatorSymbol, int rightValue, int result)
        {
            history.Text = $"{leftValue} {operatorSymbol} {rightValue} =";
            display.Text = result.ToString();
            currentExpression = "";
            shouldClearOnNextInput = true;
        }

        public int GetCurrentValue()
        {
            if (string.IsNullOrEmpty(display.Text))
                return 0;

            return int.TryParse(display.Text, out int value) ? value : 0;
        }

        public void Clear()
        {
            display.Text = "0";
            history.Text = "";
            currentExpression = "";
            shouldClearOnNextInput = false;
        }
    }
}