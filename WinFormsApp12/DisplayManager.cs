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
        private int ZeroCounter = 0;

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
                if(number == "0")
                {
                    // Prevent leading zeros
                    ZeroCounter++;
                    if (ZeroCounter > 10)
                    {
                        this.display.Text = "Stop hitting 0s!";
                    }
                        return;
                }
                else
                {
                    ZeroCounter = 0;
                }
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
            string calculation = $"{leftValue} {operatorSymbol} {rightValue} = {result}";
            history.Text = $"{leftValue} {operatorSymbol} {rightValue} =";
            display.Text = result.ToString();

            // Add to history list
            historyList.Items.Add(calculation);
            historyList.TopIndex = historyList.Items.Count - 1; // Auto-scroll to bottom

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
            shouldClearOnNextInput = true;
        }

        public void ClearHistory()
        {
            historyList.Items.Clear();
        }
    }
}