namespace BlueSheep.Interface
{
    partial class PathChoiceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PathChoiceForm));
            this.AddBt = new BlueSheep.Interface.SadikButton();
            this.DelBt = new BlueSheep.Interface.SadikButton();
            this.LoadBt = new BlueSheep.Interface.SadikButton();
            this.FilesList = new System.Windows.Forms.ListView();
            this.Nom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Auteur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // AddBt
            // 
            this.AddBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.AddBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.AddBt.Image = null;
            this.AddBt.Location = new System.Drawing.Point(13, 61);
            this.AddBt.Name = "AddBt";
            this.AddBt.RoundedCorners = false;
            this.AddBt.Size = new System.Drawing.Size(133, 26);
            this.AddBt.TabIndex = 0;
            this.AddBt.Text = "Ajouter";
            this.AddBt.Click += new System.EventHandler(this.AddBt_Click);
            // 
            // DelBt
            // 
            this.DelBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.DelBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.DelBt.Image = null;
            this.DelBt.Location = new System.Drawing.Point(152, 60);
            this.DelBt.Name = "DelBt";
            this.DelBt.RoundedCorners = false;
            this.DelBt.Size = new System.Drawing.Size(131, 26);
            this.DelBt.TabIndex = 1;
            this.DelBt.Text = "Supprimer";
            this.DelBt.Click += new System.EventHandler(this.DelBt_Click);
            // 
            // LoadBt
            // 
            this.LoadBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.LoadBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LoadBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.LoadBt.Image = null;
            this.LoadBt.Location = new System.Drawing.Point(289, 60);
            this.LoadBt.Name = "LoadBt";
            this.LoadBt.RoundedCorners = false;
            this.LoadBt.Size = new System.Drawing.Size(236, 26);
            this.LoadBt.TabIndex = 2;
            this.LoadBt.Text = "Charger";
            this.LoadBt.Click += new System.EventHandler(this.LoadBt_Click);
            // 
            // FilesList
            // 
            this.FilesList.BackColor = System.Drawing.SystemColors.Window;
            this.FilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nom,
            this.Location,
            this.Type,
            this.Version,
            this.Auteur,
            this.Path});
            this.FilesList.FullRowSelect = true;
            this.FilesList.HideSelection = false;
            this.FilesList.Location = new System.Drawing.Point(13, 93);
            this.FilesList.MultiSelect = false;
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(512, 351);
            this.FilesList.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.FilesList.TabIndex = 5;
            this.FilesList.UseCompatibleStateImageBehavior = false;
            this.FilesList.View = System.Windows.Forms.View.Details;
            this.FilesList.DoubleClick += new System.EventHandler(this.LoadBt_Click);
            // 
            // Nom
            // 
            this.Nom.Text = "Nom";
            this.Nom.Width = 133;
            // 
            // Location
            // 
            this.Location.Text = "Location";
            this.Location.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Location.Width = 69;
            // 
            // Type
            // 
            this.Type.Text = "Type";
            this.Type.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Type.Width = 81;
            // 
            // Version
            // 
            this.Version.Text = "Version";
            this.Version.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Version.Width = 53;
            // 
            // Auteur
            // 
            this.Auteur.Text = "Auteur";
            this.Auteur.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Auteur.Width = 111;
            // 
            // Path
            // 
            this.Path.Text = "Path";
            this.Path.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Choisissez le trajet à ajouter";
            this.openFileDialog1.Filter = "Fichiers texte|*.txt";
            // 
            // PathChoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 450);
            this.Controls.Add(this.FilesList);
            this.Controls.Add(this.LoadBt);
            this.Controls.Add(this.DelBt);
            this.Controls.Add(this.AddBt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PathChoiceForm";
            this.Text = "Gestion des trajets";
            this.Load += new System.EventHandler(this.PathChoiceForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SadikButton AddBt;
        private SadikButton DelBt;
        private SadikButton LoadBt;
        internal System.Windows.Forms.ColumnHeader Nom;
        internal System.Windows.Forms.ColumnHeader Location;
        internal System.Windows.Forms.ColumnHeader Type;
        internal System.Windows.Forms.ColumnHeader Version;
        internal System.Windows.Forms.ColumnHeader Auteur;
        internal System.Windows.Forms.ColumnHeader Path;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView FilesList;

    }
}