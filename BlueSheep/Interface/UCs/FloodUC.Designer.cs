namespace BlueSheep.Interface.UCs
{
    partial class FloodUC
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
            this.sadikTabControl4 = new BlueSheep.Interface.SadikTabControl();
            this.tpNormal = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.PrivateExitBox = new BlueSheep.Interface.SadikCheckbox();
            this.PrivateEnterBox = new BlueSheep.Interface.SadikCheckbox();
            this.GeneralBox = new BlueSheep.Interface.SadikCheckbox();
            this.RecrutementBox = new BlueSheep.Interface.SadikCheckbox();
            this.CommerceBox = new BlueSheep.Interface.SadikCheckbox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.StartStopFloodingBox = new BlueSheep.Interface.SadikCheckbox();
            this.FloodContentRbox = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.sadikLabel10 = new BlueSheep.Interface.SadikLabel();
            this.NUDFlood = new System.Windows.Forms.NumericUpDown();
            this.IsRandomingNumberBox = new BlueSheep.Interface.SadikCheckbox();
            this.IsRandomingSmileyBox = new BlueSheep.Interface.SadikCheckbox();
            this.tpAvancé = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.PlayerListLb = new System.Windows.Forms.ListBox();
            this.ClearListeBt = new BlueSheep.Interface.SadikButton();
            this.RemovePlayerBt = new BlueSheep.Interface.SadikButton();
            this.IsMemoryCheck = new BlueSheep.Interface.SadikCheckbox();
            this.FloodPlayersBt = new BlueSheep.Interface.SadikButton();
            this.sadikTabControl4.SuspendLayout();
            this.tpNormal.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDFlood)).BeginInit();
            this.tpAvancé.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // sadikTabControl4
            // 
            this.sadikTabControl4.Controls.Add(this.tpNormal);
            this.sadikTabControl4.Controls.Add(this.tpAvancé);
            this.sadikTabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sadikTabControl4.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikTabControl4.ItemSize = new System.Drawing.Size(0, 30);
            this.sadikTabControl4.Location = new System.Drawing.Point(0, 0);
            this.sadikTabControl4.Name = "sadikTabControl4";
            this.sadikTabControl4.SelectedIndex = 0;
            this.sadikTabControl4.Size = new System.Drawing.Size(484, 305);
            this.sadikTabControl4.TabIndex = 1;
            // 
            // tpNormal
            // 
            this.tpNormal.BackColor = System.Drawing.Color.White;
            this.tpNormal.Controls.Add(this.groupBox7);
            this.tpNormal.Controls.Add(this.groupBox6);
            this.tpNormal.Controls.Add(this.groupBox5);
            this.tpNormal.Location = new System.Drawing.Point(4, 34);
            this.tpNormal.Name = "tpNormal";
            this.tpNormal.Padding = new System.Windows.Forms.Padding(3);
            this.tpNormal.Size = new System.Drawing.Size(476, 267);
            this.tpNormal.TabIndex = 0;
            this.tpNormal.Text = "Normal";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.PrivateExitBox);
            this.groupBox7.Controls.Add(this.PrivateEnterBox);
            this.groupBox7.Controls.Add(this.GeneralBox);
            this.groupBox7.Controls.Add(this.RecrutementBox);
            this.groupBox7.Controls.Add(this.CommerceBox);
            this.groupBox7.Location = new System.Drawing.Point(298, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(159, 150);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Canaux";
            // 
            // PrivateExitBox
            // 
            this.PrivateExitBox.Checked = false;
            this.PrivateExitBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.PrivateExitBox.Location = new System.Drawing.Point(6, 123);
            this.PrivateExitBox.Name = "PrivateExitBox";
            this.PrivateExitBox.Size = new System.Drawing.Size(140, 20);
            this.PrivateExitBox.TabIndex = 4;
            this.PrivateExitBox.Text = "Privé (Sortie)";
            // 
            // PrivateEnterBox
            // 
            this.PrivateEnterBox.Checked = false;
            this.PrivateEnterBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.PrivateEnterBox.Location = new System.Drawing.Point(6, 97);
            this.PrivateEnterBox.Name = "PrivateEnterBox";
            this.PrivateEnterBox.Size = new System.Drawing.Size(140, 20);
            this.PrivateEnterBox.TabIndex = 3;
            this.PrivateEnterBox.Text = "Privé (Entrée)";
            // 
            // GeneralBox
            // 
            this.GeneralBox.Checked = false;
            this.GeneralBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.GeneralBox.Location = new System.Drawing.Point(6, 71);
            this.GeneralBox.Name = "GeneralBox";
            this.GeneralBox.Size = new System.Drawing.Size(140, 20);
            this.GeneralBox.TabIndex = 2;
            this.GeneralBox.Text = "Général";
            // 
            // RecrutementBox
            // 
            this.RecrutementBox.Checked = false;
            this.RecrutementBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.RecrutementBox.Location = new System.Drawing.Point(6, 45);
            this.RecrutementBox.Name = "RecrutementBox";
            this.RecrutementBox.Size = new System.Drawing.Size(140, 20);
            this.RecrutementBox.TabIndex = 1;
            this.RecrutementBox.Text = "Recrutement";
            // 
            // CommerceBox
            // 
            this.CommerceBox.Checked = false;
            this.CommerceBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.CommerceBox.Location = new System.Drawing.Point(6, 19);
            this.CommerceBox.Name = "CommerceBox";
            this.CommerceBox.Size = new System.Drawing.Size(140, 20);
            this.CommerceBox.TabIndex = 0;
            this.CommerceBox.Text = "Commerce";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.StartStopFloodingBox);
            this.groupBox6.Controls.Add(this.FloodContentRbox);
            this.groupBox6.Location = new System.Drawing.Point(24, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(246, 150);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Contenu";
            // 
            // StartStopFloodingBox
            // 
            this.StartStopFloodingBox.Checked = false;
            this.StartStopFloodingBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.StartStopFloodingBox.Location = new System.Drawing.Point(6, 128);
            this.StartStopFloodingBox.Name = "StartStopFloodingBox";
            this.StartStopFloodingBox.Size = new System.Drawing.Size(190, 15);
            this.StartStopFloodingBox.TabIndex = 1;
            this.StartStopFloodingBox.Text = "Lancer/Arrêter le flood.";
            this.StartStopFloodingBox.CheckedChanged += new BlueSheep.Interface.SadikCheckbox.CheckedChangedEventHandler(this.StartStopFloodingBox_CheckedChanged);
            // 
            // FloodContentRbox
            // 
            this.FloodContentRbox.Location = new System.Drawing.Point(6, 19);
            this.FloodContentRbox.MaxLength = 256;
            this.FloodContentRbox.Name = "FloodContentRbox";
            this.FloodContentRbox.Size = new System.Drawing.Size(212, 103);
            this.FloodContentRbox.TabIndex = 0;
            this.FloodContentRbox.Text = "";
            this.FloodContentRbox.TextChanged += new System.EventHandler(this.FloodContentRbox_TextChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.sadikLabel10);
            this.groupBox5.Controls.Add(this.NUDFlood);
            this.groupBox5.Controls.Add(this.IsRandomingNumberBox);
            this.groupBox5.Controls.Add(this.IsRandomingSmileyBox);
            this.groupBox5.Location = new System.Drawing.Point(24, 155);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(414, 95);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Options";
            // 
            // sadikLabel10
            // 
            this.sadikLabel10.AutoSize = true;
            this.sadikLabel10.Font = new System.Drawing.Font("Verdana", 8F);
            this.sadikLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.sadikLabel10.Location = new System.Drawing.Point(105, 73);
            this.sadikLabel10.Name = "sadikLabel10";
            this.sadikLabel10.Size = new System.Drawing.Size(244, 13);
            this.sadikLabel10.TabIndex = 3;
            this.sadikLabel10.Text = "Intervalle entre deux phrases (secondes)";
            // 
            // NUDFlood
            // 
            this.NUDFlood.Location = new System.Drawing.Point(29, 71);
            this.NUDFlood.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.NUDFlood.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUDFlood.Name = "NUDFlood";
            this.NUDFlood.Size = new System.Drawing.Size(70, 20);
            this.NUDFlood.TabIndex = 2;
            this.NUDFlood.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // IsRandomingNumberBox
            // 
            this.IsRandomingNumberBox.Checked = false;
            this.IsRandomingNumberBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.IsRandomingNumberBox.Location = new System.Drawing.Point(6, 45);
            this.IsRandomingNumberBox.Name = "IsRandomingNumberBox";
            this.IsRandomingNumberBox.Size = new System.Drawing.Size(354, 20);
            this.IsRandomingNumberBox.TabIndex = 1;
            this.IsRandomingNumberBox.Text = "Ajouter un nombre aléatoire à la fin du message";
            // 
            // IsRandomingSmileyBox
            // 
            this.IsRandomingSmileyBox.Checked = false;
            this.IsRandomingSmileyBox.Font = new System.Drawing.Font("Verdana", 8F);
            this.IsRandomingSmileyBox.Location = new System.Drawing.Point(6, 19);
            this.IsRandomingSmileyBox.Name = "IsRandomingSmileyBox";
            this.IsRandomingSmileyBox.Size = new System.Drawing.Size(343, 20);
            this.IsRandomingSmileyBox.TabIndex = 0;
            this.IsRandomingSmileyBox.Text = "Ajouter un smiley aléatoire à la fin du message";
            // 
            // tpAvancé
            // 
            this.tpAvancé.BackColor = System.Drawing.Color.White;
            this.tpAvancé.Controls.Add(this.groupBox11);
            this.tpAvancé.Controls.Add(this.FloodPlayersBt);
            this.tpAvancé.Location = new System.Drawing.Point(4, 34);
            this.tpAvancé.Name = "tpAvancé";
            this.tpAvancé.Padding = new System.Windows.Forms.Padding(3);
            this.tpAvancé.Size = new System.Drawing.Size(476, 267);
            this.tpAvancé.TabIndex = 1;
            this.tpAvancé.Text = "Avancé";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.PlayerListLb);
            this.groupBox11.Controls.Add(this.ClearListeBt);
            this.groupBox11.Controls.Add(this.RemovePlayerBt);
            this.groupBox11.Controls.Add(this.IsMemoryCheck);
            this.groupBox11.Location = new System.Drawing.Point(21, 17);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(430, 180);
            this.groupBox11.TabIndex = 5;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Contrôles";
            // 
            // PlayerListLb
            // 
            this.PlayerListLb.FormattingEnabled = true;
            this.PlayerListLb.Location = new System.Drawing.Point(6, 47);
            this.PlayerListLb.Name = "PlayerListLb";
            this.PlayerListLb.Size = new System.Drawing.Size(205, 121);
            this.PlayerListLb.TabIndex = 4;
            // 
            // ClearListeBt
            // 
            this.ClearListeBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.ClearListeBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClearListeBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.ClearListeBt.Image = null;
            this.ClearListeBt.Location = new System.Drawing.Point(262, 108);
            this.ClearListeBt.Name = "ClearListeBt";
            this.ClearListeBt.RoundedCorners = false;
            this.ClearListeBt.Size = new System.Drawing.Size(131, 34);
            this.ClearListeBt.TabIndex = 3;
            this.ClearListeBt.Text = "Supprimer tout";
            this.ClearListeBt.Click += new System.EventHandler(this.ClearListeBt_Click);
            // 
            // RemovePlayerBt
            // 
            this.RemovePlayerBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.RemovePlayerBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemovePlayerBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.RemovePlayerBt.Image = null;
            this.RemovePlayerBt.Location = new System.Drawing.Point(262, 59);
            this.RemovePlayerBt.Name = "RemovePlayerBt";
            this.RemovePlayerBt.RoundedCorners = false;
            this.RemovePlayerBt.Size = new System.Drawing.Size(131, 34);
            this.RemovePlayerBt.TabIndex = 2;
            this.RemovePlayerBt.Text = "Supprimer le joueur";
            this.RemovePlayerBt.Click += new System.EventHandler(this.RemovePlayerBt_Click);
            // 
            // IsMemoryCheck
            // 
            this.IsMemoryCheck.Checked = false;
            this.IsMemoryCheck.Font = new System.Drawing.Font("Verdana", 8F);
            this.IsMemoryCheck.Location = new System.Drawing.Point(99, 19);
            this.IsMemoryCheck.Name = "IsMemoryCheck";
            this.IsMemoryCheck.Size = new System.Drawing.Size(229, 22);
            this.IsMemoryCheck.TabIndex = 0;
            this.IsMemoryCheck.Text = "Activer la mémorisation intelligente";
            // 
            // FloodPlayersBt
            // 
            this.FloodPlayersBt.ButtonStyle = BlueSheep.Interface.SadikButton.Style.Blue;
            this.FloodPlayersBt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FloodPlayersBt.Font = new System.Drawing.Font("Verdana", 8F);
            this.FloodPlayersBt.Image = null;
            this.FloodPlayersBt.Location = new System.Drawing.Point(81, 202);
            this.FloodPlayersBt.Name = "FloodPlayersBt";
            this.FloodPlayersBt.RoundedCorners = false;
            this.FloodPlayersBt.Size = new System.Drawing.Size(333, 47);
            this.FloodPlayersBt.TabIndex = 4;
            this.FloodPlayersBt.Text = "Flooder les joueurs";
            this.FloodPlayersBt.Click += new System.EventHandler(this.FloodPlayersBt_Click);
            // 
            // FloodUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sadikTabControl4);
            this.Name = "FloodUC";
            this.Size = new System.Drawing.Size(484, 305);
            this.sadikTabControl4.ResumeLayout(false);
            this.tpNormal.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDFlood)).EndInit();
            this.tpAvancé.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SadikTabControl sadikTabControl4;
        private System.Windows.Forms.TabPage tpNormal;
        private System.Windows.Forms.GroupBox groupBox7;
        private SadikCheckbox PrivateExitBox;
        public SadikCheckbox PrivateEnterBox;
        private SadikCheckbox GeneralBox;
        private SadikCheckbox RecrutementBox;
        private SadikCheckbox CommerceBox;
        private System.Windows.Forms.GroupBox groupBox6;
        public SadikCheckbox StartStopFloodingBox;
        public System.Windows.Forms.RichTextBox FloodContentRbox;
        private System.Windows.Forms.GroupBox groupBox5;
        private SadikLabel sadikLabel10;
        private System.Windows.Forms.NumericUpDown NUDFlood;
        public SadikCheckbox IsRandomingNumberBox;
        public SadikCheckbox IsRandomingSmileyBox;
        private System.Windows.Forms.TabPage tpAvancé;
        private System.Windows.Forms.GroupBox groupBox11;
        public System.Windows.Forms.ListBox PlayerListLb;
        private SadikButton ClearListeBt;
        private SadikButton RemovePlayerBt;
        public SadikCheckbox IsMemoryCheck;
        private SadikButton FloodPlayersBt;
    }
}
