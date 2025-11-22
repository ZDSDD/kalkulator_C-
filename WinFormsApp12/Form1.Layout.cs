using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    // Partial class allows us to split the code into logic (Form1.cs) and UI (Form1.Layout.cs)
    public partial class Form1
    {
        // UI Controls
        private TextBox textBox1;
        private Label label1;
        private ListBox historyListBox;
        private Button btnMode;

        // Helper lists for managing UI state
        private List<Button> standardOnlyButtons = new List<Button>();
        private List<Button> programmerOnlyButtons = new List<Button>();
        private List<Button> hexButtons = new List<Button>();
        private List<Button> numericButtons = new List<Button>();
        private List<Button> baseButtons = new List<Button>();

        private Color baseButtonDefaultBackColor;

        private void SetupScalableLayout()
        {
            Controls.Clear();

            // Main Container
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

            // Calculator Section
            TableLayoutPanel calculatorLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1
            };
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            calculatorLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75));

            // Output Displays
            label1 = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, Font = new Font("Segoe UI", 10) };
            textBox1 = new TextBox { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 16, FontStyle.Bold), TextAlign = HorizontalAlignment.Right, ReadOnly = true };

            // Top Buttons (Mode, Base, Prog Ops)
            FlowLayoutPanel topButtonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                MinimumSize = new Size(0, 35)
            };

            btnMode = CreateButton("Standard", buttonMode_Click);
            btnMode.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnMode.Size = new Size(100, 35);
            topButtonsPanel.Controls.Add(btnMode);

            SetupBaseButtons(topButtonsPanel);
            SetupProgrammerButtons(topButtonsPanel);

            // History Panel
            Panel historyPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(5) };
            Label historyTitle = new Label { Text = "History", Dock = DockStyle.Top, Font = new Font("Segoe UI", 12, FontStyle.Bold), Height = 30, TextAlign = ContentAlignment.MiddleLeft };
            historyListBox = new ListBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 10), BorderStyle = BorderStyle.FixedSingle };
            historyPanel.Controls.Add(historyListBox);
            historyPanel.Controls.Add(historyTitle);

            // Initialize Display Manager here or in constructor (Logic file handles instantiation)
            // Grid
            TableLayoutPanel buttonGrid = CreateButtonGrid();

            // Assembly
            calculatorLayout.Controls.Add(label1, 0, 0);
            calculatorLayout.Controls.Add(textBox1, 0, 1);
            calculatorLayout.Controls.Add(topButtonsPanel, 0, 2);
            calculatorLayout.Controls.Add(buttonGrid, 0, 3);

            mainContainer.Controls.Add(calculatorLayout, 0, 0);
            mainContainer.Controls.Add(historyPanel, 1, 0);
            Controls.Add(mainContainer);

            this.MinimumSize = new Size(600, 700);
        }

        private void SetupBaseButtons(FlowLayoutPanel panel)
        {
            string[] bases = { "HEX", "DEC", "OCT", "BIN" };
            foreach (var bName in bases)
            {
                Button b = CreateButton(bName, buttonBase_Click);
                b.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                b.Size = new Size(60, 35);
                panel.Controls.Add(b);
                baseButtons.Add(b);
                programmerOnlyButtons.Add(b);
            }
            baseButtonDefaultBackColor = baseButtons.Count > 0 ? baseButtons[0].BackColor : SystemColors.Control;
        }

        private void SetupProgrammerButtons(FlowLayoutPanel panel)
        {
            string[] progOps = { "AND", "OR", "XOR", "Lsh", "Rsh" };
            foreach (var op in progOps)
            {
                Button b = CreateButton(op, buttonOperator_Click); // Map directly to operator click
                b.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                b.Size = new Size(60, 35);
                panel.Controls.Add(b);
                programmerOnlyButtons.Add(b);
            }

            Button btnNot = CreateButton("NOT", buttonUnary_Click);
            btnNot.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            btnNot.Size = new Size(60, 35);
            panel.Controls.Add(btnNot);
            programmerOnlyButtons.Add(btnNot);

            string[] hexChars = { "A", "B", "C", "D", "E", "F" };
            foreach (var ch in hexChars)
            {
                Button hb = CreateButton(ch, buttonNumeric_Click);
                hb.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                hb.Size = new Size(35, 35);
                panel.Controls.Add(hb);
                programmerOnlyButtons.Add(hb);
                hexButtons.Add(hb);
            }
        }

        private TableLayoutPanel CreateButtonGrid()
        {
            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 6,
                ColumnCount = 4
            };

            for (int i = 0; i < grid.RowCount; i++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / grid.RowCount));
            for (int i = 0; i < grid.ColumnCount; i++) grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / grid.ColumnCount));

            // Standard Helpers
            standardOnlyButtons.Add(AddButton(grid, "1/x", 0, 0, buttonUnary_Click));
            standardOnlyButtons.Add(AddButton(grid, "x^2", 0, 1, buttonUnary_Click));
            standardOnlyButtons.Add(AddButton(grid, "sqrt", 0, 2, buttonUnary_Click));
            standardOnlyButtons.Add(AddButton(grid, "%", 0, 3, buttonUnary_Click));

            // Numbers and Ops
            AddButton(grid, "/", 1, 3, buttonOperator_Click);
            AddButton(grid, "*", 2, 3, buttonOperator_Click);
            AddButton(grid, "-", 3, 3, buttonOperator_Click);
            AddButton(grid, "+", 4, 3, buttonOperator_Click);

            string[] nums = { "7", "8", "9", "4", "5", "6", "1", "2", "3", "0" };
            int[] rows = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4 };
            int[] cols = { 0, 1, 2, 0, 1, 2, 0, 1, 2, 1 };

            for (int i = 0; i < nums.Length; i++)
            {
                Button b = AddButton(grid, nums[i], rows[i], cols[i], buttonNumeric_Click);
                numericButtons.Add(b);
            }

            // Bottom row specials
            Button btnSign = AddButton(grid, "+/-", 4, 0, buttonSign_Click);
            Button btnDecimal = AddButton(grid, ".", 4, 2, buttonDecimal_Click);
            standardOnlyButtons.Add(btnSign);
            standardOnlyButtons.Add(btnDecimal);

            AddButton(grid, "C", 5, 0, buttonClear_Click);
            Button equalsBtn = CreateButton("=", buttonEquals_Click);
            grid.Controls.Add(equalsBtn, 1, 5);
            grid.SetColumnSpan(equalsBtn, 3);
            equalsBtn.Dock = DockStyle.Fill;

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
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderColor = Color.DarkGray;
            btn.FlatAppearance.MouseOverBackColor = Color.LightGray;
            btn.FlatAppearance.MouseDownBackColor = Color.DarkGray;
            btn.Click += onClick;
            return btn;
        }
    }
}