namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        private Calculator calculator;
        private DisplayManager display;
        private Button[] buttonNum = new Button[3];

        public Form1()
        {
            InitializeComponent();

            calculator = new Calculator();
            display = new DisplayManager(textBox1, label1);

            CreateDynamicButtons();
            display.Clear();
        }

        private void CreateDynamicButtons()
        {
            for (int i = 0; i < buttonNum.Length; i++)
            {
                buttonNum[i] = new Button
                {
                    Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238),
                    Location = new Point(10 + i * 57, 204),
                    Name = "button" + i,
                    Size = new Size(50, 50),
                    TabIndex = i,
                    Text = (i + 1).ToString(),
                    UseVisualStyleBackColor = true
                };

                buttonNum[i].Click += buttonNumeric_Click;
                Controls.Add(buttonNum[i]);
            }
        }

        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            string number = (sender as Button)?.Text ?? "";
            display.AppendNumber(number);
            display.AppendToHistory(number);
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            string operatorSymbol = (sender as Button)?.Text ?? "";

            int currentValue = display.GetCurrentValue();
            calculator.Calculate(currentValue, operatorSymbol);

            display.ShowResult(calculator.Result);
            display.AppendToHistory($" {operatorSymbol} ");
        }
    }
}