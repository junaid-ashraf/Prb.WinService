namespace Prb.ServiceOperation
{
    partial class MainForm
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
            this.btnUninstall = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.btnclearresult = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStopService = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.checkBoxOthers = new System.Windows.Forms.CheckBox();
            this.checkBoxRoutersSwitches = new System.Windows.Forms.CheckBox();
            this.checkBoxPrinters = new System.Windows.Forms.CheckBox();
            this.checkBoxLinux = new System.Windows.Forms.CheckBox();
            this.checkBoxMac = new System.Windows.Forms.CheckBox();
            this.checkBoxWin = new System.Windows.Forms.CheckBox();
            this.tbDmoanName = new System.Windows.Forms.TextBox();
            this.labelDomainName = new System.Windows.Forms.Label();
            this.labelDomainAdmin = new System.Windows.Forms.Label();
            this.tbDomainAdmin = new System.Windows.Forms.TextBox();
            this.labelPass = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonDomainSelect = new System.Windows.Forms.RadioButton();
            this.radioButtonDomainAuto = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxHwDetail = new System.Windows.Forms.CheckBox();
            this.checkBoxSwDetail = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUninstall
            // 
            this.btnUninstall.Location = new System.Drawing.Point(554, 115);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(130, 53);
            this.btnUninstall.TabIndex = 22;
            this.btnUninstall.Text = "Uninstall Service";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMessage);
            this.groupBox1.Location = new System.Drawing.Point(12, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(806, 163);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(20, 23);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(766, 125);
            this.txtMessage.TabIndex = 6;
            this.txtMessage.Text = "";
            // 
            // btnclearresult
            // 
            this.btnclearresult.Location = new System.Drawing.Point(690, 115);
            this.btnclearresult.Name = "btnclearresult";
            this.btnclearresult.Size = new System.Drawing.Size(130, 53);
            this.btnclearresult.TabIndex = 20;
            this.btnclearresult.Text = "Clear Result";
            this.btnclearresult.UseVisualStyleBackColor = true;
            this.btnclearresult.Click += new System.EventHandler(this.btnclearresult_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(364, 116);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(166, 23);
            this.btnRestart.TabIndex = 19;
            this.btnRestart.Text = "Restart Service";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStopService
            // 
            this.btnStopService.Location = new System.Drawing.Point(364, 145);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(166, 23);
            this.btnStopService.TabIndex = 18;
            this.btnStopService.Text = "Stop Service";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopService_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(188, 115);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(152, 53);
            this.btnStart.TabIndex = 17;
            this.btnStart.Text = "Start Service";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnService
            // 
            this.btnService.Location = new System.Drawing.Point(12, 115);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(153, 53);
            this.btnService.TabIndex = 16;
            this.btnService.Text = "Install Service";
            this.btnService.UseVisualStyleBackColor = true;
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // checkBoxOthers
            // 
            this.checkBoxOthers.AutoSize = true;
            this.checkBoxOthers.Checked = true;
            this.checkBoxOthers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOthers.Location = new System.Drawing.Point(102, 65);
            this.checkBoxOthers.Name = "checkBoxOthers";
            this.checkBoxOthers.Size = new System.Drawing.Size(57, 17);
            this.checkBoxOthers.TabIndex = 5;
            this.checkBoxOthers.Text = "Others";
            this.checkBoxOthers.UseVisualStyleBackColor = true;
            // 
            // checkBoxRoutersSwitches
            // 
            this.checkBoxRoutersSwitches.AutoSize = true;
            this.checkBoxRoutersSwitches.Checked = true;
            this.checkBoxRoutersSwitches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRoutersSwitches.Location = new System.Drawing.Point(102, 44);
            this.checkBoxRoutersSwitches.Name = "checkBoxRoutersSwitches";
            this.checkBoxRoutersSwitches.Size = new System.Drawing.Size(120, 17);
            this.checkBoxRoutersSwitches.TabIndex = 4;
            this.checkBoxRoutersSwitches.Text = "Routers / Switches ";
            this.checkBoxRoutersSwitches.UseVisualStyleBackColor = true;
            // 
            // checkBoxPrinters
            // 
            this.checkBoxPrinters.AutoSize = true;
            this.checkBoxPrinters.Checked = true;
            this.checkBoxPrinters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPrinters.Location = new System.Drawing.Point(102, 23);
            this.checkBoxPrinters.Name = "checkBoxPrinters";
            this.checkBoxPrinters.Size = new System.Drawing.Size(61, 17);
            this.checkBoxPrinters.TabIndex = 3;
            this.checkBoxPrinters.Text = "Printers";
            this.checkBoxPrinters.UseVisualStyleBackColor = true;
            // 
            // checkBoxLinux
            // 
            this.checkBoxLinux.AutoSize = true;
            this.checkBoxLinux.Checked = true;
            this.checkBoxLinux.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLinux.Location = new System.Drawing.Point(18, 65);
            this.checkBoxLinux.Name = "checkBoxLinux";
            this.checkBoxLinux.Size = new System.Drawing.Size(51, 17);
            this.checkBoxLinux.TabIndex = 2;
            this.checkBoxLinux.Text = "Linux";
            this.checkBoxLinux.UseVisualStyleBackColor = true;
            // 
            // checkBoxMac
            // 
            this.checkBoxMac.AutoSize = true;
            this.checkBoxMac.Checked = true;
            this.checkBoxMac.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMac.Location = new System.Drawing.Point(18, 44);
            this.checkBoxMac.Name = "checkBoxMac";
            this.checkBoxMac.Size = new System.Drawing.Size(49, 17);
            this.checkBoxMac.TabIndex = 1;
            this.checkBoxMac.Text = "MAC";
            this.checkBoxMac.UseVisualStyleBackColor = true;
            // 
            // checkBoxWin
            // 
            this.checkBoxWin.AutoSize = true;
            this.checkBoxWin.Checked = true;
            this.checkBoxWin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxWin.Location = new System.Drawing.Point(18, 23);
            this.checkBoxWin.Name = "checkBoxWin";
            this.checkBoxWin.Size = new System.Drawing.Size(70, 17);
            this.checkBoxWin.TabIndex = 0;
            this.checkBoxWin.Text = "Windows";
            this.checkBoxWin.UseVisualStyleBackColor = true;
            // 
            // tbDmoanName
            // 
            this.tbDmoanName.Location = new System.Drawing.Point(212, 19);
            this.tbDmoanName.Name = "tbDmoanName";
            this.tbDmoanName.Size = new System.Drawing.Size(116, 20);
            this.tbDmoanName.TabIndex = 0;
            this.tbDmoanName.Text = "techimp.local";
            // 
            // labelDomainName
            // 
            this.labelDomainName.AutoSize = true;
            this.labelDomainName.Location = new System.Drawing.Point(123, 22);
            this.labelDomainName.Name = "labelDomainName";
            this.labelDomainName.Size = new System.Drawing.Size(74, 13);
            this.labelDomainName.TabIndex = 1;
            this.labelDomainName.Text = "Domain Name";
            // 
            // labelDomainAdmin
            // 
            this.labelDomainAdmin.AutoSize = true;
            this.labelDomainAdmin.Location = new System.Drawing.Point(123, 47);
            this.labelDomainAdmin.Name = "labelDomainAdmin";
            this.labelDomainAdmin.Size = new System.Drawing.Size(75, 13);
            this.labelDomainAdmin.TabIndex = 3;
            this.labelDomainAdmin.Text = "Domian Admin";
            // 
            // tbDomainAdmin
            // 
            this.tbDomainAdmin.Location = new System.Drawing.Point(212, 44);
            this.tbDomainAdmin.Name = "tbDomainAdmin";
            this.tbDomainAdmin.Size = new System.Drawing.Size(116, 20);
            this.tbDomainAdmin.TabIndex = 2;
            this.tbDomainAdmin.Text = "mis";
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Location = new System.Drawing.Point(123, 70);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(53, 13);
            this.labelPass.TabIndex = 5;
            this.labelPass.Text = "Password";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(212, 67);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(116, 20);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.Text = "VeryPazz(90)";
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonDomainSelect);
            this.groupBox2.Controls.Add(this.radioButtonDomainAuto);
            this.groupBox2.Controls.Add(this.labelDomainName);
            this.groupBox2.Controls.Add(this.tbDmoanName);
            this.groupBox2.Controls.Add(this.labelPass);
            this.groupBox2.Controls.Add(this.tbDomainAdmin);
            this.groupBox2.Controls.Add(this.tbPassword);
            this.groupBox2.Controls.Add(this.labelDomainAdmin);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(345, 100);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Probe Site";
            // 
            // radioButtonDomainSelect
            // 
            this.radioButtonDomainSelect.AutoSize = true;
            this.radioButtonDomainSelect.Checked = true;
            this.radioButtonDomainSelect.Location = new System.Drawing.Point(15, 47);
            this.radioButtonDomainSelect.Name = "radioButtonDomainSelect";
            this.radioButtonDomainSelect.Size = new System.Drawing.Size(82, 17);
            this.radioButtonDomainSelect.TabIndex = 7;
            this.radioButtonDomainSelect.TabStop = true;
            this.radioButtonDomainSelect.Text = "Domain Info";
            this.radioButtonDomainSelect.UseVisualStyleBackColor = true;
            this.radioButtonDomainSelect.CheckedChanged += new System.EventHandler(this.radioButtonDomainSelect_CheckedChanged);
            // 
            // radioButtonDomainAuto
            // 
            this.radioButtonDomainAuto.AutoSize = true;
            this.radioButtonDomainAuto.Location = new System.Drawing.Point(15, 20);
            this.radioButtonDomainAuto.Name = "radioButtonDomainAuto";
            this.radioButtonDomainAuto.Size = new System.Drawing.Size(98, 17);
            this.radioButtonDomainAuto.TabIndex = 6;
            this.radioButtonDomainAuto.Text = "Default Domain";
            this.radioButtonDomainAuto.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxWin);
            this.groupBox3.Controls.Add(this.checkBoxMac);
            this.groupBox3.Controls.Add(this.checkBoxLinux);
            this.groupBox3.Controls.Add(this.checkBoxOthers);
            this.groupBox3.Controls.Add(this.checkBoxPrinters);
            this.groupBox3.Controls.Add(this.checkBoxRoutersSwitches);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(364, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(230, 100);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Probe Scanning";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxHwDetail);
            this.groupBox4.Controls.Add(this.checkBoxSwDetail);
            this.groupBox4.Location = new System.Drawing.Point(611, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(187, 97);
            this.groupBox4.TabIndex = 24;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Scanning Type";
            // 
            // checkBoxHwDetail
            // 
            this.checkBoxHwDetail.AutoSize = true;
            this.checkBoxHwDetail.Checked = true;
            this.checkBoxHwDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHwDetail.Location = new System.Drawing.Point(40, 23);
            this.checkBoxHwDetail.Name = "checkBoxHwDetail";
            this.checkBoxHwDetail.Size = new System.Drawing.Size(102, 17);
            this.checkBoxHwDetail.TabIndex = 4;
            this.checkBoxHwDetail.Text = "Hardware Detail";
            this.checkBoxHwDetail.UseVisualStyleBackColor = true;
            // 
            // checkBoxSwDetail
            // 
            this.checkBoxSwDetail.AutoSize = true;
            this.checkBoxSwDetail.Checked = true;
            this.checkBoxSwDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSwDetail.Location = new System.Drawing.Point(40, 48);
            this.checkBoxSwDetail.Name = "checkBoxSwDetail";
            this.checkBoxSwDetail.Size = new System.Drawing.Size(98, 17);
            this.checkBoxSwDetail.TabIndex = 5;
            this.checkBoxSwDetail.Text = "Software Detail";
            this.checkBoxSwDetail.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(842, 60);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 371);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnUninstall);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnclearresult);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnStopService);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnService);
            this.Name = "MainForm";
            this.Text = "Prb Active Directory With Sqlite";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Button btnclearresult;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnService;
        private System.Windows.Forms.CheckBox checkBoxOthers;
        private System.Windows.Forms.CheckBox checkBoxRoutersSwitches;
        private System.Windows.Forms.CheckBox checkBoxPrinters;
        private System.Windows.Forms.CheckBox checkBoxLinux;
        private System.Windows.Forms.CheckBox checkBoxMac;
        private System.Windows.Forms.CheckBox checkBoxWin;
        private System.Windows.Forms.TextBox tbDmoanName;
        private System.Windows.Forms.Label labelDomainName;
        private System.Windows.Forms.Label labelDomainAdmin;
        private System.Windows.Forms.TextBox tbDomainAdmin;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonDomainSelect;
        private System.Windows.Forms.RadioButton radioButtonDomainAuto;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxHwDetail;
        private System.Windows.Forms.CheckBox checkBoxSwDetail;
        private System.Windows.Forms.Button button1;
    }
}

