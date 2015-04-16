﻿namespace BlueSheep.Interface
 {
     partial class GestAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestAccounts));
            this.sadikTabControl1 = new BlueSheep.Interface.SadikTabControl();
            this.LaunchPage = new System.Windows.Forms.TabPage();
            this.IsMITM = new BlueSheep.Interface.SadikCheckbox();
            this.DelBt = new BlueSheep.Interface.SadikButton();
            this.listViewAccounts = new System.Windows.Forms.ListView();
            this.LaunchAccountsSelecBt = new BlueSheep.Interface.SadikButton();
            this.AddPage = new System.Windows.Forms.TabPage();
            this.sadikLabel2 = new BlueSheep.Interface.SadikLabel();
            this.sadikLabel1 = new BlueSheep.Interface.SadikLabel();
            this.AddBt = new BlueSheep.Interface.SadikButton();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxAccountName = new System.Windows.Forms.TextBox();
            this.sadikTabControl1.SuspendLayout();
            this.LaunchPage.SuspendLayout();
            this.AddPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // sadikTabControl1
            // 
            this.sadikTabControl1.Controls.Add(this.LaunchPage);
            this.sadikTabControl1.Controls.Add(this.AddPage);
            this.sadikTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sadikTabControl1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikTabControl1.ItemSize = new System.Drawing.Size(0, 30);
            this.sadikTabControl1.Location = new System.Drawing.Point(20, 60);
            this.sadikTabControl1.Name = "sadikTabControl1";
            this.sadikTabControl1.SelectedIndex = 0;
            this.sadikTabControl1.Size = new System.Drawing.Size(276, 245);
            this.sadikTabControl1.TabIndex = 12;
            // 
            // LaunchPage
            // 
            this.LaunchPage.BackColor = System.Drawing.Color.White;
            this.LaunchPage.Controls.Add(this.IsMITM);
            this.LaunchPage.Controls.Add(this.DelBt);
            this.LaunchPage.Controls.Add(this.listViewAccounts);
            this.LaunchPage.Controls.Add(this.LaunchAccountsSelecBt);
            this.LaunchPage.Location = new System.Drawing.Point(4, 34);
            this.LaunchPage.Name = "LaunchPage";
            this.LaunchPage.Padding = new System.Windows.Forms.Padding(3);
            this.LaunchPage.Size = new System.Drawing.Size(268, 207);
            this.LaunchPage.TabIndex = 0;
            this.LaunchPage.Text = "Lancer";
            // 
            // IsMITM
            // 
            this.IsMITM.Checked = false;
            this.IsMITM.Font = new System.Drawing.Font("Verdana", 8F);
            this.IsMITM.Location = new System.Drawing.Point(66, 175);
            this.IsMITM.Name = "IsMITM";
            this.IsMITM.Size = new System.Drawing.Size(165, 20);
            this.IsMITM.TabIndex = 15;
            this.IsMITM.Text = "Mode MITM";
            this.IsMITM.CheckedChanged += new BlueSheep.Interface.SadikCheckbox.CheckedChangedEventHandler(this.IsMITM_CheckedChanged);
            // 
            // DelBt
            // 
            this.DelBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.DelBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.DelBt.Image = null;
            this.DelBt.Location = new System.Drawing.Point(23, 143);
            this.DelBt.Name = "DelBt";
            this.DelBt.RoundedCorners = false;
            this.DelBt.Size = new System.Drawing.Size(85, 26);
            this.DelBt.TabIndex = 14;
            this.DelBt.Text = "Supprimer";
            this.DelBt.Click += new System.EventHandler(this.DelBt_Click);
            // 
            // listViewAccounts
            // 
            this.listViewAccounts.Location = new System.Drawing.Point(23, 6);
            this.listViewAccounts.Name = "listViewAccounts";
            this.listViewAccounts.Size = new System.Drawing.Size(228, 131);
            this.listViewAccounts.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewAccounts.TabIndex = 13;
            this.listViewAccounts.UseCompatibleStateImageBehavior = false;
            this.listViewAccounts.View = System.Windows.Forms.View.Details;
            // 
            // LaunchAccountsSelecBt
            // 
            this.LaunchAccountsSelecBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.LaunchAccountsSelecBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LaunchAccountsSelecBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.LaunchAccountsSelecBt.Image = null;
            this.LaunchAccountsSelecBt.Location = new System.Drawing.Point(114, 143);
            this.LaunchAccountsSelecBt.Name = "LaunchAccountsSelecBt";
            this.LaunchAccountsSelecBt.RoundedCorners = false;
            this.LaunchAccountsSelecBt.Size = new System.Drawing.Size(137, 26);
            this.LaunchAccountsSelecBt.TabIndex = 12;
            this.LaunchAccountsSelecBt.Text = "Lancer les comptes";
            this.LaunchAccountsSelecBt.Click += new System.EventHandler(this.LaunchAccountsSelecBt_Click);
            // 
            // AddPage
            // 
            this.AddPage.BackColor = System.Drawing.Color.White;
            this.AddPage.Controls.Add(this.sadikLabel2);
            this.AddPage.Controls.Add(this.sadikLabel1);
            this.AddPage.Controls.Add(this.AddBt);
            this.AddPage.Controls.Add(this.textBoxPassword);
            this.AddPage.Controls.Add(this.textBoxAccountName);
            this.AddPage.Location = new System.Drawing.Point(4, 34);
            this.AddPage.Name = "AddPage";
            this.AddPage.Padding = new System.Windows.Forms.Padding(3);
            this.AddPage.Size = new System.Drawing.Size(268, 207);
            this.AddPage.TabIndex = 1;
            this.AddPage.Text = "Ajouter";
            // 
            // sadikLabel2
            // 
            this.sadikLabel2.AutoSize = true;
            this.sadikLabel2.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel2.Location = new System.Drawing.Point(13, 52);
            this.sadikLabel2.Name = "sadikLabel2";
            this.sadikLabel2.Size = new System.Drawing.Size(91, 13);
            this.sadikLabel2.TabIndex = 18;
            this.sadikLabel2.Text = "Mot de passe :";
            // 
            // sadikLabel1
            // 
            this.sadikLabel1.AutoSize = true;
            this.sadikLabel1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel1.Location = new System.Drawing.Point(13, 17);
            this.sadikLabel1.Name = "sadikLabel1";
            this.sadikLabel1.Size = new System.Drawing.Size(106, 13);
            this.sadikLabel1.TabIndex = 17;
            this.sadikLabel1.Text = "Nom de compte :";
            // 
            // AddBt
            // 
            this.AddBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.AddBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.AddBt.Image = null;
            this.AddBt.Location = new System.Drawing.Point(40, 91);
            this.AddBt.Name = "AddBt";
            this.AddBt.RoundedCorners = false;
            this.AddBt.Size = new System.Drawing.Size(188, 26);
            this.AddBt.TabIndex = 16;
            this.AddBt.Text = "Ajouter";
            this.AddBt.Click += new System.EventHandler(this.buttonAddAccount_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPassword.Location = new System.Drawing.Point(125, 49);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(137, 20);
            this.textBoxPassword.TabIndex = 15;
            this.textBoxPassword.Text = "Entrez le mot de passe...";
            this.textBoxPassword.GotFocus += new System.EventHandler(this.textBoxPassword_GotFocus);
            this.textBoxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_Enter);
            this.textBoxPassword.LostFocus += new System.EventHandler(this.textBoxPassword_LostFocus);
            // 
            // textBoxAccountName
            // 
            this.textBoxAccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAccountName.Location = new System.Drawing.Point(125, 14);
            this.textBoxAccountName.Name = "textBoxAccountName";
            this.textBoxAccountName.Size = new System.Drawing.Size(137, 20);
            this.textBoxAccountName.TabIndex = 14;
            this.textBoxAccountName.Text = "Entrez le nom de compte...";
            this.textBoxAccountName.GotFocus += new System.EventHandler(this.textBoxAccountName_GotFocus);
            this.textBoxAccountName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAccountName_Enter);
            this.textBoxAccountName.LostFocus += new System.EventHandler(this.textBoxAccountName_LostFocus);
            // 
            // GestAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 325);
            this.Controls.Add(this.sadikTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GestAccounts";
            this.Text = "Gestion des comptes";
            this.sadikTabControl1.ResumeLayout(false);
            this.LaunchPage.ResumeLayout(false);
            this.AddPage.ResumeLayout(false);
            this.AddPage.PerformLayout();
            this.ResumeLayout(false);

         }
         #endregion
         private System.Windows.Forms.TabPage AddPage;
         private SadikLabel sadikLabel2;
         private SadikLabel sadikLabel1;
         private SadikButton AddBt;
         private System.Windows.Forms.TextBox textBoxPassword;
         private System.Windows.Forms.TextBox textBoxAccountName;
         private System.Windows.Forms.TabPage LaunchPage;
         private SadikButton DelBt;
         private System.Windows.Forms.ListView listViewAccounts;
         private SadikButton LaunchAccountsSelecBt;
         private SadikTabControl sadikTabControl1;
         private SadikCheckbox IsMITM;
     }
 }