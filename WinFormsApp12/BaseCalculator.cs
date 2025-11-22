using System;

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
                // Complete the pending operation first
                result = ApplyOperation(leftOperand, currentValue, currentOperator);
                leftOperand = result;
            }
            else
            {
                // Start new calculation
                result = currentValue;
                leftOperand = currentValue;
                hasPendingOperator = true;
            }

            currentOperator = op;
            lastOperator = op;
            lastRightOperand = currentValue;
        }

        public T CalculateFinal(T rightValue)
        {
            if (hasPendingOperator)
            {
                // Complete pending calculation
                result = ApplyOperation(leftOperand, rightValue, currentOperator);
                leftOperand = result;
                hasPendingOperator = false;

                lastRightOperand = rightValue;
                lastOperator = currentOperator;
            }
            else
            {
                // Repeat last operation (when pressing = multiple times)
                if (lastOperator is not null)
                {
                    leftOperand = result;
                    result = ApplyOperation(result, lastRightOperand, lastOperator);
                }
            }

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