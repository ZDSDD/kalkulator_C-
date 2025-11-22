namespace WinFormsApp12
{
    public enum StandardOperation
    {
        None,
        Add,
        Subtract,
        Multiply,
        Divide
    }
    public class Calculator : BaseCalculator<double, StandardOperation>
    {
        protected override double ApplyOperation(double left, double right, StandardOperation op)
        {
            return op switch
            {
                StandardOperation.Add => left + right,
                StandardOperation.Subtract => left - right,
                StandardOperation.Multiply => left * right,
                StandardOperation.Divide => right == 0 ? throw new DivideByZeroException() : left / right,
                StandardOperation.None => right,
                _ => right
            };
        }

        public static double CalculateSquare(double v) => v * v;
        public static double CalculateSqrt(double v) => v < 0 ? throw new ArgumentException() : Math.Sqrt(v);
        public static double CalculateOneDivX(double v) => v == 0 ? throw new DivideByZeroException() : 1 / v;
        public static double CalculatePercentage(double v) => v / 100;
    }


}