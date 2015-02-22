using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlueSheep.Interface.Text;
using BlueSheep.Engine;
using BlueSheep.Engine.Types;
using BlueSheep.Engine.Network;
using Timer = System.Threading.Timer;
using BlueSheep.Core;
using BlueSheep.Common.Types;
using System.Threading;
using BlueSheep.Common.Data;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Engine.Frame;
using BlueSheep.Core.Fight;
using BlueSheep.Core.Map;
using BlueSheep.Core.Inventory;
using BlueSheep.Core.Path;
using BlueSheep.Core.Misc;
using BlueSheep.Core.Npc;
using BlueSheep.Core.Job;
using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Interface.Text.Chat;
using BlueSheep.Common.Data.D2o;
using System.Collections;
using BlueSheep.Engine.Constants;
using System.Windows.Forms.DataVisualization.Charting;
using BlueSheep.Interface.UCs;

namespace BlueSheep.Interface
{
    public partial class AccountUC : UserControl
    {
        /// <summary>
        /// Main UC. TODO : Split this hugggge thing into multiple UC.
        /// </summary>

        #region Fields
        public string AccountName;
        public string AccountPassword;
        private Running m_Running;
        private Thread m_ConnectionThread;
        private Timer m_TimerConnectionThread;
        public string FloodContent;
        private List<Bot> m_Bots = new List<Bot>();
        public string loginstate;
        private DateTime m_NextMeal;
        public bool IsMaster;
        public bool IsSlave;
        public BFight Fight;
        public Map Map;
        public Inventory Inventory;
        public List<Spell> Spells;
        public PathManager Path;
        public Group MyGroup;
        public int MyGroupId;
        public HouseBuy House = null;
        public Npc Npc;
        public Flood Flood;
        public List<Job> Jobs;
        public bool Busy;
        public double serverTimeLag;
        public Gather Gather;
        public List<JobUC> JobsUC;
        public HumanCheck HumanCheck;
        public HeroicUC HeroicUC;
        public GestItemsUC GestItemsUC;
        public bool IsMITM;
        #endregion

        #region Properties
        public bool IsBegun { get; set; }
        public SocketManager SocketManager { get; set; }
        public DateTime NextMeal { get; set; }
        public string Ticket { get; set; }
        public List<Pet> PetsModifiedList { get; set; }
        public List<Pet> petsList { get; set; }
        public List<int> GiftsList { get; set; }
        public InteractiveElement Safe { get; set; }
        //public List<ObjectItem> InventoryItems { get; set; }
        public CharacterBaseInformations CharacterBaseInformations { get; set; }
        public string State { get; set; }
        public short Sequence { get; set; }
        public LatencyFrame LatencyFrame { get; set; }
        public Pods Pods { get; set; }
        public List<ObjectItem> SafeItems { get; set; }
        public Running Running { get; set; }
        public Queue<short> LastPacketID { get; set; }
        public short LastPacket;
        public int MapID { get; set; }
        public CharacterCharacteristicsInformations CharacterStats { get; set; }
        #endregion

        #region Delegates
        private delegate void SetLogsCallback(TextInformation textInformations, int levelVerbose);
        private delegate void ActualizeAccountInformationsCallback();
        private delegate void SetLicenceCallback(bool response, string text);
        private delegate void DelegBar(int Bar, int Max, int value, string text);
        private delegate void DelegListView(ListViewItem delta, ListView gamma);
        private delegate void DelegLabel(string text, Label lab);
        private delegate void DelegBool(bool param1);
        private delegate void DelegGatherPie(Dictionary<string, int> ressources, Dictionary<DateTime,int> xp);
        private delegate bool DelegVerifGroup(List<string> monsters);
        #endregion

        #region Constructeurs

