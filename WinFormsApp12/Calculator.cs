namespace WinFormsApp12
{
    /// <summary>
    /// Handles calculator math operations
    /// </summary>
    public class Calculator
    {
        private int result = 0;
        private string currentOperator = "";

        public int Result => result;

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

            // Store the new operator for next time
            currentOperator = operatorSymbol;
        }

        public void Reset()
        {
            result = 0;
            currentOperator = "";
        }
    }
}