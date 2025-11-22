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
            public string Left { get; set; }
            public string Op { get; set; }
            public string Right { get; set; }
            public string Result { get; set; }

            public override string ToString()
            {
                return $"{Left} {Op} {Right} = {Result}";
            }
        }

        public HistoryManager(ListBox listBox)
        {
            _listBox = listBox;
        }

        public void AddEntry(string left, string op, string right, string result)
        {
            var item = new HistoryItem
            {
                Left = left,
                Op = op,
                Right = right,
                Result = result
            };

            _listBox.Items.Add(item);

            // Auto-scroll to bottom
            _listBox.TopIndex = _listBox.Items.Count - 1;
        }

        public void Clear()
        {
            _listBox.Items.Clear();
        }
    }
}