namespace DCS_Configuration_Tool
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.UpdateSims = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button4 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.allChkBx = new System.Windows.Forms.CheckBox();
            this.upsChkBx = new System.Windows.Forms.CheckBox();
            this.switchChkBx = new System.Windows.Forms.CheckBox();
            this.scsAChkBx = new System.Windows.Forms.CheckBox();
            this.scsDChkBx = new System.Windows.Forms.CheckBox();
            this.pickleChkBx = new System.Windows.Forms.CheckBox();
            this.movChkBx = new System.Windows.Forms.CheckBox();
            this.ismChkBx = new System.Windows.Forms.CheckBox();
            this.bscChkBx = new System.Windows.Forms.CheckBox();
            this.aecChkBx = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateSims
            // 
            this.UpdateSims.Location = new System.Drawing.Point(124, 10);
            this.UpdateSims.Margin = new System.Windows.Forms.Padding(2);
            this.UpdateSims.Name = "UpdateSims";
            this.UpdateSims.Size = new System.Drawing.Size(75, 19);
            this.UpdateSims.TabIndex = 0;
            this.UpdateSims.Text = "UPDATE ";
            this.UpdateSims.UseVisualStyleBackColor = true;
            this.UpdateSims.Click += new System.EventHandler(this.UpdateSimulators);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(127, 165);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 19);
            this.button2.TabIndex = 1;
            this.button2.Text = "STOP ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.StopSimulators);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 165);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 19);
            this.button1.TabIndex = 2;
            this.button1.Text = "START ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.StartSimulators);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(15, 100);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(111, 19);
            this.button3.TabIndex = 3;
            this.button3.Text = "SET LAN";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.SetLocalAreaNetwork);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "HMS LAN",
            "AGS LAN 1",
            "AGS LAN 2",
            "ADMACS LAN"});
            this.checkedListBox1.Location = new System.Drawing.Point(4, 24);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkedListBox1.Size = new System.Drawing.Size(106, 60);
            this.checkedListBox1.TabIndex = 4;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(15, 126);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(125, 19);
            this.button4.TabIndex = 5;
            this.button4.Text = "SET IP NETWORK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.SetIpNetwork);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(4, 27);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(123, 17);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "FULL IP NETWORK";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(4, 48);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(123, 17);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "JCTS IP NETWORK";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(4, 69);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(150, 17);
            this.radioButton3.TabIndex = 8;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "NON-JCTS IP NETWORK";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(15, 96);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(125, 19);
            this.button5.TabIndex = 9;
            this.button5.Text = "TEST IP CONNECTION";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.CheckNetwork);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Location = new System.Drawing.Point(18, 151);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(164, 158);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP Configuration";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkedListBox1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(18, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(165, 134);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "LAN Configuration";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.allChkBx);
            this.groupBox3.Controls.Add(this.upsChkBx);
            this.groupBox3.Controls.Add(this.switchChkBx);
            this.groupBox3.Controls.Add(this.scsAChkBx);
            this.groupBox3.Controls.Add(this.scsDChkBx);
            this.groupBox3.Controls.Add(this.pickleChkBx);
            this.groupBox3.Controls.Add(this.movChkBx);
            this.groupBox3.Controls.Add(this.ismChkBx);
            this.groupBox3.Controls.Add(this.bscChkBx);
            this.groupBox3.Controls.Add(this.aecChkBx);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Location = new System.Drawing.Point(186, 11);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(218, 205);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Simulator Configuration";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // allChkBx
            // 
            this.allChkBx.AutoSize = true;
            this.allChkBx.Location = new System.Drawing.Point(93, 124);
            this.allChkBx.Name = "allChkBx";
            this.allChkBx.Size = new System.Drawing.Size(89, 17);
            this.allChkBx.TabIndex = 25;
            this.allChkBx.Text = "SELECT ALL";
            this.allChkBx.UseVisualStyleBackColor = true;
            this.allChkBx.CheckedChanged += new System.EventHandler(this.allChkBx_CheckedChanged);
            // 
            // upsChkBx
            // 
            this.upsChkBx.AutoSize = true;
            this.upsChkBx.Location = new System.Drawing.Point(21, 101);
            this.upsChkBx.Name = "upsChkBx";
            this.upsChkBx.Size = new System.Drawing.Size(48, 17);
            this.upsChkBx.TabIndex = 24;
            this.upsChkBx.Text = "UPS";
            this.upsChkBx.UseVisualStyleBackColor = true;
            // 
            // switchChkBx
            // 
            this.switchChkBx.AutoSize = true;
            this.switchChkBx.Location = new System.Drawing.Point(93, 78);
            this.switchChkBx.Name = "switchChkBx";
            this.switchChkBx.Size = new System.Drawing.Size(91, 17);
            this.switchChkBx.TabIndex = 23;
            this.switchChkBx.Text = "SWITCH SIM";
            this.switchChkBx.UseVisualStyleBackColor = true;
            // 
            // scsAChkBx
            // 
            this.scsAChkBx.AutoSize = true;
            this.scsAChkBx.Location = new System.Drawing.Point(93, 55);
            this.scsAChkBx.Name = "scsAChkBx";
            this.scsAChkBx.Size = new System.Drawing.Size(117, 17);
            this.scsAChkBx.TabIndex = 22;
            this.scsAChkBx.Text = "SCS ADMACS SIM";
            this.scsAChkBx.UseVisualStyleBackColor = true;
            // 
            // scsDChkBx
            // 
            this.scsDChkBx.AutoSize = true;
            this.scsDChkBx.Location = new System.Drawing.Point(93, 32);
            this.scsDChkBx.Name = "scsDChkBx";
            this.scsDChkBx.Size = new System.Drawing.Size(95, 17);
            this.scsDChkBx.TabIndex = 21;
            this.scsDChkBx.Text = "SCS DISPLAY";
            this.scsDChkBx.UseVisualStyleBackColor = true;
            // 
            // pickleChkBx
            // 
            this.pickleChkBx.AutoSize = true;
            this.pickleChkBx.Location = new System.Drawing.Point(93, 99);
            this.pickleChkBx.Name = "pickleChkBx";
            this.pickleChkBx.Size = new System.Drawing.Size(109, 17);
            this.pickleChkBx.TabIndex = 20;
            this.pickleChkBx.Text = "PICKLE SWITCH";
            this.pickleChkBx.UseVisualStyleBackColor = true;
            // 
            // movChkBx
            // 
            this.movChkBx.AutoSize = true;
            this.movChkBx.Location = new System.Drawing.Point(21, 124);
            this.movChkBx.Name = "movChkBx";
            this.movChkBx.Size = new System.Drawing.Size(50, 17);
            this.movChkBx.TabIndex = 19;
            this.movChkBx.Text = "MOV";
            this.movChkBx.UseVisualStyleBackColor = true;
            // 
            // ismChkBx
            // 
            this.ismChkBx.AutoSize = true;
            this.ismChkBx.Location = new System.Drawing.Point(21, 78);
            this.ismChkBx.Name = "ismChkBx";
            this.ismChkBx.Size = new System.Drawing.Size(45, 17);
            this.ismChkBx.TabIndex = 18;
            this.ismChkBx.Text = "ISM";
            this.ismChkBx.UseVisualStyleBackColor = true;
            // 
            // bscChkBx
            // 
            this.bscChkBx.AutoSize = true;
            this.bscChkBx.Location = new System.Drawing.Point(21, 55);
            this.bscChkBx.Name = "bscChkBx";
            this.bscChkBx.Size = new System.Drawing.Size(47, 17);
            this.bscChkBx.TabIndex = 17;
            this.bscChkBx.Text = "BSC";
            this.bscChkBx.UseVisualStyleBackColor = true;
            // 
            // aecChkBx
            // 
            this.aecChkBx.AutoSize = true;
            this.aecChkBx.Location = new System.Drawing.Point(21, 32);
            this.aecChkBx.Name = "aecChkBx";
            this.aecChkBx.Size = new System.Drawing.Size(47, 17);
            this.aecChkBx.TabIndex = 16;
            this.aecChkBx.Text = "AEC";
            this.aecChkBx.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(409, 14);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(455, 264);
            this.listBox1.TabIndex = 14;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Location = new System.Drawing.Point(189, 262);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(215, 47);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Log";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(119, 17);
            this.button7.Margin = new System.Windows.Forms.Padding(2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(80, 19);
            this.button7.TabIndex = 17;
            this.button7.Text = "DELETE  OUTPUT";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.deleteLog_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(18, 17);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(76, 19);
            this.button6.TabIndex = 16;
            this.button6.Text = "SAVE FILE";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.logFile_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(409, 286);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(455, 23);
            this.progressBar1.TabIndex = 16;
            this.progressBar1.Visible = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.UpdateSims);
            this.groupBox5.Location = new System.Drawing.Point(189, 220);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(215, 34);
            this.groupBox5.TabIndex = 26;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Update Simulators";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 342);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "DCS Configuration Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpdateSims;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox ismChkBx;
        private System.Windows.Forms.CheckBox bscChkBx;
        private System.Windows.Forms.CheckBox aecChkBx;
        private System.Windows.Forms.CheckBox movChkBx;
        private System.Windows.Forms.CheckBox pickleChkBx;
        private System.Windows.Forms.CheckBox scsAChkBx;
        private System.Windows.Forms.CheckBox scsDChkBx;
        private System.Windows.Forms.CheckBox upsChkBx;
        private System.Windows.Forms.CheckBox switchChkBx;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox allChkBx;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}

