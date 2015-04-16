namespace BlueSheep.Engine.ExceptionHandler
{
    partial class ExceptionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionDialog));
            this.btnMore = new System.Windows.Forms.Button();
            this.txtMore = new System.Windows.Forms.TextBox();
            this.lblMoreHeading = new System.Windows.Forms.Label();
            this.lblActionHeading = new System.Windows.Forms.Label();
            this.lblScopeHeading = new System.Windows.Forms.Label();
            this.lblErrorHeading = new System.Windows.Forms.Label();
            this.ActionBox = new System.Windows.Forms.RichTextBox();
            this.ScopeBox = new System.Windows.Forms.RichTextBox();
            this.ErrorBox = new System.Windows.Forms.RichTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.sadikButton1 = new BlueSheep.Interface.SadikButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMore
            // 
            this.btnMore.Location = new System.Drawing.Point(125, 383);
            this.btnMore.Name = "btnMore";
            this.btnMore.Size = new System.Drawing.Size(28, 24);
            this.btnMore.TabIndex = 17;
            this.btnMore.Text = ">>";
            this.btnMore.Click += new System.EventHandler(this.btnMore_Click_1);
            // 
            // txtMore
            // 
            this.txtMore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMore.CausesValidation = false;
            this.txtMore.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMore.Location = new System.Drawing.Point(24, 403);
            this.txtMore.Multiline = true;
            this.txtMore.Name = "txtMore";
            this.txtMore.ReadOnly = true;
            this.txtMore.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMore.Size = new System.Drawing.Size(443, 123);
            this.txtMore.TabIndex = 18;
            this.txtMore.Text = "(detailed information, such as exception details)";
            // 
            // lblMoreHeading
            // 
            this.lblMoreHeading.AutoSize = true;
            this.lblMoreHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblMoreHeading.Location = new System.Drawing.Point(21, 387);
            this.lblMoreHeading.Name = "lblMoreHeading";
            this.lblMoreHeading.Size = new System.Drawing.Size(105, 13);
            this.lblMoreHeading.TabIndex = 16;
            this.lblMoreHeading.Text = "More information";
            // 
            // lblActionHeading
            // 
            this.lblActionHeading.AutoSize = true;
            this.lblActionHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblActionHeading.Location = new System.Drawing.Point(21, 267);
            this.lblActionHeading.Name = "lblActionHeading";
            this.lblActionHeading.Size = new System.Drawing.Size(301, 13);
            this.lblActionHeading.TabIndex = 14;
            this.lblActionHeading.Text = "What you can do about it. Ce que vous pouvez faire :";
            // 
            // lblScopeHeading
            // 
            this.lblScopeHeading.AutoSize = true;
            this.lblScopeHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblScopeHeading.Location = new System.Drawing.Point(21, 179);
            this.lblScopeHeading.Name = "lblScopeHeading";
            this.lblScopeHeading.Size = new System.Drawing.Size(354, 13);
            this.lblScopeHeading.TabIndex = 12;
            this.lblScopeHeading.Text = "How this will affect you ? Comment cela va-t-il vous affecter ?";
            // 
            // lblErrorHeading
            // 
            this.lblErrorHeading.AutoSize = true;
            this.lblErrorHeading.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblErrorHeading.Location = new System.Drawing.Point(146, 65);
            this.lblErrorHeading.Name = "lblErrorHeading";
            this.lblErrorHeading.Size = new System.Drawing.Size(263, 13);
            this.lblErrorHeading.TabIndex = 9;
            this.lblErrorHeading.Text = "What happened ? Qu\'est-ce qu\'il s\'est passé ?";
            // 
            // ActionBox
            // 
            this.ActionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionBox.BackColor = System.Drawing.SystemColors.Control;
            this.ActionBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ActionBox.CausesValidation = false;
            this.ActionBox.Location = new System.Drawing.Point(37, 287);
            this.ActionBox.Name = "ActionBox";
            this.ActionBox.ReadOnly = true;
            this.ActionBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ActionBox.Size = new System.Drawing.Size(430, 92);
            this.ActionBox.TabIndex = 15;
            this.ActionBox.Text = "(action)";
            // 
            // ScopeBox
            // 
            this.ScopeBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScopeBox.BackColor = System.Drawing.SystemColors.Control;
            this.ScopeBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ScopeBox.CausesValidation = false;
            this.ScopeBox.Location = new System.Drawing.Point(37, 199);
            this.ScopeBox.Name = "ScopeBox";
            this.ScopeBox.ReadOnly = true;
            this.ScopeBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ScopeBox.Size = new System.Drawing.Size(430, 64);
            this.ScopeBox.TabIndex = 13;
            this.ScopeBox.Text = "(scope)";
            // 
            // ErrorBox
            // 
            this.ErrorBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorBox.BackColor = System.Drawing.SystemColors.Control;
            this.ErrorBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ErrorBox.CausesValidation = false;
            this.ErrorBox.Location = new System.Drawing.Point(149, 84);
            this.ErrorBox.Name = "ErrorBox";
            this.ErrorBox.ReadOnly = true;
            this.ErrorBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ErrorBox.Size = new System.Drawing.Size(318, 64);
            this.ErrorBox.TabIndex = 11;
            this.ErrorBox.Text = "(error message)";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(24, 63);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(119, 113);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // sadikButton1
            // 
            this.sadikButton1.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.sadikButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sadikButton1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikButton1.Image = null;
            this.sadikButton1.Location = new System.Drawing.Point(23, 541);
            this.sadikButton1.Name = "sadikButton1";
            this.sadikButton1.RoundedCorners = false;
            this.sadikButton1.Size = new System.Drawing.Size(443, 26);
            this.sadikButton1.TabIndex = 19;
            this.sadikButton1.Text = "Continue & Exit.";
            this.sadikButton1.Click += new System.EventHandler(this.sadikButton1_Click);
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 594);
            this.Controls.Add(this.sadikButton1);
            this.Controls.Add(this.btnMore);
            this.Controls.Add(this.txtMore);
            this.Controls.Add(this.lblMoreHeading);
            this.Controls.Add(this.lblActionHeading);
            this.Controls.Add(this.lblScopeHeading);
            this.Controls.Add(this.lblErrorHeading);
            this.Controls.Add(this.ActionBox);
            this.Controls.Add(this.ScopeBox);
            this.Controls.Add(this.ErrorBox);
            this.Controls.Add(this.pictureBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExceptionDialog";
            this.Text = "BlueSheep crashed !";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnMore;
        internal System.Windows.Forms.TextBox txtMore;
        internal System.Windows.Forms.Label lblMoreHeading;
        internal System.Windows.Forms.Label lblActionHeading;
        internal System.Windows.Forms.Label lblScopeHeading;
        internal System.Windows.Forms.Label lblErrorHeading;
        internal System.Windows.Forms.RichTextBox ActionBox;
        internal System.Windows.Forms.RichTextBox ScopeBox;
        internal System.Windows.Forms.RichTextBox ErrorBox;
        internal System.Windows.Forms.PictureBox pictureBox2;
        private Interface.SadikButton sadikButton1;
    }
}