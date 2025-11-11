namespace WinFormsApp12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
            for (int i = 0; i < buttonNum.Length; i++)
            {
                buttonNum[i] = new Button();

                buttonNum[i].Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
                buttonNum[i].Location = new Point(10 + i*57, 204);
                buttonNum[i].Name = "button" + i;
                buttonNum[i].Size = new Size(50, 50);
                buttonNum[i].TabIndex = i;
                buttonNum[i].Text = (i+1).ToString();
                buttonNum[i].UseVisualStyleBackColor = true;
                buttonNum[i].Click += buttonNumeric_Click;

                Controls.Add(buttonNum[i]);
            }
            //buttonNum[1].Location = new Point(122, 204);
            

            label1.Text = "";
        }
        private Button [] buttonNum =  new Button[3];
        

        private void buttonNumeric_Click(object sender, EventArgs e)
        {
            if (czyObliczenia)
            {
                czyObliczenia = false;
                textBox1.Text = "";
            }
            textBox1.Text += (sender as Button)?.Text;
            label1.Text += (sender as Button)?.Text;
        }

        int wynik = 0;
        bool czyObliczenia = false;
        string operatorObliczen = "";
        private void buttonOperator_Click(object sender, EventArgs e)
        {
            string? symbol = (sender as Button)?.Text;
            label1.Text += $" {symbol} ";
            czyObliczenia = true;
            switch (operatorObliczen)
            {
                case "+":
                    wynik += Convert.ToInt32(textBox1.Text);
                    break;

                case "-":
                    wynik -= Convert.ToInt32(textBox1.Text);
                    break;
                case "":
                    wynik = Convert.ToInt32(textBox1.Text);
                    break;

            }
            operatorObliczen = symbol;
            textBox1.Text = wynik.ToString();
        }

        
    }
}
