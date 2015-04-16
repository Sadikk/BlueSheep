namespace BlueSheep.Interface.UCs
{
    partial class RegenUC
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
            this.LVItems = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // LVItems
            // 
            this.LVItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LVItems.Location = new System.Drawing.Point(0, 0);
            this.LVItems.Name = "LVItems";
            this.LVItems.Size = new System.Drawing.Size(410, 290);
            this.LVItems.TabIndex = 0;
            this.LVItems.UseCompatibleStateImageBehavior = false;
            this.LVItems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LVGestItems_TouchPressed);
            // 
            // RegenUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LVItems);
            this.Name = "RegenUC";
            this.Size = new System.Drawing.Size(410, 290);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView LVItems;

    }
}
