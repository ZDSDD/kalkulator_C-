using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp12
{
    /// <summary>
    /// Handles 64-bit integer, multi-base, and bitwise operations
    /// </summary>
    public class ProgrammerCalculator
    {
        private long result = 0;
        private string currentOperator = "";
        private long leftOperand = 0;

        public long Result => result;
        public string CurrentOperator => currentOperator;
        public long LeftOperand => leftOperand;

        // Handles unary (single-number) operations
        public long PerformUnaryOperation(string operatorSymbol, long value)
        {
            switch (operatorSymbol)
            {
                case "~": // Bitwise NOT
                    return ~value;
                default:
                    return value;
            }
        }

        // Handles binary (two-number) operations
        public void Calculate(long value, string operatorSymbol)
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
                    if (value == 0)
                    {
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }
                    result /= value; // Integer division
                    break;
                case "&": // AND
                    result &= value;
                    break;
                case "|": // OR
                    result |= value;
                    break;
                case "^": // XOR
                    result ^= value;
                    break;
                case "<<": // Left Shift
                    // Shift operators require the right-hand side to be an 'int'
                    result <<= (int)value;
                    break;
                case ">>": // Right Shift
                    result >>= (int)value;
                    break;
                case "": // No previous operator, this is the first number
                    result = value;
                    break;
            }

            leftOperand = result;
            currentOperator = operatorSymbol;
        }

        public long CalculateFinal(long rightValue)
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