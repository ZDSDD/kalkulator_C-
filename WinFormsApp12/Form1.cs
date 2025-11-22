using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        private DisplayManager display;
        private HistoryManager historyManager;
        private ICalculatorStrategy _currentStrategy;
        private StandardStrategy _standardStrategy;
        private ProgrammerStrategy _programmerStrategy;

        public Form1()
        {
            InitializeComponent();
            SetupScalableLayout();
            historyManager = new HistoryManager(historyListBox);
            display = new DisplayManager(textBox1, label1, historyManager);

            _standardStrategy = new StandardStrategy(new Calculator(), display);
            _programmerStrategy = new ProgrammerStrategy(new ProgrammerCalculator(), display);

            _currentStrategy = _standardStrategy;
            UpdateUIVisibility();
        }

        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            string number = (sender as Button)?.Text ?? "";
            if (_currentStrategy.IsDigitAllowed(number))
            {
                _currentStrategy.AppendNumber(number);
            }
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            string op = (sender as Button)?.Text ?? "";
            _currentStrategy.ApplyOperator(op);
        }

        private void buttonUnary_Click(object sender, EventArgs e)
        {
            string op = (sender as Button)?.Text ?? "";
            _currentStrategy.ApplyUnary(op);
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            _currentStrategy.CalculateResult();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            _currentStrategy.Clear();
        }

        private void buttonSign_Click(object sender, EventArgs e) => _currentStrategy.ToggleSign();

        private void buttonDecimal_Click(object sender, EventArgs e) => _currentStrategy.AppendDecimal();

        private void buttonBase_Click(object sender, EventArgs e)
        {
            string baseText = (sender as Button)?.Text ?? "DEC";
            _currentStrategy.ChangeBase(baseText);

            UpdateNumericButtonEnabledState();
            HighlightSelectedBase();
        }

        private void buttonMode_Click(object sender, EventArgs e)
        {
            if (_currentStrategy is StandardStrategy)
            {
                _currentStrategy = _programmerStrategy;
                btnMode.Text = "Programmer";
            }
            else
            {
                _currentStrategy = _standardStrategy;
                btnMode.Text = "Standard";
            }

            _currentStrategy.Clear();
            UpdateUIVisibility();
        }


        private void UpdateUIVisibility()
        {
            bool isProg = _currentStrategy is ProgrammerStrategy;

            foreach (var btn in standardOnlyButtons) btn.Visible = !isProg;
            foreach (var btn in programmerOnlyButtons) btn.Visible = isProg;

            UpdateNumericButtonEnabledState();
            HighlightSelectedBase();
        }

        private void UpdateNumericButtonEnabledState()
        {
            foreach (var btn in numericButtons)
            {
                btn.Enabled = _currentStrategy.IsDigitAllowed(btn.Text);
            }
            foreach (var btn in hexButtons)
            {
                btn.Enabled = _currentStrategy.IsDigitAllowed(btn.Text);
            }
        }

        private void HighlightSelectedBase()
        {
            foreach (var b in baseButtons)
            {
                b.BackColor = baseButtonDefaultBackColor;
                b.FlatAppearance.BorderColor = Color.DarkGray;
            }

            if (_currentStrategy is ProgrammerStrategy progStrat)
            {
                string currentBaseName = progStrat.CurrentBaseName;
                var activeBtn = baseButtons.FirstOrDefault(b => b.Text == currentBaseName);
                if (activeBtn != null)
                {
                    activeBtn.BackColor = Color.CornflowerBlue;
                    activeBtn.FlatAppearance.BorderColor = Color.Black;
                }
            }
        }
    }
}

