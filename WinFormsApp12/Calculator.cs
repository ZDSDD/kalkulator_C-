namespace WinFormsApp12
{

    public enum SpecialOperatorsSimple
    {
        SQUARE,
        SQRT,
        ONEDIVX,
        PERCENTAGE,
    }
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
        
        public void CalculateSpecial(SpecialOperatorsSimple op)
        {
            switch (op)
            {
                case SpecialOperatorsSimple.SQUARE:
                    result = result * result;
                    break;
                case SpecialOperatorsSimple.SQRT:
                    result = Math.Sqrt(result);
                    break;
                case SpecialOperatorsSimple.ONEDIVX:
                    if (result != 0)
                    {
                        result = 1 / result;
                    }
                    else
                    {
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }
                    break;
                case SpecialOperatorsSimple.PERCENTAGE:
                    result = result / 100;
                    break;
            }

            // Update left operand to reflect the value used for display after a special op
            leftOperand = result;
            // Clear current operator because a special op is immediate
            currentOperator = "";
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
                        throw new DivideByZeroException("Cannot divide by zero.");
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