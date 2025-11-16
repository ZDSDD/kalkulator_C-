namespace WinFormsApp12
{
    /// <summary>
    /// Handles calculator math operations
    /// </summary>
    public class Calculator
    {
        private double result = 0;
        private string currentOperator = "";
        private double leftOperand = 0;

        public double Result => result;
        public string CurrentOperator => currentOperator;
        public double LeftOperand => leftOperand;

        public void SetValue(double value)
        {
            result = value;
        }

        public void Calculate(double value, string operatorSymbol)
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
                    if (value != 0)
                    {
                        result /= value;
                    }
                    else
                    {
                        result = 0; // Reset or show error
                    }
                    break;
                case "":
                    result = value;
                    break;
            }

            // Store values for display
            leftOperand = result;
            currentOperator = operatorSymbol;
        }

        public double CalculateFinal(double rightValue)
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