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
using BlueSheep.Engine.Enums;
using BlueSheep.Common;
using System.IO;

namespace BlueSheep.Interface
{
    public partial class AccountUC : MetroFramework.Controls.MetroUserControl
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
        private List<Bot> m_Bots = new List<Bot>();
        public string loginstate;
        private DateTime m_NextMeal;
        public bool IsMaster;
        public bool IsSlave;
        public BFight Fight;
        public FightData FightData;
        public Map Map;
        public MapData MapData;
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
        public CaracUC CaracUC;
        public RegenUC RegenUC;
        public FloodUC FloodUC;
        public bool IsMITM;
        public Status state;
        public ConfigManager ConfigManager;
        public FightParser FightParser;
        public WatchDog WatchDog;
        public AccountFrm m_ParentForm;
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
        public CharacterBaseInformations CharacterBaseInformations { get; set; }
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
        private delegate bool BoolCallback();
        private delegate void Callback();
        #endregion

        #region Constructeurs

        public AccountUC(string username, string password, bool socket, MetroFramework.Forms.MetroForm form = null)
        {
            InitializeComponent();
            MonsterTextBox.KeyUp += (s, e) =>
            {
                IntelliSense.AutoCompleteTextBox(MonsterTextBox, lstPopup, IntelliSense.MonstersList, e);
            };
            if (form != null)
                m_ParentForm = (AccountFrm)form;
            AccountName = username;
            AccountPassword = password;
            PetsModifiedList = new List<Pet>();
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
            LVItemBag.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Nom", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Quantité", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Prix moyen", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Nom", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Quantité", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Prix de vente", -2, HorizontalAlignment.Center);
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
            LVItemBag.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Name", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItemBag.Columns.Add("Average Price", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("GID", 0, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("UID", 0, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Name", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Type", -2, HorizontalAlignment.Center);
            LVItemShop.Columns.Add("Sell price", -2, HorizontalAlignment.Center);
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

            //Carac
            CaracUC = new CaracUC(this);
            StatsPage.Controls.Add(CaracUC);
            CaracUC.Show();

            //Regen
            RegenUC = new RegenUC(this);
            RegenPage.Controls.Add(RegenUC);
            RegenUC.Show();

            //Flood
            FloodUC = new FloodUC(this);
            tabPage2.Controls.Add(FloodUC);
            FloodUC.Show();

            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BlueSheep", "Accounts", AccountName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //Config Manager
            this.ConfigManager = new ConfigManager(this);
           
            Flood = new Core.Misc.Flood(this);
            FightData = new FightData(this);
            MapData = new MapData(this);
            WatchDog = new WatchDog(this);
        }

        public AccountUC()
        {
            InitializeComponent();
        }

