namespace WinFormsApp12
{
    /// <summary>
    /// Handles calculator math operations
    /// </summary>
    public class Calculator
    {
        private int result = 0;
        private string currentOperator = "";
        private int leftOperand = 0;

        public int Result => result;
        public string CurrentOperator => currentOperator;
        public int LeftOperand => leftOperand;

        public void SetValue(int value)
        {
            result = value;
        }

        public void Calculate(int value, string operatorSymbol)
        {
            // Do the previous operation
            switch (currentOperator)
            {
                case "+":
                    result += value;
                    break;
                case "-":
                    result -= value;
                    break;
                case "*":
                    result *= value;
                    break;
                case "/":
                    result = value != 0 ? result / value : 0;
                    break;
                case "":
                    result = value;
                    break;
            }

            // Store values for display
            leftOperand = result;
            currentOperator = operatorSymbol;
        }

        public int CalculateFinal(int rightValue)
        {
            Calculate(rightValue, "");
            return result;
        }

        public void Reset()
        {
            result = 0;
            currentOperator = "";
            leftOperand = 0;
        }
    }
}