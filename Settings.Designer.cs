namespace Auto_Trader_Platform
{
    partial class Settings
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
            lbInvestmentAmount = new Label();
            lblPort = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            lblAccountNumber = new Label();
            lblRoute = new Label();
            txtInvestmentAmount = new TextBox();
            txtPort = new TextBox();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtRoute = new TextBox();
            txtAccountNumber = new TextBox();
            btnConnect = new Button();
            lblSettings = new Label();
            btnBack = new Button();
            btnUpdateSettings = new Button();
            LblBuyingPower = new Label();
            SuspendLayout();
            // 
            // lbInvestmentAmount
            // 
            lbInvestmentAmount.AutoSize = true;
            lbInvestmentAmount.BackColor = SystemColors.ActiveBorder;
            lbInvestmentAmount.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbInvestmentAmount.Location = new Point(12, 74);
            lbInvestmentAmount.Name = "lbInvestmentAmount";
            lbInvestmentAmount.Size = new Size(180, 25);
            lbInvestmentAmount.TabIndex = 0;
            lbInvestmentAmount.Text = "Investment Amount:";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.BackColor = SystemColors.ActiveBorder;
            lblPort.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPort.Location = new Point(12, 115);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(50, 25);
            lblPort.TabIndex = 1;
            lblPort.Text = "Port:";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.BackColor = SystemColors.ActiveBorder;
            lblUsername.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUsername.Location = new Point(12, 154);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(101, 25);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.BackColor = SystemColors.ActiveBorder;
            lblPassword.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPassword.Location = new Point(12, 194);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(95, 25);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            // 
            // lblAccountNumber
            // 
            lblAccountNumber.AutoSize = true;
            lblAccountNumber.BackColor = SystemColors.ActiveBorder;
            lblAccountNumber.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAccountNumber.Location = new Point(12, 233);
            lblAccountNumber.Name = "lblAccountNumber";
            lblAccountNumber.Size = new Size(159, 25);
            lblAccountNumber.TabIndex = 4;
            lblAccountNumber.Text = "Account Number:";
            // 
            // lblRoute
            // 
            lblRoute.AutoSize = true;
            lblRoute.BackColor = SystemColors.ActiveBorder;
            lblRoute.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblRoute.Location = new Point(12, 276);
            lblRoute.Name = "lblRoute";
            lblRoute.Size = new Size(64, 25);
            lblRoute.TabIndex = 5;
            lblRoute.Text = "Route:";
            // 
            // txtInvestmentAmount
            // 
            txtInvestmentAmount.Location = new Point(194, 74);
            txtInvestmentAmount.Name = "txtInvestmentAmount";
            txtInvestmentAmount.Size = new Size(161, 23);
            txtInvestmentAmount.TabIndex = 6;
            // 
            // txtPort
            // 
            txtPort.Location = new Point(194, 115);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(161, 23);
            txtPort.TabIndex = 7;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(194, 154);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(161, 23);
            txtUsername.TabIndex = 8;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(194, 194);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(161, 23);
            txtPassword.TabIndex = 9;
            // 
            // txtRoute
            // 
            txtRoute.Location = new Point(194, 276);
            txtRoute.Name = "txtRoute";
            txtRoute.Size = new Size(161, 23);
            txtRoute.TabIndex = 10;
            // 
            // txtAccountNumber
            // 
            txtAccountNumber.Location = new Point(194, 233);
            txtAccountNumber.Name = "txtAccountNumber";
            txtAccountNumber.Size = new Size(161, 23);
            txtAccountNumber.TabIndex = 11;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = SystemColors.ActiveBorder;
            btnConnect.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConnect.Location = new Point(12, 387);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(208, 47);
            btnConnect.TabIndex = 12;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // lblSettings
            // 
            lblSettings.AutoSize = true;
            lblSettings.BackColor = SystemColors.AppWorkspace;
            lblSettings.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSettings.Location = new Point(12, 9);
            lblSettings.Name = "lblSettings";
            lblSettings.Size = new Size(84, 25);
            lblSettings.TabIndex = 13;
            lblSettings.Text = "Settings";
            // 
            // btnBack
            // 
            btnBack.Location = new Point(713, 13);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 14;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnUpdateSettings
            // 
            btnUpdateSettings.BackColor = SystemColors.ActiveBorder;
            btnUpdateSettings.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateSettings.Location = new Point(12, 331);
            btnUpdateSettings.Name = "btnUpdateSettings";
            btnUpdateSettings.Size = new Size(208, 50);
            btnUpdateSettings.TabIndex = 15;
            btnUpdateSettings.Text = "Update Settings";
            btnUpdateSettings.UseVisualStyleBackColor = false;
            btnUpdateSettings.Click += btnUpdateSettings_Click;
            // 
            // LblBuyingPower
            // 
            LblBuyingPower.AutoSize = true;
            LblBuyingPower.BackColor = SystemColors.ActiveBorder;
            LblBuyingPower.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblBuyingPower.Location = new Point(102, 9);
            LblBuyingPower.Name = "LblBuyingPower";
            LblBuyingPower.Size = new Size(136, 25);
            LblBuyingPower.TabIndex = 16;
            LblBuyingPower.Text = "Buying Power";
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LblBuyingPower);
            Controls.Add(btnUpdateSettings);
            Controls.Add(btnBack);
            Controls.Add(lblSettings);
            Controls.Add(btnConnect);
            Controls.Add(txtAccountNumber);
            Controls.Add(txtRoute);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(txtPort);
            Controls.Add(txtInvestmentAmount);
            Controls.Add(lblRoute);
            Controls.Add(lblAccountNumber);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(lblPort);
            Controls.Add(lbInvestmentAmount);
            Name = "Settings";
            Text = "Settings";
            Load += Settings_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Button btnConnect;           // Now public
        public Label LblBuyingPower;        // Now public
        private Label lbInvestmentAmount;
        private Label lblPort;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblAccountNumber;
        private Label lblRoute;
        private TextBox txtInvestmentAmount;
        private TextBox txtPort;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtRoute;
        private TextBox txtAccountNumber;
        private Label lblSettings;
        private Button btnBack;
        private Button btnUpdateSettings;

    }
}
