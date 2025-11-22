using System;
using System.Windows.Forms;

namespace CalculatorApp
{
    /// <summary>
    /// Responsible solely for managing the history log (ListBox)
    /// </summary>
    public class HistoryManager
    {
        private readonly ListBox _listBox;

        // Simple data model for a history item
        public class HistoryItem
        {
            public string Calculation { get; set; }
            public string Result { get; set; }

            public override string ToString()
            {
                return $"{Calculation} = {Result}";
            }
        }

        public HistoryManager(ListBox listBox)
        {
            _listBox = listBox;
        }

        public void AddEntry(string calcualtion, string result)
        {
            var item = new HistoryItem
            {
                Calculation = calcualtion,
                Result = result
            };

            _listBox.Items.Add(item);

            _listBox.TopIndex = _listBox.Items.Count - 1;
        }

        public void Clear()
        {
            _listBox.Items.Clear();
        }
    }
}