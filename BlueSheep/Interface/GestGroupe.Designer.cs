namespace BlueSheep.Interface
{
    partial class GestGroupe
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestGroupe));
            this.sadikTabControl1 = new BlueSheep.Interface.SadikTabControl();
            this.LaunchPage = new System.Windows.Forms.TabPage();
            this.DelBt = new BlueSheep.Interface.SadikButton();
            this.listViewGroups = new System.Windows.Forms.ListView();
            this.LaunchGroupsSelecBt = new BlueSheep.Interface.SadikButton();
            this.AddPage = new System.Windows.Forms.TabPage();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.listViewAccounts = new System.Windows.Forms.ListView();
            this.AddBt = new BlueSheep.Interface.SadikButton();
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
            this.sadikTabControl1.Size = new System.Drawing.Size(282, 244);
            this.sadikTabControl1.TabIndex = 13;
            // 
            // LaunchPage
            // 
            this.LaunchPage.BackColor = System.Drawing.Color.White;
            this.LaunchPage.Controls.Add(this.DelBt);
            this.LaunchPage.Controls.Add(this.listViewGroups);
            this.LaunchPage.Controls.Add(this.LaunchGroupsSelecBt);
            this.LaunchPage.Location = new System.Drawing.Point(4, 34);
            this.LaunchPage.Name = "LaunchPage";
            this.LaunchPage.Padding = new System.Windows.Forms.Padding(3);
            this.LaunchPage.Size = new System.Drawing.Size(274, 206);
            this.LaunchPage.TabIndex = 0;
            this.LaunchPage.Text = "Lancer";
            // 
            // DelBt
            // 
            this.DelBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.DelBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DelBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.DelBt.Image = null;
            this.DelBt.Location = new System.Drawing.Point(23, 171);
            this.DelBt.Name = "DelBt";
            this.DelBt.RoundedCorners = false;
            this.DelBt.Size = new System.Drawing.Size(85, 26);
            this.DelBt.TabIndex = 14;
            this.DelBt.Text = "Supprimer";
            this.DelBt.Click += new System.EventHandler(this.DelBt_Click);
            // 
            // listViewGroups
            // 
            this.listViewGroups.Location = new System.Drawing.Point(23, 6);
            this.listViewGroups.Name = "listViewGroups";
            this.listViewGroups.Size = new System.Drawing.Size(228, 159);
            this.listViewGroups.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewGroups.TabIndex = 13;
            this.listViewGroups.UseCompatibleStateImageBehavior = false;
            this.listViewGroups.View = System.Windows.Forms.View.Details;
            // 
            // LaunchGroupsSelecBt
            // 
            this.LaunchGroupsSelecBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.LaunchGroupsSelecBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LaunchGroupsSelecBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.LaunchGroupsSelecBt.Image = null;
            this.LaunchGroupsSelecBt.Location = new System.Drawing.Point(114, 171);
            this.LaunchGroupsSelecBt.Name = "LaunchGroupsSelecBt";
            this.LaunchGroupsSelecBt.RoundedCorners = false;
            this.LaunchGroupsSelecBt.Size = new System.Drawing.Size(137, 26);
            this.LaunchGroupsSelecBt.TabIndex = 12;
            this.LaunchGroupsSelecBt.Text = "Lancer les groupes";
            this.LaunchGroupsSelecBt.Click += new System.EventHandler(this.LaunchGroupsSelecBt_Click);
            // 
            // AddPage
            // 
            this.AddPage.BackColor = System.Drawing.Color.White;
            this.AddPage.Controls.Add(this.NameBox);
            this.AddPage.Controls.Add(this.listViewAccounts);
            this.AddPage.Controls.Add(this.AddBt);
            this.AddPage.Location = new System.Drawing.Point(4, 34);
            this.AddPage.Name = "AddPage";
            this.AddPage.Padding = new System.Windows.Forms.Padding(3);
            this.AddPage.Size = new System.Drawing.Size(274, 206);
            this.AddPage.TabIndex = 1;
            this.AddPage.Text = "Créer";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(41, 164);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(184, 20);
            this.NameBox.TabIndex = 18;
            // 
            // listViewAccounts
            // 
            this.listViewAccounts.Location = new System.Drawing.Point(22, 6);
            this.listViewAccounts.Name = "listViewAccounts";
            this.listViewAccounts.Size = new System.Drawing.Size(228, 152);
            this.listViewAccounts.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewAccounts.TabIndex = 17;
            this.listViewAccounts.UseCompatibleStateImageBehavior = false;
            this.listViewAccounts.View = System.Windows.Forms.View.Details;
            // 
            // AddBt
            // 
            this.AddBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.AddBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.AddBt.Image = null;
            this.AddBt.Location = new System.Drawing.Point(41, 190);
            this.AddBt.Name = "AddBt";
            this.AddBt.RoundedCorners = false;
            this.AddBt.Size = new System.Drawing.Size(188, 26);
            this.AddBt.TabIndex = 16;
            this.AddBt.Text = "Créer";
            this.AddBt.Click += new System.EventHandler(this.AddBt_Click);
            // 
            // GestGroupe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 324);
            this.Controls.Add(this.sadikTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GestGroupe";
            this.Text = "Gestion des groupes";
            this.sadikTabControl1.ResumeLayout(false);
            this.LaunchPage.ResumeLayout(false);
            this.AddPage.ResumeLayout(false);
            this.AddPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SadikTabControl sadikTabControl1;
        private System.Windows.Forms.TabPage LaunchPage;
        private SadikButton DelBt;
        private System.Windows.Forms.ListView listViewGroups;
        private SadikButton LaunchGroupsSelecBt;
        private System.Windows.Forms.TabPage AddPage;
        private SadikButton AddBt;
        private System.Windows.Forms.ListView listViewAccounts;
        private System.Windows.Forms.TextBox NameBox;
    }
}