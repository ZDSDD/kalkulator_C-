namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        private Calculator calculator;
        private DisplayManager display;
        private TextBox textBox1;
        private Label label1;
        private ListBox historyListBox;

        public Form1()
        {
            InitializeComponent();

            calculator = new Calculator();

            SetupScalableLayout();
            display.Clear();
        }

        private void SetupScalableLayout()
        {
            // Clear any existing controls except our display
            Controls.Clear();

            // Main horizontal split: calculator on left, history on right
            TableLayoutPanel mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10)
            };

            // Left side (calculator) takes 70%, right side (history) takes 30%
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Calculator panel (left side)
            TableLayoutPanel calculatorLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };

            // Row sizes: history label, display textbox, buttons grid
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // History label
            label1 = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };

            // Display textbox
            textBox1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 16),
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };

            // History panel (right side)
            Panel historyPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            Label historyTitle = new Label
            {
                Text = "History",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            historyListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            historyPanel.Controls.Add(historyListBox);
            historyPanel.Controls.Add(historyTitle);

            // Create display manager with all controls including history list
            display = new DisplayManager(textBox1, label1, historyListBox);

            // Button grid
            TableLayoutPanel buttonGrid = CreateButtonGrid();

            calculatorLayout.Controls.Add(label1, 0, 0);
            calculatorLayout.Controls.Add(textBox1, 0, 1);
            calculatorLayout.Controls.Add(buttonGrid, 0, 2);

            mainContainer.Controls.Add(calculatorLayout, 0, 0);
            mainContainer.Controls.Add(historyPanel, 1, 0);

            Controls.Add(mainContainer);
        }

        private TableLayoutPanel CreateButtonGrid()
        {
            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 5,
                ColumnCount = 4
            };

            // Equal sizing for all rows and columns
            for (int i = 0; i < 5; i++)
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            for (int i = 0; i < 4; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));

            // Number buttons 7-9
            AddButton(grid, "7", 0, 0, buttonNumeric_Click);
            AddButton(grid, "8", 0, 1, buttonNumeric_Click);
            AddButton(grid, "9", 0, 2, buttonNumeric_Click);
            AddButton(grid, "/", 0, 3, buttonOperator_Click);

            // Number buttons 4-6
            AddButton(grid, "4", 1, 0, buttonNumeric_Click);
            AddButton(grid, "5", 1, 1, buttonNumeric_Click);
            AddButton(grid, "6", 1, 2, buttonNumeric_Click);
            AddButton(grid, "*", 1, 3, buttonOperator_Click);

            // Number buttons 1-3
            AddButton(grid, "1", 2, 0, buttonNumeric_Click);
            AddButton(grid, "2", 2, 1, buttonNumeric_Click);
            AddButton(grid, "3", 2, 2, buttonNumeric_Click);
            AddButton(grid, "-", 2, 3, buttonOperator_Click);

            // Bottom row
            AddButton(grid, "0", 3, 1, buttonNumeric_Click);
            AddButton(grid, "+", 3, 3, buttonOperator_Click);

            // Clear button
            AddButton(grid, "C", 4, 0, buttonClear_Click);

            // Equals button (spans 3 columns)
            Button equalsBtn = CreateButton("=", buttonEquals_Click);
            grid.Controls.Add(equalsBtn, 1, 4);
            grid.SetColumnSpan(equalsBtn, 3);

            return grid;
        }

        private void AddButton(TableLayoutPanel grid, string text, int row, int col, EventHandler onClick)
        {
            Button btn = CreateButton(text, onClick);
            grid.Controls.Add(btn, col, row);
        }

        private Button CreateButton(string text, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                UseVisualStyleBackColor = true
            };
            btn.Click += onClick;
            return btn;
        }

        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            string number = (sender as Button)?.Text ?? "";
            display.AppendNumber(number);
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            string operatorSymbol = (sender as Button)?.Text ?? "";

            int currentValue = display.GetCurrentValue();
            calculator.Calculate(currentValue, operatorSymbol);

            display.ShowOperator(operatorSymbol, calculator.LeftOperand);
            display.ShowResult(calculator.Result);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            calculator.Reset();
            display.Clear();
            display.ClearHistory();
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            int rightValue = display.GetCurrentValue();
            int leftValue = calculator.LeftOperand;
            string op = calculator.CurrentOperator;

            int result = calculator.CalculateFinal(rightValue);
            display.ShowEqualsResult(leftValue, op, rightValue, result);
        }
    }
}