#endregion

        #region Methodes d'interfaces

        private void Form_Closed(object sender, EventArgs e)
        {
            if (this.SocketManager != null)
                this.SocketManager.DisconnectFromGUI();
            if (IsMITM)
            {
                this.SocketManager.DisconnectServer("Form closing");
            }
        }

        private void SaveConfig_Click(object sender, EventArgs e)
        {
            ConfigManager.SaveConfig();
        }

        private void DeleteConfigBt_Click(object sender, EventArgs e)
        {
            ConfigManager.DeleteConfig();
        }

        public void InitMITM()
        {
            this.SocketManager = new SocketManager(this);
            this.SocketManager.InitMITM();

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
                text.Text = "[" + DateTime.Now.ToLongTimeString() +
                    "] (" + text.Category + ") " + text.Text;
                if (text.Category == "Debug" && !DebugMode.Checked)
                    return;

                if (LogCb.Checked)
                    using (StreamWriter fileWriter = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Logs\" + DateTime.Now.ToShortDateString().Replace("/", "-") + "_" + this.CharacterBaseInformations.name +".txt", true))
                        fileWriter.WriteLine(text.Text);

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
            if (state == BlueSheep.Engine.Enums.Status.Fighting)
            {
                Log(new ErrorTextInformation("Impossible de supprimer un objet en combat ^^"), 0);
                return;
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
            if (state == BlueSheep.Engine.Enums.Status.Fighting)
            {
                Log(new ErrorTextInformation("Impossible de jeter un objet en combat ^^"), 0);
                return;
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
            if (state == BlueSheep.Engine.Enums.Status.Fighting)
            {
                Log(new ErrorTextInformation("Impossible d'utiliser un objet en combat ^^"), 0);
                return;
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
            if (this.SocketManager.State == SocketState.Connected)
            {
                this.SocketManager.DisconnectFromGUI();
            }
            else
            {
                //Init();
                TryReconnect(2);
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
                Log(new BotTextInformation("Lancement du trajet"),1);
                Path.Start();
            }
            else
                Log(new ErrorTextInformation("Aucun trajet chargé"),3);
        }

        private void StopPathBt_Click(object sender, EventArgs e)
        {
            if (Path != null)
            {
                Path.StopPath();
                this.Log(new BotTextInformation("Trajet arrêté"),1);
            }
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

        public void ActualizeInventory()
        {
            this.BeginInvoke(new MethodInvoker(LVItems.Items.Clear));
            foreach (Core.Inventory.Item i in Inventory.Items)
            {
                string[] row1 = { i.GID.ToString(), i.UID.ToString(), i.Name, i.Quantity.ToString(), i.Type.ToString(), i.Price.ToString() };
                System.Windows.Forms.ListViewItem li = new System.Windows.Forms.ListViewItem(row1);
                li.ToolTipText = i.Description;
                AddItem(li, LVItems);
            }
            RegenUC.RefreshQuantity();

            this.BeginInvoke(new MethodInvoker(LVItemBag.Items.Clear));
            foreach (Core.Inventory.Item i in Inventory.Items)
            {
                string[] row1 = { i.GID.ToString(), i.UID.ToString(), i.Name, i.Quantity.ToString(), i.Type.ToString(), i.Price.ToString() };
                System.Windows.Forms.ListViewItem li = new System.Windows.Forms.ListViewItem(row1);
                li.ToolTipText = i.Description;
                AddItem(li, LVItemBag);
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
            if (lab.InvokeRequired)
            {
                Invoke(new DelegLabel(ModLabel), content, lab);
                return;
            }
            lab.Text = content;
        }

        private void CommandeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = CommandeBox.Text;
                CommandeBox.Clear();
                CLIParser parser = new CLIParser(this);
                List<string> result = parser.Parse(text);
                foreach (string s in result)
                {
                    if (s.Contains("ERROR"))
                        Log(new ErrorTextInformation(s), 0);
                    else
                        Log(new BotTextInformation(s), 0);
                }   
            }
            else if (e.KeyCode == Keys.Up && CLIParser.CommandsHistory.Count > 0)
            {
                int index = CLIParser.CommandsHistory.IndexOf(CommandeBox.Text);
                if (index != -1 && index != 0)
                    CommandeBox.Text = CLIParser.CommandsHistory[index - 1];
                else
                    CommandeBox.Text = CLIParser.CommandsHistory[CLIParser.CommandsHistory.Count - 1];
            }
            else if (e.KeyCode == Keys.Down && CLIParser.CommandsHistory.Count > 0)
            {
                int index = CLIParser.CommandsHistory.IndexOf(CommandeBox.Text);
                if (index != -1 && index != CLIParser.CommandsHistory.Count)
                    CommandeBox.Text = CLIParser.CommandsHistory[index + 1];
                else
                    CommandeBox.Text = "";
            }
        }

        public void ActualizeMap()
        {
            this.BeginInvoke(new MethodInvoker(MapView.Items.Clear)); 
            foreach (BlueSheep.Core.Map.Elements.InteractiveElement e in MapData.InteractiveElements.Keys)
            {
                BlueSheep.Core.Map.Elements.StatedElement element = MapData.StatedElements.Find(s => s.Id == e.Id);
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
            foreach (GameRolePlayNpcInformations n in MapData.Npcs)
            {
                AddItem(new ListViewItem(new string[] { Convert.ToString(n.npcId), "?" , "Pnj" }), MapView);
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
                    JobsTabP.TabPages.Add(j.Name);
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
                        if (j.Level >= (int)d.Fields["levelMin"])
                        {
                            string name = I18N.GetText((int)d.Fields["nameId"]);
                            int rid = (int)d.Fields["interactiveId"];
                                string typename = I18N.GetText((int)GameData.GetDataObject(D2oFileEnum.Interactives, rid).Fields["nameId"]);
                                uc.g.Rows.Add(name, typename, rid);
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
                                string rname = "Unknown";
                                DataClass data = GameData.GetDataObject(D2oFileEnum.Items, c);
                                if (data != null)
                                {
                                    rname = I18N.GetText((int)data.Fields["nameId"]);
                                    uc.gg.Rows.Add(name, rname, c);
                                }
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
            #if __MonoCS__

            #else
            if (WinLoseFightPie.Titles.Count < 1)
            WinLoseFightPie.Titles.Add("Résultats des combats");
            #endif
            WinLoseFightPie.Series.Clear();
            WinLoseFightPie.ChartAreas[0].BackColor = Color.Transparent;
            Series series1 = new Series
            {
                Name = "series1",
                IsVisibleInLegend = false,
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
            #if __MonoCS__

            #else
            if (XpBarsChart.Titles.Count < 1)
            this.XpBarsChart.Titles.Add("Experience gagnée");
            #endif
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

        public void SetStatus(Status status)
        {
            state = status;
            string nstatus = "";
            switch (status)
            {
                case Status.None:
                    nstatus = "Connecté";
                    break;
                case Status.Exchanging:
                    nstatus = "Echange";
                    break;
                case Status.Fighting:
                    nstatus = "Combat";
                    break;
                case Status.Gathering:
                    nstatus = "Récolte";
                    break;
                case Status.Moving:
                    nstatus = "Déplacement";
                    break;
                case Status.Speaking:
                    nstatus = "Dialogue";
                    break;
                case Status.Teleporting:
                    nstatus = "Teleportation";
                    break;
                case Status.Regenerating:
                    nstatus = "Régénération";
                    break;
                case Status.Disconnected:
                    nstatus = "Déconnecté";
                    break;
                case Status.Busy:
                    nstatus = "Occupé";
                    break;
            }
            if (MainForm.ActualMainForm.Lang != "FR")
            {
                nstatus = BlueSheep.Engine.Constants.Translate.GetTranslation(nstatus, MainForm.ActualMainForm.Lang);
            }
            Invoke(new DelegLabel(ModLabel),nstatus, StatusLb);
        }
        #endregion

        #region Methodes Publics

        public void TryReconnect(int secondes)
        {
            Log(new ConnectionTextInformation("Reconnexion automatique dans " + secondes + " secondes."), 0);
            SocketManager.Disconnect("Try Reconnect.");
            m_TimerConnectionThread = new Timer(TimerConnectionThreadFinished, null, (int)TimeSpan.FromSeconds(secondes).TotalMilliseconds,
                Timeout.Infinite);
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

                if (m_NextMeal.Year == 1)
                {
                    m_NextMeal = new DateTime(NextMeal.Year, NextMeal.Month, NextMeal.Day, NextMeal.Hour,
                        NextMeal.Minute, 0);
                }

                else if (NextMeal <= m_NextMeal)
                {
                    m_NextMeal = new DateTime(NextMeal.Year, NextMeal.Month, NextMeal.Day,NextMeal.Hour,
                        NextMeal.Minute, 0);
                }

            if (m_NextMeal.Year != 1)
            {
                m_NextMeal = m_NextMeal.AddMinutes(2);

                DateTime difference = new DateTime((m_NextMeal - DateTime.Now).Ticks);

               Log(new GeneralTextInformation("Prochain repas dans " + difference.Hour + " heure(s) " +
                    difference.Minute + " minute(s)."),3);
               this.SocketManager.Disconnect("Wait before next meal.");

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
            //this.Enabled = false;
            //this.SocketManager = null;
            m_ConnectionThread = new Thread(Connect);
            m_ConnectionThread.Start();
        }

        public bool PerformGather()
        {
            List<int> r = new List<int>();
            foreach (JobUC j in JobsUC)
            {
                for (int i = 0; i < j.g.RowCount; i++)
                {
                    if (Convert.ToBoolean(j.g.Rows[i].Cells[3].Value) == true)
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
            foreach (ListViewItem i in MonstersRestrictionsView.Items)
            {
                switch (i.SubItems[3].Text)
                {
                    case "Interdit":
                        switch (i.SubItems[1].Text)
                        {
                            case ">":
                                if (monsters.FindAll(f => i.SubItems[0].Text == f).Count > Convert.ToInt32(i.SubItems[2].Text))
                                    return false;
                                else
                                    continue;
                            case "<":
                                if (monsters.FindAll(f => i.SubItems[0].Text == f).Count < Convert.ToInt32(i.SubItems[2].Text))
                                    return false;
                                else
                                    continue;
                            case "=":
                                if (monsters.FindAll(f => i.SubItems[0].Text == f).Count == Convert.ToInt32(i.SubItems[2].Text))
                                    return false;
                                else
                                    continue;
                            default:
                                continue;
                        }
                    case "Obligatoire":
                        switch (i.SubItems[1].Text)
                        {
                            case ">":
                                if (!(monsters.FindAll(f => i.SubItems[0].Text == f).Count > Convert.ToInt32(i.SubItems[2].Text)))
                                    return false;
                                else
                                    continue;
                            case "<":
                                if (!(monsters.FindAll(f => i.SubItems[0].Text == f).Count < Convert.ToInt32(i.SubItems[2].Text)))
                                    return false;
                                else
                                    continue;
                            case "=":
                                if (!(monsters.FindAll(f => i.SubItems[0].Text == f).Count == Convert.ToInt32(i.SubItems[2].Text)))
                                    return false;
                                else
                                    continue;
                            default:
                                continue;
                        }
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

            if (this.SocketManager != null && this.SocketManager.State == SocketState.Connected)
                return;

            Log(new ConnectionTextInformation("Connexion."), 0);

            if (this.SocketManager == null)
                this.SocketManager = new SocketManager(this);

            this.SocketManager.Connect(new ConnectionInformations("213.248.126.40", 5555, "d'identification"));
            loginstate = "identification";
            if (checkBoxBegin.Checked == true)
                GetNextMeal();
        }

        private void TimerConnectionThreadFinished(object stateInfo)
        {
            if (this.IsDisposed == true)
                return;
            //Init();
            //AccountUC Uc = new AccountUC(this.AccountName, this.AccountPassword, true);
            //this.ParentForm.Controls.Add(Uc);
            //Uc.Show();
            if (this.MyGroup == null && m_ParentForm != null)
                m_ParentForm.Reconnect();
            //this.Dispose();
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

        private void LVItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {

        }

        private void sadikCheckbox1_CheckedChanged(object sender)
        {

        }

       


        private void AutoDelAddBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    ListViewItem item = new ListViewItem(new string[] { LVItems.Items[i].SubItems[2].Text, "Suppression automatique" });
                    GestItemsUC.LVGestItems.Items.Add(item);
                }
            }
        }

        private void RegenAddBt_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < LVItems.Items.Count; i++)
            {
                if (LVItems.Items[i].Selected)
                {
                    ListViewItem item = new ListViewItem(new string[] { LVItems.Items[i].SubItems[2].Text, LVItems.Items[i].SubItems[3].Text });
                    RegenUC.LVItems.Items.Add(item);
                }
            }
        }

        private void RelaunchPath_CheckedChanged(object sender, EventArgs e)
        {
            if (RelaunchPath.Checked)
            {
                RelaunchPath.Text = "✔ Relancer le trajet à la reconnexion";
            }
            else
            {
                RelaunchPath.Text = "✘ Relancer le trajet à la reconnexion";
            }
            ConfigManager.SaveConfig();
        }

        private void sadikCheckbox1_CheckedChanged_1(object sender)
        {
            if (sadikCheckbox1.Checked)
            {
                PlaceTimer.Interval = 10000;
                PlaceTimer.Start();
            }
            else
            {
                PlaceTimer.Stop();
            }
        }

        private void sadikButton3_Click(object sender, EventArgs e)
        {
            
                        ExchangeRequestOnShopStockMessage packetshop = new ExchangeRequestOnShopStockMessage();
                        SocketManager.Send(packetshop);
                    
        }

        public void addItemToShop()
        {
            if (LVItemBag.InvokeRequired)
            {
                Invoke(new Callback(addItemToShop));
                return;
            }
            for (int i = 0; i < LVItemBag.Items.Count; i++)
            {
                if (LVItemBag.Items[i].Selected)
                {
                    try
                    {
            //Inventory.SendItemToShop(Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).UID, Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).Quantity, Convert.ToInt32(numericUpDown1.Value));
            ExchangeObjectMovePricedMessage msg = new ExchangeObjectMovePricedMessage();
            //BlueSheep.Core.Inventory.Item item = new BlueSheep.Core.Inventory.Item(null, 0, 0, msg.quantity, msg.objectUID, this);
            msg.objectUID = Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).UID;
            msg.quantity = Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).Quantity;
            msg.price = Convert.ToInt32(numericUpDown1.Value);
            SocketManager.Send(msg);
            Log(new ActionTextInformation("Ajout de " + Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).Name + "(x " + Inventory.GetItemFromName(LVItemBag.Items[i].SubItems[2].Text).Quantity + ") dans le magasin au prix de : " + msg.price + " Kamas"), 2);
            LeaveDialogRequestMessage packetleave = new LeaveDialogRequestMessage();
            Wait(2000, 2000);
            SocketManager.Send(packetleave);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
        private void PlaceTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (state == Status.Fighting)
                {
                    Log(new ErrorTextInformation("Impossible de se mettre en mode marchand en combat >.<"), 2);
                }
                if (this.SocketManager.State == SocketState.Connected)
                {
                    ExchangeShowVendorTaxMessage taxpacket = new ExchangeShowVendorTaxMessage();
                    SocketManager.Send(taxpacket);
                    ExchangeStartAsVendorMessage ventepacket = new ExchangeStartAsVendorMessage();
                    SocketManager.Send(ventepacket);
                    //Thread.Sleep(500);
                    Log(new BotTextInformation("Essai de l'activation du mode marchand"), 1);
                    if (this.SocketManager.State == SocketState.Closed)
                    {
                        this.SocketManager.DisconnectFromGUI();
                        PlaceTimer.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool NeedToAddItem()
        {
            if (LVItemBag.InvokeRequired)
                return (bool)Invoke(new BoolCallback(NeedToAddItem));
            else
                return (LVItemBag.SelectedItems != null);
        }

        private void BtnActualize_Click(object sender, EventArgs e)
        {
            ExchangeRequestOnShopStockMessage packetshop = new ExchangeRequestOnShopStockMessage();
            SocketManager.Send(packetshop);
            LeaveDialogRequestMessage packetleave = new LeaveDialogRequestMessage();
            SocketManager.Send(packetleave);
        }

        public void actualizeshop(List<ObjectItemToSell>sell)
        {
            this.BeginInvoke(new MethodInvoker(LVItemShop.Items.Clear));

            foreach (ObjectItemToSell i in sell)
            {
                BlueSheep.Core.Inventory.Item item = new BlueSheep.Core.Inventory.Item(i.effects.ToList(), i.objectGID,0,i.quantity, i.objectUID, this);
                string[] row1 = { item.GID.ToString(), item.UID.ToString(), item.Name, item.Quantity.ToString(), item.Type, i.objectPrice.ToString() };
                System.Windows.Forms.ListViewItem li = new System.Windows.Forms.ListViewItem(row1);
                li.ToolTipText = item.Description;
                AddItem(li, LVItemShop);
            }  
        }

        private void client_shoprequest(){

        }
    }
}