        public AccountUC(string username, string password, bool socket)
        {
            InitializeComponent();
            MonsterTextBox.KeyUp += (s, e) =>
            {
                IntelliSense.AutoCompleteTextBox(MonsterTextBox, lstPopup, IntelliSense.MonstersList, e);
            };
            AccountName = username;
            AccountPassword = password;
            PetsModifiedList = new List<Pet>();
            PrivateExitBox.Hide();
            this.IsMITM = !socket;
            switch (MainForm.ActualMainForm.Lang)
            {
                case "FR":
                    listViewPets.Columns.Add("Nom", 150, HorizontalAlignment.Left);
            listViewPets.Columns.Add("UID", 0, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Nourriture (Quantité)", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Prochain repas", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Caractéristiques", -2, HorizontalAlignment.Left);
            LVItems.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("Nom", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Quantité", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Prix moyen", -2, HorizontalAlignment.Center);
                    break;
                case "EN":
                    listViewPets.Columns.Add("Name", 150, HorizontalAlignment.Left);
            listViewPets.Columns.Add("UID", 0, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Food (Quantity)", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Next meal", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Stats", -2, HorizontalAlignment.Left);
            LVItems.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("Name", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Average Price", -2, HorizontalAlignment.Center);
            VTabAccount.TabPages[1].Text = "Fight";
            VTabAccount.TabPages[2].Text = "Pets";
            VTabAccount.TabPages[3].Text = "Inventory";
            VTabAccount.TabPages[4].Text = "Misc";
            VTabAccount.TabPages[5].Text = "Settings";
            sadikTabControl2.TabPages[0].Text = "Inventory";
            FloodPage.TabPages[0].Text = "Houses";
            sadikLabel2.Text = "% of life";
            GroupBox2.Text = "AI";
            ChoiceIABt.Text = "Choose an AI";
            CreateIABt.Text = "Create an AI";
            NomIA.Text = "No AI";
            sadikLabel5.Text = "Numbers of monsters";
            sadikLabel6.Text = "Levels of monsters";
            checkBoxBegin.Text = "Start feeding";
            toolStripButton1.Text = "Connect/Disconnect";
            PathDownBt.Text = "Path";
            LaunchPathBt.Text = "Launch path";
            LoadPathBt.Text = "Load path";
            StopPathBt.Text = "Stop path";
            break;
                case "ES":
            listViewPets.Columns.Add("Nombre", 150, HorizontalAlignment.Left);
            listViewPets.Columns.Add("UID", 0, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Alimento (cantidad)", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Comida próxima", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Características", -2, HorizontalAlignment.Left);
            LVItems.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("Nombre", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Cantidad", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Tipo", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Precio medio", -2, HorizontalAlignment.Center);
            VTabAccount.TabPages[1].Text = "Combate";
            VTabAccount.TabPages[2].Text = "Mascotas";
            VTabAccount.TabPages[3].Text = "Inventario";
            VTabAccount.TabPages[4].Text = "Miscelánea";
            VTabAccount.TabPages[5].Text = "Parámetros";
            sadikTabControl2.TabPages[0].Text = "Inventario";
            FloodPage.TabPages[0].Text = "Casas";
            sadikLabel2.Text = "% de vida";
            GroupBox2.Text = "IA";
            ChoiceIABt.Text = "Choose an IA";
            CreateIABt.Text = "Create an IA";
            NomIA.Text = "No IA";
            sadikLabel5.Text = "Número de monstruos";
            sadikLabel6.Text = "Niveles de los monstruos";
            checkBoxBegin.Text = "Empezar la ganadería";
            toolStripButton1.Text = "Conectar/Desconectar";
            PathDownBt.Text = "Trayecto";
            LaunchPathBt.Text = "Lanzar el trayecto";
            LoadPathBt.Text = "Cargar un trayecto";
            StopPathBt.Text = "Parar el trayecto";
            break;
                case "PT":
                    listViewPets.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            listViewPets.Columns.Add("UID", 0, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Comida (quantidade)", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Próxima refeição", -2, HorizontalAlignment.Left);
            listViewPets.Columns.Add("Características", -2, HorizontalAlignment.Left);
            LVItems.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItems.Columns.Add("Nome", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Quantidade", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Tipo", -2, HorizontalAlignment.Center);
            LVItems.Columns.Add("Preço médio", -2, HorizontalAlignment.Center);
            VTabAccount.TabPages[1].Text = "Luta";
            VTabAccount.TabPages[2].Text = "Animais";
            VTabAccount.TabPages[3].Text = "Inventário";
            VTabAccount.TabPages[4].Text = "Diverso";
            VTabAccount.TabPages[5].Text = "Parâmetros";
            sadikTabControl2.TabPages[0].Text = "Inventário";
            FloodPage.TabPages[0].Text = "Casas";
            sadikLabel2.Text = "% de vida";
            GroupBox2.Text = "IA";
            ChoiceIABt.Text = "Escolha um IA";
            CreateIABt.Text = "Crie um IA";
            NomIA.Text = "No AI";
            sadikLabel5.Text = "Número de monstros";
            sadikLabel6.Text = "Nível de monstros";
            checkBoxBegin.Text = "Comece a procriação";
            toolStripButton1.Text = "Conecte / desconecte";
            PathDownBt.Text = "Trajecto";
            LaunchPathBt.Text = "Lance a trajecto";
            LoadPathBt.Text = "Carregue (Custo) um trajecto";
            StopPathBt.Text = "Parada trajecto";
            break;
            }
            ComparateurBox.SelectedIndex = 0;
            MonstersRestrictionsView.Columns.Add("Nom", -2);
            MonstersRestrictionsView.Columns.Add("Comparateur", -2);
            MonstersRestrictionsView.Columns.Add("Nombre", -2);
            MonstersRestrictionsView.Columns.Add("Restriction", -2);
            JobsUC = new List<JobUC>();
            NextMeal = new DateTime();
            Ticket = string.Empty;
            PetsModifiedList = null;
            petsList = null;
            Safe = null;
            CharacterBaseInformations = null;
            Sequence = 0;
            LatencyFrame = null;
            Pods = null;
            SafeItems = new List<ObjectItem>();
            LastPacketID = new Queue<short>();
            Running = null;
            //Fight = new BFight(this);
            Fight = null;
            Map = new Map(this);
            Inventory = new Inventory(this);
            Spells = new List<Spell>();
            Npc = new Npc(this);
            Flood = new Core.Misc.Flood(this);
            Jobs = new List<Job>();
            Gather = new Gather(this);

            //Heroic mode
            HeroicUC = new HeroicUC(this);
            FloodPage.TabPages[3].Controls.Add(HeroicUC);
            HeroicUC.Show();

            //Items management
            GestItemsUC = new GestItemsUC(this);
            tabPage9.Controls.Add(GestItemsUC);
            GestItemsUC.Show();

            
        }

        public AccountUC()
        {
            InitializeComponent();
        }

#endregion

        #region Methodes d'interfaces


        private void Form_Closed(object sender, EventArgs e)
        {
            this.SocketManager.DisconnectFromGUI();
            if (IsMITM)
            {
                this.SocketManager.DisconnectServer();
            }
        }

        public void InitMITM()
        {
            this.SocketManager = new SocketManager(this);
            this.SocketManager.ListenDofus();
        }
        public void Log(TextInformation text,int levelVerbose)
        {
            if (this.IsDisposed == true)
                return;
            if ((int)NUDVerbose.Value < levelVerbose)
                return;
            if (LogConsole.InvokeRequired)
                Invoke(new SetLogsCallback(Log), text, levelVerbose);
            else
            {
                if (MainForm.ActualMainForm.Lang != "FR")
                {
                    text.Text = BlueSheep.Engine.Constants.Translate.GetTranslation(text.Text, MainForm.ActualMainForm.Lang);
                }
                text.Text = "[" + DateTime.Now.ToShortTimeString() +
                    "] (" + text.Category + ") " + text.Text;

                //using (StreamWriter fileWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Logs.txt", true))
                //    fileWriter.WriteLine(text.Text);

                int startIndex = LogConsole.TextLength;

                LogConsole.AppendText(text.Text + "\r\n");
                LogConsole.Select(LogConsole.Text.Length, 0);
                LogConsole.ScrollToCaret();

                LogConsole.SelectionStart = startIndex;
                LogConsole.SelectionLength = text.Text.Length;
                LogConsole.SelectionColor = text.Color;
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            //Delete an item from inventory
            if (StatusLb.Text == "Combat" && StatusLb.Text == "Fighting")
            {
                Log(new ErrorTextInformation("Impossible de supprimer un objet en combat ^^"), 0);
            }
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    Inventory.DeleteItem(Convert.ToInt32(LVItems.Items[i].SubItems[1].Text), Convert.ToInt32(LVItems.Items[i].SubItems[3].Text));
                }
            }
        }

        private void DropItems_Click(object sender, EventArgs e)
        {
            //Drop an item from inventory
            if (StatusLb.Text == "Combat" && StatusLb.Text == "Fighting")
            {
                Log(new ErrorTextInformation("Impossible de jeter un objet en combat ^^"), 0);
            }
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    Inventory.DropItem(Convert.ToInt32(LVItems.Items[i].SubItems[1].Text), Convert.ToInt32(LVItems.Items[i].SubItems[3].Text));
                }
            }
        }

        private void sadikButton1_Click(object sender, EventArgs e)
        {
            //Use an item from inventory
            if (StatusLb.Text == "Combat" && StatusLb.Text == "Fighting")
            {
                Log(new ErrorTextInformation("Impossible d'utiliser un objet en combat ^^"), 0);
            }
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    Inventory.UseItem(Convert.ToInt32(LVItems.Items[i].SubItems[1].Text));
                }
            }
        }

