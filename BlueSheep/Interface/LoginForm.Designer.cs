namespace BlueSheep.Interface
{
    partial class LoginForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.textBoxAccount = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.linkLabelForgottenPassword = new System.Windows.Forms.LinkLabel();
            this.linkLabelCreateAccount = new System.Windows.Forms.LinkLabel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonConnect = new BlueSheep.Interface.SadikButton();
            this.sadikLabel1 = new BlueSheep.Interface.SadikLabel();
            this.sadikLabel2 = new BlueSheep.Interface.SadikLabel();
            this.SuspendLayout();
            // 
            // textBoxAccount
            // 
            this.textBoxAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAccount.Location = new System.Drawing.Point(136, 15);
            this.textBoxAccount.Name = "textBoxAccount";
            this.textBoxAccount.Size = new System.Drawing.Size(164, 20);
            this.textBoxAccount.TabIndex = 2;
            this.textBoxAccount.Text = "Entrez le nom d\'utilisateur...";
            this.textBoxAccount.GotFocus += new System.EventHandler(this.textBoxAccountName_GotFocus);
            this.textBoxAccount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAccountName_Enter);
            this.textBoxAccount.LostFocus += new System.EventHandler(this.textBoxAccountName_LostFocus);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(119, 50);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(181, 20);
            this.textBoxPassword.TabIndex = 3;
            this.textBoxPassword.Text = "Entrez le mot de passe...";
            this.textBoxPassword.GotFocus += new System.EventHandler(this.textBoxPassword_GotFocus);
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_Enter);
            this.textBoxPassword.LostFocus += new System.EventHandler(this.textBoxPassword_LostFocus);
            // 
            // linkLabelForgottenPassword
            // 
            this.linkLabelForgottenPassword.AutoSize = true;
            this.linkLabelForgottenPassword.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelForgottenPassword.Location = new System.Drawing.Point(15, 121);
            this.linkLabelForgottenPassword.Name = "linkLabelForgottenPassword";
            this.linkLabelForgottenPassword.Size = new System.Drawing.Size(111, 13);
            this.linkLabelForgottenPassword.TabIndex = 4;
            this.linkLabelForgottenPassword.TabStop = true;
            this.linkLabelForgottenPassword.Text = "Mot de passe oublié ?";
            this.linkLabelForgottenPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelForgottenPassword_LinkClicked);
            // 
            // linkLabelCreateAccount
            // 
            this.linkLabelCreateAccount.AutoSize = true;
            this.linkLabelCreateAccount.LinkColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelCreateAccount.Location = new System.Drawing.Point(147, 121);
            this.linkLabelCreateAccount.Name = "linkLabelCreateAccount";
            this.linkLabelCreateAccount.Size = new System.Drawing.Size(146, 13);
            this.linkLabelCreateAccount.TabIndex = 5;
            this.linkLabelCreateAccount.TabStop = true;
            this.linkLabelCreateAccount.Text = "Créer un compte BlueSheep !";
            this.linkLabelCreateAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCreateAccount_LinkClicked);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "1392525079_5202.ico");
            // 
            // buttonConnect
            // 
            this.buttonConnect.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.buttonConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonConnect.Font = new System.Drawing.Font("Verdana", 8F);
            this.buttonConnect.Image = null;
            this.buttonConnect.Location = new System.Drawing.Point(18, 85);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.RoundedCorners = false;
            this.buttonConnect.Size = new System.Drawing.Size(282, 26);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "Connecter";
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // sadikLabel1
            // 
            this.sadikLabel1.AutoSize = true;
            this.sadikLabel1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel1.Location = new System.Drawing.Point(18, 18);
            this.sadikLabel1.Name = "sadikLabel1";
            this.sadikLabel1.Size = new System.Drawing.Size(112, 13);
            this.sadikLabel1.TabIndex = 7;
            this.sadikLabel1.Text = "Nom d\'utilisateur :";
            // 
            // sadikLabel2
            // 
            this.sadikLabel2.AutoSize = true;
            this.sadikLabel2.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel2.Location = new System.Drawing.Point(18, 53);
            this.sadikLabel2.Name = "sadikLabel2";
            this.sadikLabel2.Size = new System.Drawing.Size(95, 13);
            this.sadikLabel2.TabIndex = 8;
            this.sadikLabel2.Text = "Mot de passe : ";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(317, 143);
            this.Controls.Add(this.sadikLabel2);
            this.Controls.Add(this.sadikLabel1);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.linkLabelCreateAccount);
            this.Controls.Add(this.linkLabelForgottenPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxAccount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAccount;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.LinkLabel linkLabelForgottenPassword;
        private System.Windows.Forms.LinkLabel linkLabelCreateAccount;
        private System.Windows.Forms.ImageList imageList;
        private SadikButton buttonConnect;
        private SadikLabel sadikLabel1;
        private SadikLabel sadikLabel2;
    }
}