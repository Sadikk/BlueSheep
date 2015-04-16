namespace BlueSheep.Interface
{
    partial class IAChoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IAChoice));
            this.FilesList = new System.Windows.Forms.ListView();
            this.Nom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Classe = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Version = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Path = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AddBt = new BlueSheep.Interface.SadikButton();
            this.DelBt = new BlueSheep.Interface.SadikButton();
            this.LoadBt = new BlueSheep.Interface.SadikButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FilesList
            // 
            this.FilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nom,
            this.Classe,
            this.Version,
            this.Author,
            this.Description,
            this.Path});
            this.FilesList.FullRowSelect = true;
            this.FilesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FilesList.HideSelection = false;
            this.FilesList.Location = new System.Drawing.Point(11, 88);
            this.FilesList.MultiSelect = false;
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(466, 344);
            this.FilesList.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.FilesList.TabIndex = 1;
            this.FilesList.UseCompatibleStateImageBehavior = false;
            this.FilesList.View = System.Windows.Forms.View.Details;
            this.FilesList.DoubleClick += new System.EventHandler(this.LoadBt_Click);
            // 
            // Nom
            // 
            this.Nom.Text = "Nom";
            this.Nom.Width = 100;
            // 
            // Classe
            // 
            this.Classe.DisplayIndex = 2;
            this.Classe.Text = "Classe";
            this.Classe.Width = 111;
            // 
            // Version
            // 
            this.Version.DisplayIndex = 3;
            this.Version.Text = "Version";
            this.Version.Width = 53;
            // 
            // Author
            // 
            this.Author.DisplayIndex = 1;
            this.Author.Text = "Auteur";
            this.Author.Width = 112;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 200;
            // 
            // Path
            // 
            this.Path.Text = "Path";
            // 
            // AddBt
            // 
            this.AddBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.AddBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.AddBt.Image = null;
            this.AddBt.Location = new System.Drawing.Point(12, 55);
            this.AddBt.Name = "AddBt";
            this.AddBt.RoundedCorners = false;
            this.AddBt.Size = new System.Drawing.Size(115, 26);
            this.AddBt.TabIndex = 2;
            this.AddBt.Text = "Ajouter";
            this.AddBt.Click += new System.EventHandler(this.AddBt_Click);
            // 
            // DelBt
            // 
            this.DelBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.DelBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.DelBt.Image = null;
            this.DelBt.Location = new System.Drawing.Point(133, 55);
            this.DelBt.Name = "DelBt";
            this.DelBt.RoundedCorners = false;
            this.DelBt.Size = new System.Drawing.Size(117, 26);
            this.DelBt.TabIndex = 3;
            this.DelBt.Text = "Supprimer";
            this.DelBt.Click += new System.EventHandler(this.DelBt_Click);
            // 
            // LoadBt
            // 
            this.LoadBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.LoadBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LoadBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.LoadBt.Image = null;
            this.LoadBt.Location = new System.Drawing.Point(256, 55);
            this.LoadBt.Name = "LoadBt";
            this.LoadBt.RoundedCorners = false;
            this.LoadBt.Size = new System.Drawing.Size(221, 26);
            this.LoadBt.TabIndex = 4;
            this.LoadBt.Text = "Charger";
            this.LoadBt.Click += new System.EventHandler(this.LoadBt_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // IAChoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 441);
            this.Controls.Add(this.LoadBt);
            this.Controls.Add(this.DelBt);
            this.Controls.Add(this.AddBt);
            this.Controls.Add(this.FilesList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IAChoice";
            this.Text = "Choix de l\'IA";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView FilesList;
        internal System.Windows.Forms.ColumnHeader Nom;
        internal System.Windows.Forms.ColumnHeader Author;
        internal System.Windows.Forms.ColumnHeader Classe;
        internal System.Windows.Forms.ColumnHeader Version;
        internal System.Windows.Forms.ColumnHeader Path;
        private SadikButton AddBt;
        private SadikButton DelBt;
        private SadikButton LoadBt;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColumnHeader Description;
    }
}