        private void sadikButton2_Click(object sender, EventArgs e)
        {
            //Equip an item from inventory
            if (StatusLb.Text == "Combat" && StatusLb.Text == "Fighting")
            {
                Log(new ErrorTextInformation("Impossible d'équiper un item en combat ^^"), 0);
            }
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    Inventory.EquipItem(Convert.ToInt32(LVItems.Items[i].SubItems[1].Text));
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.SocketManager.Connected() == true)
            {
                this.SocketManager.DisconnectFromGUI();
            }
            else
            {
                m_ConnectionThread = new Thread(Connect);
                m_ConnectionThread.Start();
            }
        }

        private void checkBoxBegin_CheckedChanged(object sender)
        {
            if (checkBoxBegin.Checked == true)
            {
                StartFeeding();
            }
            else
            {
                Log(new BotTextInformation("L'élevage va être stoppé"),3);
                Running = null;
            }
        }

        public void ModifBar(int Bar, int Max, int value, string text)
        {
            if (VitaBar.InvokeRequired)
                Invoke(new DelegBar(ModifBar), Bar, Max, value, text);
            else
            {
                switch (Bar)
                {
                    case 1:
                        XpBar.Maximum = Max;
                        XpBar.Value = value;
                        XpBar.Text = text;
                        break;
                    case 2:
                        VitaBar.Maximum = Max;
                        VitaBar.Value = value;
                        VitaBar.Text = text;
                        break;
                    case 3:
                        PodsBar.Maximum = Max;
                        PodsBar.Value = value;
                        PodsBar.Text = text;
                        break;
                    case 4:
                        KamasLabel.Text = text + " kamas";
                        break;
                    case 5:
                        PosLabel.Text = text;
                        break;
                    case 6:
                        if (MainForm.ActualMainForm.Lang != "FR")
                            text = Engine.Constants.Translate.GetTranslation(text, MainForm.ActualMainForm.Lang);
                        StatusLb.Text = text;
                        break;
                    case 7:
                        this.ParentForm.Text = text;
                        break;
                    case 8:
                        this.LevelLb.Text = text;
                        break;
                    case 9:
                        this.SubcribeLb.Text = text;
                        break;
                }
            }
        }

