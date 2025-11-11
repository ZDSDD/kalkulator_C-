namespace WinFormsApp12
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            textBox1 = new TextBox();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // button4
            // 
            button4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button4.Location = new Point(174, 247);
            button4.Margin = new Padding(4, 5, 4, 5);
            button4.Name = "button4";
            button4.Size = new Size(71, 83);
            button4.TabIndex = 5;
            button4.Text = "6";
            button4.UseVisualStyleBackColor = true;
            button4.Click += buttonNumeric_Click;
            // 
            // button5
            // 
            button5.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button5.Location = new Point(94, 247);
            button5.Margin = new Padding(4, 5, 4, 5);
            button5.Name = "button5";
            button5.Size = new Size(71, 83);
            button5.TabIndex = 4;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += buttonNumeric_Click;
            // 
            // button6
            // 
            button6.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button6.Location = new Point(14, 247);
            button6.Margin = new Padding(4, 5, 4, 5);
            button6.Name = "button6";
            button6.Size = new Size(71, 83);
            button6.TabIndex = 3;
            button6.Text = "4";
            button6.UseVisualStyleBackColor = true;
            button6.Click += buttonNumeric_Click;
            // 
            // button7
            // 
            button7.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button7.Location = new Point(174, 153);
            button7.Margin = new Padding(4, 5, 4, 5);
            button7.Name = "button7";
            button7.Size = new Size(71, 83);
            button7.TabIndex = 8;
            button7.Text = "9";
            button7.UseVisualStyleBackColor = true;
            button7.Click += buttonNumeric_Click;
            // 
            // button8
            // 
            button8.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button8.Location = new Point(94, 153);
            button8.Margin = new Padding(4, 5, 4, 5);
            button8.Name = "button8";
            button8.Size = new Size(71, 83);
            button8.TabIndex = 7;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += buttonNumeric_Click;
            // 
            // button9
            // 
            button9.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button9.Location = new Point(14, 153);
            button9.Margin = new Padding(4, 5, 4, 5);
            button9.Name = "button9";
            button9.Size = new Size(71, 83);
            button9.TabIndex = 6;
            button9.Text = "7";
            button9.UseVisualStyleBackColor = true;
            button9.Click += buttonNumeric_Click;
            // 
            // button10
            // 
            button10.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button10.Location = new Point(94, 433);
            button10.Margin = new Padding(4, 5, 4, 5);
            button10.Name = "button10";
            button10.Size = new Size(71, 83);
            button10.TabIndex = 9;
            button10.Text = "0";
            button10.UseVisualStyleBackColor = true;
            button10.Click += buttonNumeric_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(14, 52);
            textBox1.Margin = new Padding(4, 5, 4, 5);
            textBox1.Name = "textBox1";
            textBox1.RightToLeft = RightToLeft.Yes;
            textBox1.Size = new Size(327, 31);
            textBox1.TabIndex = 10;
            // 
            // button11
            // 
            button11.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button11.Location = new Point(271, 153);
            button11.Margin = new Padding(4, 5, 4, 5);
            button11.Name = "button11";
            button11.Size = new Size(71, 83);
            button11.TabIndex = 11;
            button11.Text = "/";
            button11.UseVisualStyleBackColor = true;
            button11.Click += buttonOperator_Click;
            // 
            // button12
            // 
            button12.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button12.Location = new Point(270, 247);
            button12.Margin = new Padding(4, 5, 4, 5);
            button12.Name = "button12";
            button12.Size = new Size(71, 83);
            button12.TabIndex = 12;
            button12.Text = "*";
            button12.UseVisualStyleBackColor = true;
            button12.Click += buttonOperator_Click;
            // 
            // button13
            // 
            button13.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button13.Location = new Point(270, 340);
            button13.Margin = new Padding(4, 5, 4, 5);
            button13.Name = "button13";
            button13.Size = new Size(71, 83);
            button13.TabIndex = 13;
            button13.Text = "-";
            button13.UseVisualStyleBackColor = true;
            button13.Click += buttonOperator_Click;
            // 
            // button14
            // 
            button14.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button14.Location = new Point(270, 433);
            button14.Margin = new Padding(4, 5, 4, 5);
            button14.Name = "button14";
            button14.Size = new Size(71, 83);
            button14.TabIndex = 14;
            button14.Text = "+";
            button14.UseVisualStyleBackColor = true;
            button14.Click += buttonOperator_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(287, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(59, 25);
            label1.TabIndex = 15;
            label1.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 726);
            Controls.Add(label1);
            Controls.Add(button14);
            Controls.Add(button13);
            Controls.Add(button12);
            Controls.Add(button11);
            Controls.Add(textBox1);
            Controls.Add(button10);
            Controls.Add(button7);
            Controls.Add(button8);
            Controls.Add(button9);
            Controls.Add(button4);
            Controls.Add(button5);
            Controls.Add(button6);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion


        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button button10;
        private TextBox textBox1;
        private Button button11;
        private Button button12;
        private Button button13;
        private Button button14;
        private Label label1;
    }
}
