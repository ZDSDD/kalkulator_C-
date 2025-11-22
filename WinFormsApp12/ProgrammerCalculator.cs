using System;

namespace CalculatorApp
{


    public enum ProgrammerOperation
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide,
        And,
        Or,
        Xor,
        LeftShift,
        RightShift
    }


    public class ProgrammerCalculator : BaseCalculator<long, ProgrammerOperation>
    {
        protected override long ApplyOperation(long left, long right, ProgrammerOperation op)
        {
            return op switch
            {
                ProgrammerOperation.Add => left + right,
                ProgrammerOperation.Subtract => left - right,
                ProgrammerOperation.Multiply => left * right,
                ProgrammerOperation.Divide => right == 0 ? throw new DivideByZeroException() : left / right,
                ProgrammerOperation.And => left & right,
                ProgrammerOperation.Or => left | right,
                ProgrammerOperation.Xor => left ^ right,
                ProgrammerOperation.LeftShift => left << (int)right,
                ProgrammerOperation.RightShift => left >> (int)right,

                ProgrammerOperation.None => right,

                _ => left
            };
        }

        public long ApplyUnary(string op, long value)
        {
            return op switch
            {
                "~" => ~value,
                _ => value
            };
        }
    }
}
