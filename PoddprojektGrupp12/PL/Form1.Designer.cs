namespace PL
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
            PoddHanteraren = new GroupBox();
            label5 = new Label();
            button8 = new Button();
            textBox4 = new TextBox();
            button7 = new Button();
            button5 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            button1 = new Button();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            button3 = new Button();
            groupBox1 = new GroupBox();
            textBox2 = new TextBox();
            label3 = new Label();
            listBox2 = new ListBox();
            groupBox2 = new GroupBox();
            button6 = new Button();
            listBox3 = new ListBox();
            button4 = new Button();
            textBox3 = new TextBox();
            label4 = new Label();
            PoddHanteraren.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // PoddHanteraren
            // 
            PoddHanteraren.Controls.Add(label5);
            PoddHanteraren.Controls.Add(button8);
            PoddHanteraren.Controls.Add(textBox4);
            PoddHanteraren.Controls.Add(button7);
            PoddHanteraren.Controls.Add(button5);
            PoddHanteraren.Controls.Add(button2);
            PoddHanteraren.Controls.Add(listBox1);
            PoddHanteraren.Controls.Add(button1);
            PoddHanteraren.Controls.Add(comboBox1);
            PoddHanteraren.Controls.Add(label2);
            PoddHanteraren.Controls.Add(label1);
            PoddHanteraren.Controls.Add(textBox1);
            PoddHanteraren.Location = new Point(20, 35);
            PoddHanteraren.Margin = new Padding(5);
            PoddHanteraren.Name = "PoddHanteraren";
            PoddHanteraren.Padding = new Padding(5);
            PoddHanteraren.Size = new Size(791, 1080);
            PoddHanteraren.TabIndex = 0;
            PoddHanteraren.TabStop = false;
            PoddHanteraren.Text = "Hantera Poddar";

            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 159);
            label5.Name = "label5";
            label5.Size = new Size(84, 32);
            label5.TabIndex = 10;
            label5.Text = "Namn:";
            // 
            // button8
            // 
            button8.Location = new Point(23, 875);
            button8.Name = "button8";
            button8.Size = new Size(150, 46);
            button8.TabIndex = 3;
            button8.Text = "Visa Alla Poddar";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(177, 159);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(430, 39);
            textBox4.TabIndex = 9;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ControlLight;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Location = new Point(618, 83);
            button7.Name = "button7";
            button7.Size = new Size(153, 48);
            button7.TabIndex = 8;
            button7.Text = "Hämta";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnHamta_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ControlLight;
            button5.FlatAppearance.MouseDownBackColor = Color.Navy;
            button5.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button5.FlatStyle = FlatStyle.Flat;
            button5.Location = new Point(23, 1001);
            button5.Margin = new Padding(5);
            button5.Name = "button5";
            button5.Size = new Size(318, 46);
            button5.TabIndex = 8;
            button5.Text = "Ändra Namn/Kategori";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ControlLight;
            button2.FlatAppearance.MouseDownBackColor = Color.Navy;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(372, 1001);
            button2.Margin = new Padding(5);
            button2.Name = "button2";
            button2.Size = new Size(153, 46);
            button2.TabIndex = 6;
            button2.Text = "Ta Bort";
            button2.UseVisualStyleBackColor = false;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(23, 307);
            listBox1.Margin = new Padding(5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(756, 548);
            listBox1.TabIndex = 5;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ControlLight;
            button1.FlatAppearance.MouseDownBackColor = Color.Navy;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(618, 223);
            button1.Margin = new Padding(5);
            button1.Name = "button1";
            button1.Size = new Size(153, 46);
            button1.TabIndex = 4;
            button1.Text = "Spara";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnSavePodcast_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(177, 224);
            comboBox1.Margin = new Padding(5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(430, 40);
            comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 227);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(129, 32);
            label2.TabIndex = 2;
            label2.Text = "Kategorier:";

            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 98);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(60, 32);
            label1.TabIndex = 1;
            label1.Text = "URL:";

            // 
            // textBox1
            // 
            textBox1.Location = new Point(177, 86);
            textBox1.Margin = new Padding(5);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(430, 39);
            textBox1.TabIndex = 0;

            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLight;
            button3.FlatAppearance.MouseDownBackColor = Color.Navy;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(250, 398);
            button3.Margin = new Padding(5);
            button3.Name = "button3";
            button3.Size = new Size(153, 46);
            button3.TabIndex = 7;
            button3.Text = "Ändra";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(listBox2);
            groupBox1.Location = new Point(852, 37);
            groupBox1.Margin = new Padding(5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(5);
            groupBox1.Size = new Size(614, 592);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Avsnitt";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(15, 288);
            textBox2.Margin = new Padding(5);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(587, 262);
            textBox2.TabIndex = 2;
            textBox2.TextChanged += listBox2_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(15, 235);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(142, 32);
            label3.TabIndex = 1;
            label3.Text = "Beskrivning:";

            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.Location = new Point(15, 54);
            listBox2.Margin = new Padding(5);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(587, 164);
            listBox2.TabIndex = 0;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(listBox3);
            groupBox2.Controls.Add(button3);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(textBox3);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(847, 638);
            groupBox2.Margin = new Padding(5);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(5);
            groupBox2.Size = new Size(609, 477);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Kategorier";
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ControlLight;
            button6.FlatAppearance.MouseDownBackColor = Color.Navy;
            button6.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button6.FlatStyle = FlatStyle.Flat;
            button6.Location = new Point(437, 398);
            button6.Margin = new Padding(5);
            button6.Name = "button6";
            button6.Size = new Size(153, 46);
            button6.TabIndex = 9;
            button6.Text = "Ta Bort";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.Location = new Point(29, 104);
            listBox3.Margin = new Padding(5);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(558, 260);
            listBox3.TabIndex = 3;
            listBox3.SelectedIndexChanged += listBox3_SelectedIndexChanged;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ControlLight;
            button4.FlatAppearance.MouseDownBackColor = Color.Navy;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(437, 48);
            button4.Margin = new Padding(5);
            button4.Name = "button4";
            button4.Size = new Size(153, 46);
            button4.TabIndex = 2;
            button4.Text = "Spara";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnSaveCategory_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(124, 48);
            textBox3.Margin = new Padding(5);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(301, 39);
            textBox3.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 53);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(84, 32);
            label4.TabIndex = 0;
            label4.Text = "Namn:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1736, 1155);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(PoddHanteraren);
            Margin = new Padding(5);
            Name = "Form1";
            Text = "Poddhanteraren";
            Load += Form1_Load;
            PoddHanteraren.ResumeLayout(false);
            PoddHanteraren.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox PoddHanteraren;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Button button3;
        private Button button2;
        private ListBox listBox1;
        private Button button1;
        private ComboBox comboBox1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label3;
        private ListBox listBox2;
        private TextBox textBox2;
        private ListBox listBox3;
        private Button button4;
        private TextBox textBox3;
        private Label label4;
        private Button button6;
        private Button button5;
        private Button button7;
        private TextBox textBox4;
        private Button button8;
        private Label label5;
    }
}
