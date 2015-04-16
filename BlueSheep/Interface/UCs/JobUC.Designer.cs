namespace BlueSheep.Interface
{
    partial class JobUC
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.sadikTabControl1 = new BlueSheep.Interface.SadikTabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.OpenBagCb = new BlueSheep.Interface.SadikCheckbox();
            this.g = new System.Windows.Forms.DataGridView();
            this.gg = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.sadikTabControl1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.g)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gg)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // sadikTabControl1
            // 
            this.sadikTabControl1.Controls.Add(this.tabPage4);
            this.sadikTabControl1.Controls.Add(this.tabPage5);
            this.sadikTabControl1.Controls.Add(this.tabPage6);
            this.sadikTabControl1.Controls.Add(this.tabPage7);
            this.sadikTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sadikTabControl1.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikTabControl1.ItemSize = new System.Drawing.Size(0, 30);
            this.sadikTabControl1.Location = new System.Drawing.Point(0, 0);
            this.sadikTabControl1.Name = "sadikTabControl1";
            this.sadikTabControl1.SelectedIndex = 0;
            this.sadikTabControl1.Size = new System.Drawing.Size(410, 283);
            this.sadikTabControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(402, 245);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Récolte";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Location = new System.Drawing.Point(4, 34);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(402, 245);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Craft";
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.White;
            this.tabPage6.Location = new System.Drawing.Point(4, 34);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(402, 245);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Stats";
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.Color.White;
            this.tabPage7.Controls.Add(this.OpenBagCb);
            this.tabPage7.Location = new System.Drawing.Point(4, 34);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(402, 245);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "Config";
            // 
            // OpenBagCb
            // 
            this.OpenBagCb.Checked = false;
            this.OpenBagCb.Font = new System.Drawing.Font("Verdana", 8F);
            this.OpenBagCb.Location = new System.Drawing.Point(16, 19);
            this.OpenBagCb.Name = "OpenBagCb";
            this.OpenBagCb.Size = new System.Drawing.Size(304, 20);
            this.OpenBagCb.TabIndex = 0;
            this.OpenBagCb.Text = "Ouvrir automatiquement les sacs de ressources.";
            // 
            // g
            // 
            this.g.BackgroundColor = System.Drawing.Color.LightCyan;
            this.g.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.g.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g.Location = new System.Drawing.Point(0, 0);
            this.g.Name = "g";
            this.g.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.g.Size = new System.Drawing.Size(410, 283);
            this.g.TabIndex = 0;
            // 
            // gg
            // 
            this.gg.BackgroundColor = System.Drawing.Color.LightCyan;
            this.gg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gg.Location = new System.Drawing.Point(3, 3);
            this.gg.Name = "gg";
            this.gg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gg.Size = new System.Drawing.Size(419, 323);
            this.gg.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 329);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Récolte";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.gg);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 329);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Craft";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.White;
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(425, 329);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Stats";
            // 
            // JobUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sadikTabControl1);
            this.Name = "JobUC";
            this.Size = new System.Drawing.Size(410, 283);
            this.sadikTabControl1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.g)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gg)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SadikTabControl sadikTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.DataGridView g;
        public System.Windows.Forms.DataGridView gg;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        public SadikCheckbox OpenBagCb;
    }
}