        public void AddItem(ListViewItem li, ListView list)
        {
            //this.BeginInvoke(new MethodInvoker(LVItems.Items.Add),li);
            if (list.InvokeRequired == true)
                Invoke(new DelegListView(AddItem), li,list);
            else
                list.Items.Add(li);
        }

        private void LoadPathBt_Click(object sender, EventArgs e)
        {
            PathChoiceForm frm = new PathChoiceForm(this);
            frm.Show();
        }

        private void LaunchPathBt_Click(object sender, EventArgs e)
        {
            if (Path != null)
            {
                Path.Stop = false;
                Log(new BotTextInformation("Lancement du trajet"),1);
                Path.ParsePath();
            }
            else
                Log(new ErrorTextInformation("Aucun trajet chargé"),3);
        }

        private void StopPathBt_Click(object sender, EventArgs e)
        {
            if (Path != null)
            {
                Path = null;
                PathDownBt.Text = "Trajet";
                this.Log(new BotTextInformation("Trajet déchargé"),1);
            }
        }

        private void CreateIABt_Click(object sender, EventArgs e)
        {
            IACreator frm = new IACreator(this);
            frm.Show();
        }

        private void ChoiceIABt_Click(object sender, EventArgs e)
        {
            IAChoice frm = new IAChoice(this);
            frm.ShowDialog();
        }

        private void StartWaitingBt_Click(object sender, EventArgs e)
        {
            House = new HouseBuy(this);
            Log(new BotTextInformation("En attente de la mise en vente d'une maison..."),1);
        }

