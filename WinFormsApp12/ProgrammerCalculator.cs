using System;

namespace WinFormsApp12
{
    /// <summary>
    /// Handles64-bit integer, multi-base, and bitwise operations
    /// </summary>
    public class ProgrammerCalculator
    {
        private long result =0;
        private string currentOperator = "";
        private long leftOperand =0;

        private long lastRightOperand =0;
        private string lastOperator = "";
        private bool isNewChain = true;

        public long Result => result;
        public string CurrentOperator => currentOperator;
        public long LeftOperand => leftOperand;

        public long PerformUnaryOperation(string operatorSymbol, long value)
        {
            switch (operatorSymbol)
            {
                case "~":
                    return ~value;
                default:
                    return value;
            }
        }

        private void PerformCalculation(long rightValue)
        {
            switch (currentOperator)
            {
                case "+":
                    result += rightValue;
                    break;
                case "-":
                    result -= rightValue;
                    break;
                case "*":
                    result *= rightValue;
                    break;
                case "/":
                    if (rightValue ==0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }
                    result /= rightValue;
                    break;
                case "&":
                    result &= rightValue;
                    break;
                case "|":
                    result |= rightValue;
                    break;
                case "^":
                    result ^= rightValue;
                    break;
                case "<<":
                    result <<= (int)rightValue;
                    break;
                case ">>":
                    result >>= (int)rightValue;
                    break;
                case "":
                    result = rightValue;
                    break;
            }

            lastRightOperand = rightValue;
            lastOperator = currentOperator;
            leftOperand = result;
        }

        public void SetOperator(string operatorSymbol, long currentValue)
        {
            if (!isNewChain)
            {
                PerformCalculation(currentValue);
            }
            else
            {
                result = currentValue;
                leftOperand = currentValue;
                isNewChain = false;
            }

            currentOperator = operatorSymbol;
        }

        public long CalculateFinal(long rightValue)
        {
            if (isNewChain)
            {
                if (!string.IsNullOrEmpty(lastOperator))
                {
                    currentOperator = lastOperator;
                    PerformCalculation(lastRightOperand);
                }
                else
                {
                    result = rightValue;
                }
            }
            else
            {
                PerformCalculation(rightValue);
                isNewChain = true;
            }

            return result;
        }

        public void Reset()
        {
            result =0;
            currentOperator = "";
            leftOperand =0;
            lastRightOperand =0;
            lastOperator = "";
            isNewChain = true;
        }
    }
}