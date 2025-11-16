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


        public static double CalculateSquare(double value)
        {
            return value * value;
        }

        public static double CalculateSqrt(double value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Invalid input"); // Sqrt of negative
            }
            return Math.Sqrt(value);
        }

        public static double CalculateOneDivX(double value)
        {
            if (value == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return 1 / value;
        }

        public static double CalculatePercentage(double value)
        {
            return value / 100;
        }

        public void Calculate(double value, string operatorSymbol)
        {
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
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }
                    break;
                case "":
                    result = value;
                    break;
            }

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