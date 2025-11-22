using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp12
{
    public abstract class BaseCalculator<T, TOp>
    {
        protected T result;
        protected TOp currentOperator;
        protected T leftOperand;

        protected TOp lastOperator;
        protected T lastRightOperand;

        protected bool hasPendingOperator = false;

        public T Result => result;
        public TOp CurrentOperator => currentOperator;
        public T LeftOperand => leftOperand;

        protected abstract T ApplyOperation(T left, T right, TOp op);

        public void ApplyOperator(T currentValue, TOp op)
        {
            if (hasPendingOperator)
            {
                result = ApplyOperation(result, currentValue, currentOperator);
            }
            else
            {
                result = currentValue;
                hasPendingOperator = true;
            }

            leftOperand = result;
            currentOperator = op;

            lastOperator = op;
            lastRightOperand = currentValue;
        }

        public T CalculateFinal(T rightValue)
        {
            if (!hasPendingOperator)
            {
                if (lastOperator is not null)
                {
                    result = ApplyOperation(result, lastRightOperand, lastOperator);
                }
                return result;
            }

            result = ApplyOperation(result, rightValue, currentOperator);
            leftOperand = result;

            hasPendingOperator = false;

            lastRightOperand = rightValue;
            lastOperator = currentOperator;

            return result;
        }

        public void Reset()
        {
            result = default;
            currentOperator = default;
            leftOperand = default;
            lastOperator = default;
            lastRightOperand = default;
            hasPendingOperator = false;
        }
    }
}
