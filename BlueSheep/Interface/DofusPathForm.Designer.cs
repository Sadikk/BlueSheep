namespace BlueSheep.Interface
{
    partial class DofusPathForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DofusPathForm));
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.FolderBrowserDialog_DofusPath = new System.Windows.Forms.FolderBrowserDialog();
            this.sadikLabel1 = new BlueSheep.Interface.SadikLabel();
            this.BtValider = new BlueSheep.Interface.SadikButton();
            this.SuspendLayout();
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(14, 72);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(308, 20);
            this.TextBox1.TabIndex = 2;
            // 
            // sadikLabel1
            // 
            this.sadikLabel1.AutoSize = true;
            this.sadikLabel1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel1.Location = new System.Drawing.Point(20, 56);
            this.sadikLabel1.Name = "sadikLabel1";
            this.sadikLabel1.Size = new System.Drawing.Size(299, 13);
            this.sadikLabel1.TabIndex = 3;
            this.sadikLabel1.Text = "Veuillez sélectionner le chemin du dossier Dofus2 :";
            // 
            // BtValider
            // 
            this.BtValider.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.BtValider.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtValider.Font = new System.Drawing.Font("Verdana", 8F);
            this.BtValider.Image = null;
            this.BtValider.Location = new System.Drawing.Point(14, 99);
            this.BtValider.Name = "BtValider";
            this.BtValider.RoundedCorners = true;
            this.BtValider.Size = new System.Drawing.Size(308, 26);
            this.BtValider.TabIndex = 4;
            this.BtValider.Text = "Parcourir";
            this.BtValider.Click += new System.EventHandler(this.BtValider_Click);
            // 
            // DofusPathForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 155);
            this.Controls.Add(this.BtValider);
            this.Controls.Add(this.sadikLabel1);
            this.Controls.Add(this.TextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DofusPathForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choix du dossier Dofus 2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog_DofusPath;
        private SadikLabel sadikLabel1;
        private SadikButton BtValider;
    }
}