        private void ParcourirBt_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                SearcherLogBox.Text = saveFileDialog1.FileName;
        }

        private void StartStopFloodingBox_CheckedChanged(object sender)
        {
            if (StartStopFloodingBox.Checked == false)
            {
                Flood.stop = true;
                Log(new BotTextInformation("Flood arrêté"),1);
                return;
            }
            Log(new BotTextInformation("Flood activé"),1);
            if (CommerceBox.Checked)
                Flood.StartFlooding(5, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
            if (RecrutementBox.Checked)
                Flood.StartFlooding(6, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
            if (GeneralBox.Checked)
                Flood.StartFlooding(0, IsRandomingSmileyBox.Checked, IsRandomingNumberBox.Checked, FloodContentRbox.Text, (int)NUDFlood.Value);
        }

        public void ActualizeInventory()
        {
            this.BeginInvoke(new MethodInvoker(LVItems.Items.Clear));
            //this.BeginInvoke(new Method(LVItems.Clear();
            foreach (Core.Inventory.Item i in Inventory.Items)
            {
                string[] row1 = { i.GID.ToString(), i.UID.ToString(), i.Name, i.Quantity.ToString(), i.Type.ToString(), i.Price.ToString() };
                System.Windows.Forms.ListViewItem li = new System.Windows.Forms.ListViewItem(row1);
                li.ToolTipText = i.Description;
                AddItem(li, LVItems);
            }
        }

        public void ActualizeFamis()
        {
            if (labelNextMeal.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(ActualizeFamis));
                return;
            }
            if (NextMeal.Year != 1)
                Invoke(new DelegLabel(ModLabel), "Prochain repas à : " + NextMeal.ToShortTimeString(), labelNextMeal);
            else
                Invoke(new DelegLabel(ModLabel), "Pas de prochain repas", labelNextMeal);

            Invoke(new DelegLabel(ModLabel), Safe != null ? "Coffre : Oui" : "Coffre : Non", labelSafe);
            
            if (listViewPets.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(listViewPets.Items.Clear));
            else
                listViewPets.Items.Clear();

            if ((petsList != null) && (petsList.Count != 0))
            {
                foreach (Pet pet in petsList)
                {
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.SubItems[0].Text = I18N.GetText((int)pet.Datas.Fields["nameId"]);
                    listViewItem.SubItems.Add(pet.Informations.UID.ToString());

                    if (pet.FoodList.Count != 0)
                        listViewItem.SubItems.Add(I18N.GetText((int)pet.FoodList[0].Datas.Fields["nameId"]) + " (" + pet.FoodList[0].Informations.Quantity + ")");
                    else
                        listViewItem.SubItems.Add("Aucune (0)");

                    if (pet.NextMeal.Year != 1)
                    {
                        DateTime nextMeal = new DateTime(pet.NextMeal.Year, pet.NextMeal.Month, pet.NextMeal.Day,
                            pet.NextMeal.Hour, pet.NextMeal.Minute, 0);

                        listViewItem.SubItems.Add(nextMeal.ToShortDateString() + " " + nextMeal.ToShortTimeString());
                    }
                    else
                        listViewItem.SubItems.Add("Pas de prochain repas.");

                    listViewItem.SubItems.Add(pet.Effect);

                   AddItem(listViewItem, listViewPets);
                   //if (listViewPets.Items.Count != 0)
                   //    listViewPets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        public void ModLabel(string content, Label lab)
        {
            lab.Text = content;
        }

        private void CommandeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendCommand(CommandeBox.Text);
                CommandeBox.Clear();
            }
        }

        private void FloodContentRbox_TextChanged(object sender, EventArgs e)
        {
            FloodContent = FloodContentRbox.Text;
        }

        public void ActualizeMap()
        {
            this.BeginInvoke(new MethodInvoker(MapView.Items.Clear)); 
            foreach (BlueSheep.Core.Map.Elements.InteractiveElement e in Map.InteractiveElements.Values)
            {
                BlueSheep.Core.Map.Elements.StatedElement element = null;
                if (Map.StatedElements.ContainsKey((int)e.Id))
                {
                    element = Map.StatedElements[(int)e.Id];
                }
                string type = "Inconnu (" + e.TypeId + ")";
                switch (e.TypeId)
                {
                    case 16:
                        type = type.Replace("Inconnu","Zaap");
                        break;
                    case 106:
                        type = type.Replace("Inconnu","Zaapi");
                        break;
                    case 105:
                        type = type.Replace("Inconnu", "Poubelle");
                        break;
                    case 120:
                        type = type.Replace("Inconnu", "Enclos");
                        break;
                    case -1:
                        type = type.Replace("Inconnu", "Porte/Escalier ?");
                        break;
                    case 119:
                        type = type.Replace("Inconnu", "Livre d'artisans");
                        break;
                }
                string cellId = "?";
                if (element != null)
                    cellId = Convert.ToString(element.CellId);

                AddItem(new ListViewItem(new string[] {Convert.ToString(e.Id), cellId,type}),MapView);
            }
            //foreach (BlueSheep.Core.Map.Elements.InteractiveElement d in Map.Doors.Values)
            //{
            //    BlueSheep.Core.Map.Elements.StatedElement element = null;
            //    if (Map.StatedElements.ContainsKey((int)d.Id))
            //    {
            //        element = Map.StatedElements[(int)d.Id];
            //    }
            //    string cellId = "?";
            //    if (element != null)
            //        cellId = Convert.ToString(element.CellId);
            //    AddItem(new ListViewItem(new string[] { Convert.ToString(d.Id), cellId, "Porte" }), MapView);
            //}
            foreach (int npcid in Npc.Npcs.Values)
            {
                AddItem(new ListViewItem(new string[] { Convert.ToString(npcid), "?" , "Pnj" }), MapView);
            }
        }

        public void ActualizeJobs()
        {
            if (JobsTabP.InvokeRequired)
                Invoke(new MethodInvoker(ActualizeJobs));
            else
            {
                JobsTabP.TabPages.Clear();
                foreach (Job j in Jobs)
                {
                    JobsTabP.TabPages.Add(I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Jobs, j.Id).Fields["nameId"]));
                    SadikTabControl t = new SadikTabControl();
                    t.TabPages.Add("Configuration");
                    t.TabPages.Add("Statistiques");
                    JobUC uc = new JobUC(this, j);
                    JobsUC.Add(uc);
                    JobsTabP.TabPages[JobsTabP.TabCount - 1].Controls.Add(uc);
                    t.Dock = DockStyle.Fill;
                    foreach (int i in j.Skills)
                    {
                        DataClass d = GameData.GetDataObject(D2oFileEnum.Skills, i);
                        if ((int)d.Fields["gatheredRessourceItem"] == -1)
                            continue;
                        if (j.Level > (int)d.Fields["levelMin"])
                        {
                            string name = I18N.GetText((int)d.Fields["nameId"]);
                            string rname = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Items, (int)d.Fields["gatheredRessourceItem"]).Fields["nameId"]);
                            int rid = (int)d.Fields["interactiveId"];
                            uc.g.Rows.Add(name, rname, rid);
                        }
                    }

                    foreach (int i in j.Skills)
                    {
                        DataClass d = GameData.GetDataObject(D2oFileEnum.Skills, i);
                        if (j.Level > (int)d.Fields["levelMin"])
                        {
                            string name = I18N.GetText((int)d.Fields["nameId"]);
                            foreach (int c in (ArrayList)d.Fields["craftableItemIds"])
                            {
                                string rname = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Items, c).Fields["nameId"]);
                                uc.gg.Rows.Add(name, rname, c);
                            }
                        }
                    }
                    uc.g.AutoResizeColumns();
                    uc.g.Columns[2].Visible = false;

                    uc.gg.AutoResizeColumns();
                    uc.gg.Columns[2].Visible = false;
                    uc.Show();
                }
            }
        }

        public void Enable(bool param1)
        {
            if (this.InvokeRequired)
                this.Invoke(new DelegBool(Enable), param1);
            else
                this.Enabled = param1;
        } 

        private void LVItems_ColumnClick(object sender, EventArgs e)
        {
            // Call the sort method to manually sort.
            LVItems.Sort();
        }

        private void ForbidMonsterBt_Click(object sender, EventArgs e)
        {
            //if (Fight == null)
            //{
            //    MessageBox.Show("Veuillez choisir une IA avant de régler les restrictions");
            //    return;
            //}
            if (MonsterTextBox.Text.Length > 0)
            {
                ListViewItem l = new ListViewItem(new string[] { MonsterTextBox.Text, (string)ComparateurBox.SelectedItem, Convert.ToString(NUDRestrictions.Value), "Interdit" });
                MonstersRestrictionsView.Items.Add(l);
            }
            MonstersRestrictionsView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void ForceMonstersBt_Click(object sender, EventArgs e)
        {
            //if (Fight == null)
            //{
            //    MessageBox.Show("Veuillez choisir une IA avant de régler les restrictions");
            //    return;
            //}
            if (MonsterTextBox.Text.Length > 0)
            {
                ListViewItem l = new ListViewItem(new string[] { MonsterTextBox.Text, (string)ComparateurBox.SelectedItem, Convert.ToString(NUDRestrictions.Value), "Obligatoire" });
                MonstersRestrictionsView.Items.Add(l);
            }
            MonstersRestrictionsView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

        }

        public void ActualizeFightStats(Dictionary<string, int> winLose, Dictionary<DateTime,int> xpwon)
        {
            if (WinLoseFightPie.InvokeRequired)
            {
                Invoke(new DelegGatherPie(ActualizeFightStats), winLose, xpwon);
                return;
            }
            if (WinLoseFightPie.Titles.Count < 1)
                WinLoseFightPie.Titles.Add("Résultats des combats");
            WinLoseFightPie.Series.Clear();
            WinLoseFightPie.ChartAreas[0].BackColor = Color.Transparent;
            Series series1 = new Series
            {
                Name = "series1",
                IsVisibleInLegend = true,
                Color = System.Drawing.Color.DeepSkyBlue,
                ChartType = SeriesChartType.Pie
            };
            WinLoseFightPie.Series.Add(series1);
            int i = 0;
            foreach (KeyValuePair<string, int> pair in winLose)
            {
                series1.Points.Add(pair.Value);
                var p1 = series1.Points[i];
                p1.AxisLabel = pair.Key + " (" + pair.Value + ")";
                p1.LegendText = pair.Key;
                i += 1;
            }
            this.XpBarsChart.Series.Clear();
            if (XpBarsChart.Titles.Count < 1)
                this.XpBarsChart.Titles.Add("Experience gagnée");
            foreach (KeyValuePair<DateTime, int> p in xpwon)
            {
                Series series = new Series(p.Key.ToShortDateString());
                series.Name = p.Key.ToShortDateString();
                series.IsVisibleInLegend = true;
                series.ChartType = SeriesChartType.Column;
                series.Points.Add(p.Value);
                XpBarsChart.Series.Add(series);
            }
            WinLoseFightPie.Invalidate();
            XpBarsChart.Invalidate();
        }

        private void MonsterTextBox_GetFocus(object sender, EventArgs e)
        {
            if (MonsterTextBox.Text == "Entrez le nom du monstre...")
                MonsterTextBox.Text = "";
        }

        private void MonstersRestrictionView_Supp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && MonstersRestrictionsView.SelectedItems.Count > 0)
            {
                for (int i = 0; i < MonstersRestrictionsView.Items.Count; i++)
                {
                    if (MonstersRestrictionsView.Items[i].Selected)
                        MonstersRestrictionsView.Items.RemoveAt(i);
                }
            }
        }
        #endregion

        #region Methodes Publics
        public void TryReconnect(int minute)
        {
            SocketManager.DisconnectFromGUI();

            Log(new ConnectionTextInformation("Reconnexion automatique dans " + minute + " minute(s)."),0);

            //if (m_TimerConnectionThread == null)
                m_TimerConnectionThread = new Timer(TimerConnectionThreadFinished, null, (int)TimeSpan.FromMinutes(minute).TotalMilliseconds,
                    Timeout.Infinite);
            //else
            //    m_TimerConnectionThread.Change((int)TimeSpan.FromMinutes(minute).TotalMilliseconds, Timeout.Infinite);

            //State = "En reconnexion (" + minute + ")";
            //MainForm.ActualMainForm.ActualizeAccountInformations();
        }

        public void Wait(int min, int max)
        {
            Random Random = new Random();
            int Temps = Random.Next(min, max);
            double endwait = Environment.TickCount + Temps;
            while (Environment.TickCount < endwait)
            {
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
        }

        public void GetNextMeal()
        {
            m_NextMeal = new DateTime();

            //foreach (Bot bot in m_Bots)
            //{
                if (m_NextMeal.Year == 1)
                {
                    m_NextMeal = new DateTime(NextMeal.Year, NextMeal.Month, NextMeal.Day, NextMeal.Hour,
                        NextMeal.Minute, 0);
                    //continue;
                }

                else if (NextMeal <= m_NextMeal)
                {
                    m_NextMeal = new DateTime(NextMeal.Year, NextMeal.Month, NextMeal.Day,NextMeal.Hour,
                        NextMeal.Minute, 0);
                }
            //}

            if (m_NextMeal.Year != 1)
            {
                m_NextMeal = m_NextMeal.AddMinutes(2);

                DateTime difference = new DateTime((m_NextMeal - DateTime.Now).Ticks);

               Log(new GeneralTextInformation("Prochain repas dans " + difference.Hour + " heure(s) " +
                    difference.Minute + " minute(s)."),3);

                if (m_TimerConnectionThread == null)
                    m_TimerConnectionThread = new Timer(TimerConnectionThreadFinished, null,
                        (int)TimeSpan.FromHours(difference.Hour).TotalMilliseconds +
                        (int)TimeSpan.FromMinutes(difference.Minute).TotalMilliseconds, Timeout.Infinite);
                else
                    m_TimerConnectionThread.Change((int)TimeSpan.FromHours(difference.Hour).TotalMilliseconds +
                        (int)TimeSpan.FromMinutes(difference.Minute).TotalMilliseconds, Timeout.Infinite);
            }
            else
            {
               Log(new GeneralTextInformation("Aucune nourriture disponible, pas de prochaine connexion."),1);
            }
        }

        public void StartFeeding()
        {
            Running = new Running(this);
            Running.Init();
        }

        public void Init()
        {
            this.Enabled = false;
            m_ConnectionThread = new Thread(Connect);
            m_ConnectionThread.Start();
        }

        private void SendCommand(string text)
        {
            string[] command = text.Split(':');
            switch (command[0])
            {
                //case "/invite":
                //    Invitation(command[1]);
                //    break;
                case "/droite":
                    this.Map.ChangeMap("droite");
                    break;
                case "/gauche":
                    this.Map.ChangeMap("gauche");
                    break;
                case "/bas":
                    this.Map.ChangeMap("bas");
                    break;
                case "/haut":
                    this.Map.ChangeMap("haut");
                    break;
                case "/mapid":
                    Log(new BotTextInformation("L'id de la map est : " + this.Map.Id),0);
                    break;
                case "/cellid":
                    Log(new BotTextInformation("Le joueur se trouve sur la cellule " + this.Map.Character.CellId),0);
                    break;
                case "/cell":
                    try
                    {
                        this.Map.MoveToCell(Convert.ToInt32(command[1]));
                        Log(new BotTextInformation("Déplacement vers la cellid " + command[1]),0);
                    }
                    catch (Exception ex)
                    {
                        Log(new ErrorTextInformation(ex.Message),0);
                    }
                    break;
                case "/g":
                    Flood.SendMessage(2, command[1]);
                    break;
                case "/r":
                    Flood.SendMessage(6, command[1]);
                    break;
                case "/b":
                    Flood.SendMessage(5, command[1]);
                    break;
                case "/a":
                    Flood.SendMessage(3, command[1]);
                    break;
                case "/s":
                    Flood.SendMessage(0, command[1]);
                    break;
                case "/w":
                    string name = command[1];
                    string content = command[2];
                    using (BigEndianWriter writer = new BigEndianWriter())
                    {
                        ChatClientPrivateMessage msg = new ChatClientPrivateMessage(content, name);
                        msg.Serialize(writer);
                        writer.Content = this.HumanCheck.hash_function(writer.Content);
                        MessagePackaging pack = new MessagePackaging(writer);
                        pack.Pack((int)msg.ProtocolID);
                        SocketManager.Send(pack.Writer.Content);
                        Log(new PrivateTextInformation("à " + name + " : " + content), 1);
                    }
                    break;
            }
        }

        public bool PerformGather()
        {
            List<int> r = new List<int>();
            foreach (JobUC j in JobsUC)
            {
                for (int i = 0; i < j.g.RowCount; i++)
                {
                    if (j.g.Rows[i].Selected == true)
                    {
                        r.Add((int)j.g.Rows[i].Cells[2].Value);
                    }
                }
                if (r.Count() <= 0 && !j.HasRightTool())
                    continue;
                return Gather.GoGather(r, j);
            }
            return false;

        }

        public void TryFeeding()
        {
            Running = new Running(this);
            Running.Init();
        }

        public void SetNextMeal()
        {
            foreach (Pet pet in petsList)
            {
                if (pet.NextMeal.Year == 1)
                    continue;

                if (pet.NonFeededForMissingFood)
                    continue;

                if (NextMeal.Year == 1)
                {
                    NextMeal = pet.NextMeal;
                    continue;
                }

                if (pet.NextMeal <= NextMeal)
                    NextMeal = pet.NextMeal;
            }
        }

        public bool VerifGroup(List<string> monsters)
        {
            if (MonstersRestrictionsView.InvokeRequired)
                return (bool)Invoke(new DelegVerifGroup(VerifGroup), monsters);
            if (MonstersRestrictionsView.Items.Count <= 0)
                return true;
            foreach (string s in monsters)
            {
                ListViewItem i = MonstersRestrictionsView.FindItemWithText(s, false, 0, false);
                if (i == null)
                    continue;
                switch (i.SubItems[1].Text)
                {
                    case ">":
                        if (monsters.FindAll(f => s == f).Count > Convert.ToInt32(i.SubItems[2].Text))
                        {
                            switch (i.SubItems[3].Text)
                            {
                                case "Interdit":
                                    return false;
                                case "Obligatoire":
                                    break;
                            }
                        }
                        break;
                    case "<":
                        if (monsters.FindAll(f => s == f).Count < Convert.ToInt32(i.SubItems[2].Text))
                        {
                            switch (i.SubItems[3].Text)
                            {
                                case "Interdit":
                                    return false;
                                case "Obligatoire":
                                    break;
                            }
                        }
                        break;
                    case "=":
                        if (monsters.FindAll(f => s == f).Count == Convert.ToInt32(i.SubItems[2].Text))
                        {
                            switch (i.SubItems[3].Text)
                            {
                                case "Interdit":
                                    return false;
                                case "Obligatoire":
                                    break;
                            }
                        }
                        break;
                }
            }

            return true;

        }
        #endregion

        #region Methodes privées
        private void Connect()
        {
            if (m_TimerConnectionThread != null)
                m_TimerConnectionThread.Change(Timeout.Infinite, Timeout.Infinite);

            Thread.Sleep(GetRandomTime());

            m_Running = new Running(this);

                    if ((this.SocketManager != null) && (this.SocketManager.Connected()))
                        return;

                    Log(new ConnectionTextInformation("Connexion."),0);

                    this.SocketManager = new SocketManager(this);

                    this.SocketManager.Connect(new ConnectionInformations("213.248.126.39", 5555, "d'identification"));
                    loginstate = "identification";
                    if (checkBoxBegin.Checked == true)
                        GetNextMeal();
        }

        private void TimerConnectionThreadFinished(object stateInfo)
        {
            if (this.IsDisposed == true)
                return;
            Connect();
        }

        private static int GetRandomTime()
        {
            Random random = new Random();

            return random.Next(500, 1250);
        }

        private void Serialize<T>(T obj, string sConfigFilePath)
        {

            System.Xml.Serialization.XmlSerializer XmlBuddy = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.Xml.XmlWriterSettings MySettings = new System.Xml.XmlWriterSettings();
            MySettings.Indent = true;
            MySettings.CloseOutput = true;
            MySettings.OmitXmlDeclaration = true;
            System.Xml.XmlWriter MyWriter = System.Xml.XmlWriter.Create(sConfigFilePath, MySettings);
            XmlBuddy.Serialize(MyWriter, obj);
            MyWriter.Flush();
            MyWriter.Close();

        }
        #endregion

        

        







        

        

 





        
    }
}

