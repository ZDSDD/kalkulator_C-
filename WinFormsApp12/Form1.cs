using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        private Calculator calculator;
        private DisplayManager display;
        private TextBox textBox1;
        private Label label1;
        private ListBox historyListBox;

        private ProgrammerCalculator progCalculator;
        private CalculatorMode currentMode = CalculatorMode.Standard;
        private NumberBase currentBase = NumberBase.Decimal;

        private List<Button> standardOnlyButtons = new List<Button>();
        private List<Button> programmerOnlyButtons = new List<Button>();
        private List<Button> hexButtons = new List<Button>();
        private List<Button> numericButtons = new List<Button>();

        private Button btnMode;
        private List<Button> baseButtons = new List<Button>();
        private Color baseButtonDefaultBackColor;

        public Form1()
        {
            InitializeComponent();

            calculator = new Calculator();
            progCalculator = new ProgrammerCalculator();
            SetupScalableLayout();
            display.Clear();

            UpdateUIVisibility();
        }

        #region UI Setup
        private void SetupScalableLayout()
        {
            Controls.Clear();

            TableLayoutPanel mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10)
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            TableLayoutPanel calculatorLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1
            };

            // REFINEMENT: Changed from Absolute to Percent/AutoSize
            // This allows the display to scale with the form, and the
            // button grid to occupy the majority of the space.
            // The AutoSize row fits the programmer toolbar perfectly.
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10)); // label1
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15)); // textBox1
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));    // topButtonsPanel
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75)); // buttonGrid

            label1 = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10)
            };

            textBox1 = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 16, FontStyle.Bold), // Made bold for emphasis
                TextAlign = HorizontalAlignment.Right,
                ReadOnly = true
            };

            FlowLayoutPanel topButtonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                // REFINEMENT: Set a minimum height, but AutoSize will handle expansion
                MinimumSize = new Size(0, 35)
            };

            btnMode = CreateButton("Standard", buttonMode_Click);

            // REFINEMENT: Standardized toolbar button font and size
            btnMode.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnMode.Width = 100;
            btnMode.Height = 35;
            topButtonsPanel.Controls.Add(btnMode);

            Button btnHex = CreateButton("HEX", buttonBase_Click);
            Button btnDec = CreateButton("DEC", buttonBase_Click);
            Button btnOct = CreateButton("OCT", buttonBase_Click);
            Button btnBin = CreateButton("BIN", buttonBase_Click);

            baseButtons.AddRange(new[] { btnHex, btnDec, btnOct, btnBin });

            foreach (var b in baseButtons)
            {
                // REFINEMENT: Standardized toolbar button font and size
                b.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                b.Width = 60;
                b.Height = 35;
                topButtonsPanel.Controls.Add(b);
                programmerOnlyButtons.Add(b);
            }

            baseButtonDefaultBackColor = baseButtons.Count > 0 ? baseButtons[0].BackColor : SystemColors.Control;

            string[] progOps = new[] { "AND", "OR", "XOR", "Lsh", "Rsh" };
            foreach (var op in progOps)
            {
                Button b = CreateButton(op, buttonProgrammerOperator_Click);
                // REFINEMENT: Standardized toolbar button font and size
                b.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                b.Width = 60;
                b.Height = 35;
                topButtonsPanel.Controls.Add(b);
                programmerOnlyButtons.Add(b);
            }

            Button btnNot = CreateButton("NOT", buttonProgrammerUnary_Click);
            // REFINEMENT: Standardized toolbar button font and size
            btnNot.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnNot.Width = 60;
            btnNot.Height = 35;
            topButtonsPanel.Controls.Add(btnNot);
            programmerOnlyButtons.Add(btnNot);

            // REFINEMENT: Removed the separate, empty 'hexPanel'.
            // The A-F buttons were already being added to 'topButtonsPanel' correctly.
            // The 'hexPanel' itself was being added as an empty control.

            foreach (var ch in new[] { "A", "B", "C", "D", "E", "F" })
            {
                Button hb = CreateButton(ch, buttonNumeric_Click);
                // REFINEMENT: Standardized toolbar button font and size
                hb.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                hb.Width = 35; // Made smaller
                hb.Height = 35;
                topButtonsPanel.Controls.Add(hb);
                programmerOnlyButtons.Add(hb);
                hexButtons.Add(hb);
            }

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

            display = new DisplayManager(textBox1, label1, historyListBox);

            TableLayoutPanel buttonGrid = CreateButtonGrid();

            calculatorLayout.Controls.Add(label1, 0, 0);
            calculatorLayout.Controls.Add(textBox1, 0, 1);
            calculatorLayout.Controls.Add(topButtonsPanel, 0, 2);
            calculatorLayout.Controls.Add(buttonGrid, 0, 3);

            mainContainer.Controls.Add(calculatorLayout, 0, 0);
            mainContainer.Controls.Add(historyPanel, 1, 0);
            Controls.Add(mainContainer);

            // Set a reasonable minimum size for the form
            this.MinimumSize = new Size(600, 700);
        }

        private TableLayoutPanel CreateButtonGrid()
        {
            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 6,
                ColumnCount = 4
            };

            // REFINEMENT: Use 100f / RowCount for clarity
            for (int i = 0; i < grid.RowCount; i++)
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / grid.RowCount));

            // REFINEMENT: Use 100f / ColumnCount for clarity
            for (int i = 0; i < grid.ColumnCount; i++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / grid.ColumnCount));

            Button btnOneDiv = AddButton(grid, "1/x", 0, 0, buttonOneDivX_Click);
            Button btnSquare = AddButton(grid, "x^2", 0, 1, buttonSquare_Click);
            Button btnSqrt = AddButton(grid, "sqrt", 0, 2, buttonSqrt_Click);
            Button btnPercent = AddButton(grid, "%", 0, 3, buttonPercentage_Click);

            standardOnlyButtons.AddRange(new[] { btnOneDiv, btnSquare, btnSqrt, btnPercent });

            Button btn7 = AddButton(grid, "7", 1, 0, buttonNumeric_Click);
            Button btn8 = AddButton(grid, "8", 1, 1, buttonNumeric_Click);
            Button btn9 = AddButton(grid, "9", 1, 2, buttonNumeric_Click);
            AddButton(grid, "/", 1, 3, buttonOperator_Click);

            Button btn4 = AddButton(grid, "4", 2, 0, buttonNumeric_Click);
            Button btn5 = AddButton(grid, "5", 2, 1, buttonNumeric_Click);
            Button btn6 = AddButton(grid, "6", 2, 2, buttonNumeric_Click);
            AddButton(grid, "*", 2, 3, buttonOperator_Click);

            Button btn1 = AddButton(grid, "1", 3, 0, buttonNumeric_Click);
            Button btn2 = AddButton(grid, "2", 3, 1, buttonNumeric_Click);
            Button btn3 = AddButton(grid, "3", 3, 2, buttonNumeric_Click);
            AddButton(grid, "-", 3, 3, buttonOperator_Click);

            Button btnSign = AddButton(grid, "+/-", 4, 0, buttonSign_Click);
            Button btn0 = AddButton(grid, "0", 4, 1, buttonNumeric_Click);
            Button btnDecimal = AddButton(grid, ".", 4, 2, buttonDecimal_Click);
            AddButton(grid, "+", 4, 3, buttonOperator_Click);

            Button btnClear = AddButton(grid, "C", 5, 0, buttonClear_Click);

            Button equalsBtn = CreateButton("=", buttonEquals_Click);
            grid.Controls.Add(equalsBtn, 1, 5);
            grid.SetColumnSpan(equalsBtn, 3);
            equalsBtn.Dock = DockStyle.Fill; // Ensure equals button fills its spanned cells

            numericButtons.AddRange(new[] { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 });

            // REFINEMENT: Removed 'btnClear' from this list.
            // The Clear button should be visible in both modes.
            standardOnlyButtons.AddRange(new[] { btnSign, btnDecimal });

            return grid;
        }

        private Button AddButton(TableLayoutPanel grid, string text, int row, int col, EventHandler onClick)
        {
            Button btn = CreateButton(text, onClick);
            grid.Controls.Add(btn, col, row);
            btn.Dock = DockStyle.Fill;
            return btn;
        }

        private Button CreateButton(string text, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                UseVisualStyleBackColor = true,
                // REFINEMENT: Set FlatStyle for a more modern look
                FlatStyle = FlatStyle.Flat
            };
            // REFINEMENT: Add some basic hover/click effects
            btn.FlatAppearance.BorderColor = Color.DarkGray;
            btn.FlatAppearance.MouseOverBackColor = Color.LightGray;
            btn.FlatAppearance.MouseDownBackColor = Color.DarkGray;

            btn.Click += onClick;
            return btn;
        }
        #endregion

        // ... [Rest of your event handlers, no changes needed there] ...
        #region Standard calculations
        private void buttonPercentage_Click(object sender, EventArgs e)
        {
            double current = display.GetCurrentValue();
            double newValue = Calculator.CalculatePercentage(current);
            display.ShowResult(newValue);
        }

        private void buttonSqrt_Click(object sender, EventArgs e)
        {
            try
            {
                double current = display.GetCurrentValue();
                double newValue = Calculator.CalculateSqrt(current);
                display.ShowResult(newValue);
            }
            catch (ArgumentException ex)
            {
                display.ShowError(ex.Message);
                calculator.Reset();
            }
        }

        private void buttonSquare_Click(object sender, EventArgs e)
        {
            try
            {
                double current = display.GetCurrentValue();
                double newValue = Calculator.CalculateSquare(current);
                display.ShowResult(newValue);
            }
            catch (ArgumentException ex)
            {
                display.ShowError(ex.Message);
                calculator.Reset();
            }
        }

        private void buttonOneDivX_Click(object sender, EventArgs e)
        {
            try
            {
                double current = display.GetCurrentValue();
                double newValue = Calculator.CalculateOneDivX(current);
                display.ShowResult(newValue);
            }
            catch (DivideByZeroException)
            {
                display.ShowError("Cannot divide by zero");
                calculator.Reset();
            }
        }
        #endregion

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Programmer)
            {
                // Note: This currently only routes operators defined in the prog list
                // (AND, OR, etc.). You may want to add logic for +, -, *, /
                // in programmer mode if desired.
                buttonProgrammerOperator_Click(sender, e);
                return;
            }

            try
            {
                string operatorSymbol = (sender as Button)?.Text ?? "";
                double currentValue = display.GetCurrentValue();
                calculator.Calculate(currentValue, operatorSymbol);
                display.ShowOperator(operatorSymbol, calculator.LeftOperand);
                display.ShowResult(calculator.Result);
            }
            catch (DivideByZeroException)
            {
                display.ShowError("Cannot divide by zero");
                calculator.Reset();
            }
        }

        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            string number = (sender as Button)?.Text ?? "";
            display.AppendNumber(number);
        }

        private void buttonSign_Click(object sender, EventArgs e)
        {
            display.ToggleSign();
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            display.AppendDecimal();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            calculator.Reset();
            progCalculator.Reset();
            display.Clear();
            display.ClearHistory();
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Standard)
            {
                double rightValue = display.GetCurrentValue();
                double leftValue = calculator.LeftOperand;
                string op = calculator.CurrentOperator;
                double result = calculator.CalculateFinal(rightValue);
                display.ShowEqualsResult(leftValue, op, rightValue, result);
            }
            else
            {
                long rightValue = display.GetCurrentInteger();
                long leftValue = progCalculator.LeftOperand;
                string op = progCalculator.CurrentOperator;
                long result = progCalculator.CalculateFinal(rightValue);
                display.ShowEqualsIntegerResult(leftValue, op, rightValue, result);
            }
        }

        private void buttonMode_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Standard)
            {
                currentMode = CalculatorMode.Programmer;
                currentBase = NumberBase.Decimal;
            }
            else
            {
                currentMode = CalculatorMode.Standard;
                currentBase = NumberBase.Decimal;
            }

            calculator.Reset();
            progCalculator.Reset();
            display.Clear();
            display.CurrentBase = currentBase;

            UpdateUIVisibility();
        }

        #region Programmer handlers
        private void buttonBase_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Standard) return;

            string baseText = (sender as Button)?.Text ?? "DEC";

            long currentValue = display.GetCurrentInteger();

            switch (baseText)
            {
                case "HEX": currentBase = NumberBase.Hexadecimal; break;
                case "DEC": currentBase = NumberBase.Decimal; break;
                case "OCT": currentBase = NumberBase.Octal; break;
                case "BIN": currentBase = NumberBase.Binary; break;
            }

            display.CurrentBase = currentBase;
            display.ShowIntegerResult(currentValue);

            UpdateNumericButtonEnabledState();
            HighlightSelectedBase();
        }

        private void buttonProgrammerOperator_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Standard) return;

            try
            {
                string operatorSymbol = (sender as Button)?.Text ?? "";

                if (operatorSymbol == "Lsh") operatorSymbol = "<<";
                if (operatorSymbol == "Rsh") operatorSymbol = ">>";

                if (operatorSymbol == "AND") operatorSymbol = "&";
                if (operatorSymbol == "OR") operatorSymbol = "|";
                if (operatorSymbol == "XOR") operatorSymbol = "^";

                // REFINEMENT: Added standard operators for programmer mode
                // This assumes your ProgrammerCalculator can handle them.
                if (new[] { "+", "-", "*", "/" }.Contains(operatorSymbol))
                {
                    // Use the symbol directly
                }
                else if (!new[] { "<<", ">>", "&", "|", "^" }.Contains(operatorSymbol))
                {
                    // If it's not a recognized prog operator, do nothing
                    return;
                }

                long currentValue = display.GetCurrentInteger();
                progCalculator.Calculate(currentValue, operatorSymbol);
                display.ShowIntegerResult(progCalculator.Result);
            }
            catch (DivideByZeroException)
            {
                display.ShowError("Cannot divide by zero");
                progCalculator.Reset();
            }
        }

        private void buttonProgrammerUnary_Click(object sender, EventArgs e)
        {
            if (currentMode == CalculatorMode.Standard) return;

            string op = (sender as Button)?.Text ?? "";
            if (op == "NOT") op = "~";

            long currentValue = display.GetCurrentInteger();
            long result = progCalculator.PerformUnaryOperation(op, currentValue);
            display.ShowIntegerResult(result);
        }
        #endregion

        private void UpdateUIVisibility()
        {
            bool isProgMode = (currentMode == CalculatorMode.Programmer);
            btnMode.Visible = true;
            foreach (var btn in standardOnlyButtons)
            {
                btn.Visible = !isProgMode;
            }
            foreach (var btn in programmerOnlyButtons)
            {
                btn.Visible = isProgMode;
            }

            btnMode.Text = isProgMode ? "Programmer" : "Standard";

            UpdateNumericButtonEnabledState();
            HighlightSelectedBase();
        }

        private void UpdateNumericButtonEnabledState()
        {
            if (currentMode == CalculatorMode.Standard)
            {
                foreach (var b in numericButtons) b.Enabled = true;
                foreach (var b in hexButtons) b.Enabled = false; // A-F buttons disabled in Standard
                return;
            }

            bool isHex = (currentBase == NumberBase.Hexadecimal);
            bool isDec = (currentBase == NumberBase.Decimal);
            bool isOct = (currentBase == NumberBase.Octal);
            bool isBin = (currentBase == NumberBase.Binary);

            foreach (var btn in hexButtons)
            {
                btn.Enabled = isHex;
            }

            foreach (var btn in numericButtons)
            {
                string txt = btn.Text;
                if (int.TryParse(txt, out int digit))
                {
                    bool allowed = false;
                    if (isHex) allowed = true;
                    else if (isDec) allowed = digit >= 0 && digit <= 9;
                    else if (isOct) allowed = digit >= 0 && digit <= 7;
                    else if (isBin) allowed = (digit == 0 || digit == 1);

                    btn.Enabled = allowed;
                }
            }

            // REFINEMENT: Find the decimal button by text to disable it in prog mode
            foreach (var btn in standardOnlyButtons)
            {
                if (btn.Text == ".") btn.Enabled = false; // No decimals in prog mode
                if (btn.Text == "+/-") btn.Enabled = false; // No sign toggle in prog mode
            }
        }

        private void HighlightSelectedBase()
        {
            foreach (var b in baseButtons)
            {
                b.BackColor = baseButtonDefaultBackColor;
                // REFINEMENT: Ensure base buttons use flat style
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = Color.DarkGray;
            }

            if (currentMode != CalculatorMode.Programmer) return;

            string selected = currentBase switch
            {
                NumberBase.Hexadecimal => "HEX",
                NumberBase.Decimal => "DEC",
                NumberBase.Octal => "OCT",
                NumberBase.Binary => "BIN",
                _ => "DEC"
            };

            foreach (var b in baseButtons)
            {
                if (b.Text == selected)
                {
                    // REFINEMENT: Use a more noticeable highlight
                    b.BackColor = Color.CornflowerBlue;
                    b.FlatAppearance.BorderColor = Color.Black;
                    break;
                }
            }
        }
    }

}