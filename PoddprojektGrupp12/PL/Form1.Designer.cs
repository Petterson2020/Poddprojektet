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
            button7 = new Button();
            button3 = new Button();
            button2 = new Button();
            listBox1 = new ListBox();
            button1 = new Button();
            comboBox1 = new ComboBox();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            textBox2 = new TextBox();
            label3 = new Label();
            listBox2 = new ListBox();
            groupBox2 = new GroupBox();
            button6 = new Button();
            button5 = new Button();
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
            PoddHanteraren.Controls.Add(button7);
            PoddHanteraren.Controls.Add(button3);
            PoddHanteraren.Controls.Add(button2);
            PoddHanteraren.Controls.Add(listBox1);
            PoddHanteraren.Controls.Add(button1);
            PoddHanteraren.Controls.Add(comboBox1);
            PoddHanteraren.Controls.Add(label2);
            PoddHanteraren.Controls.Add(label1);
            PoddHanteraren.Controls.Add(textBox1);
            PoddHanteraren.Location = new Point(12, 22);
            PoddHanteraren.Name = "PoddHanteraren";
            PoddHanteraren.Size = new Size(487, 607);
            PoddHanteraren.TabIndex = 0;
            PoddHanteraren.TabStop = false;
            PoddHanteraren.Text = "Hantera Poddar";
            PoddHanteraren.Enter += groupBox1_Enter;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ControlLight;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Location = new Point(380, 52);
            button7.Margin = new Padding(2);
            button7.Name = "button7";
            button7.Size = new Size(94, 30);
            button7.TabIndex = 8;
            button7.Text = "Hämta";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnHamta_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ControlLight;
            button3.FlatAppearance.MouseDownBackColor = Color.Navy;
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button3.FlatStyle = FlatStyle.Flat;
            button3.Location = new Point(271, 558);
            button3.Name = "button3";
            button3.Size = new Size(94, 29);
            button3.TabIndex = 7;
            button3.Text = "Ändra";
            button3.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ControlLight;
            button2.FlatAppearance.MouseDownBackColor = Color.Navy;
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(380, 558);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 6;
            button2.Text = "Ta Bort";
            button2.UseVisualStyleBackColor = false;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(14, 132);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(467, 404);
            listBox1.TabIndex = 5;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ControlLight;
            button1.FlatAppearance.MouseDownBackColor = Color.Navy;
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(380, 97);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 4;
            button1.Text = "Spara";
            button1.UseVisualStyleBackColor = false;
            button1.Click += btnSavePodcast_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(109, 97);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(266, 28);
            comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 100);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 2;
            label2.Text = "Kategorier:";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 61);
            label1.Name = "label1";
            label1.Size = new Size(38, 20);
            label1.TabIndex = 1;
            label1.Text = "URL:";
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(109, 54);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(266, 27);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(listBox2);
            groupBox1.Location = new Point(524, 23);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(378, 294);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Avsnitt";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(9, 170);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(363, 108);
            textBox2.TabIndex = 2;
            textBox2.TextChanged += listBox2_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 147);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 1;
            label3.Text = "Beskrivning:";
            label3.Click += label3_Click;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.Location = new Point(9, 34);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(363, 104);
            listBox2.TabIndex = 0;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(button5);
            groupBox2.Controls.Add(listBox3);
            groupBox2.Controls.Add(button4);
            groupBox2.Controls.Add(textBox3);
            groupBox2.Controls.Add(label4);
            groupBox2.Location = new Point(527, 331);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(375, 298);
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
            button6.Location = new Point(269, 249);
            button6.Name = "button6";
            button6.Size = new Size(94, 29);
            button6.TabIndex = 9;
            button6.Text = "Ta Bort";
            button6.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ControlLight;
            button5.FlatAppearance.MouseDownBackColor = Color.Navy;
            button5.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button5.FlatStyle = FlatStyle.Flat;
            button5.Location = new Point(160, 249);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 8;
            button5.Text = "Ändra";
            button5.UseVisualStyleBackColor = false;
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.Location = new Point(18, 65);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(345, 164);
            listBox3.TabIndex = 3;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ControlLight;
            button4.FlatAppearance.MouseDownBackColor = Color.Navy;
            button4.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 0, 192);
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(269, 30);
            button4.Name = "button4";
            button4.Size = new Size(94, 29);
            button4.TabIndex = 2;
            button4.Text = "Spara";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnSaveCategory_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(76, 30);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(187, 27);
            textBox3.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 33);
            label4.Name = "label4";
            label4.Size = new Size(52, 20);
            label4.TabIndex = 0;
            label4.Text = "Namn:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 641);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(PoddHanteraren);
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
    }
}
