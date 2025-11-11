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

        public void AppendToHistory(string text)
        {
            history.Text += text;
        }

        public void ShowResult(int result)
        {
            display.Text = result.ToString();
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
            display.Text = "";
            history.Text = "";
            shouldClearOnNextInput = false;
        }
    }
}