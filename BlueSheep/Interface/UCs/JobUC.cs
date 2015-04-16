using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BlueSheep.Core.Job;
using BlueSheep.Interface.Text;
using System.IO;
using System.Xml;

namespace BlueSheep.Interface
{
    public partial class JobUC : MetroFramework.Controls.MetroUserControl
    {
        /// <summary>
        /// Represents the Jobs tab of the main accountUC.
        /// </summary>

        #region Fields
        private AccountUC account;
        private Job job;
        private delegate void DelegActualize(Dictionary<string,int> ressources);
        private delegate void Callback();
        private TreeView TV;
        private StreamWriter sr;
        #endregion

        #region Properties
        private TreeNode DayNode
        {
            get {
                TreeNode[] t = TV.Nodes.Find(DateTime.Now.ToShortDateString(), true);
                if (t.Length > 0)
                    return t[0];
                else
                    return null;
            }
        }
        #endregion

        #region Constructors
        public JobUC(AccountUC Account, Job j , List<TreeNode> nodes = null)
        {
            InitializeComponent();
            account = Account;
            job = j;
            TV = new TreeView() { Dock = DockStyle.Fill };
            Load(nodes);
            InitTree();
            sadikTabControl1.TabPages[0].Controls.Add(g);
            sadikTabControl1.TabPages[1].Controls.Add(gg);
            sadikTabControl1.TabPages[2].Controls.Add(TV);
            //sadikTabControl1.TabPages[2].Controls.Add(GatherPie);
            this.Dock = DockStyle.Fill;
            g.Columns.Add("SkillName", "Skills");
            g.Columns.Add("RessourceName", "Ressources");
            g.Columns.Add("RessourceId", "Id");
            g.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Select",  HeaderText = "A récolter"});
            g.Columns[1].Width = 200;
            g.MultiSelect = false;

            gg.Columns.Add("SkillName", "Skills");
            gg.Columns.Add("RecipeName", "Recettes");
            gg.Columns[1].Width = 200;
            gg.Columns.Add("RecipeId", "Id");
            gg.ReadOnly = true;

            BlueSheep.Engine.Constants.Translate.TranslateUC(this);
        }
        #endregion

        #region Public Methods
        public void ActualizeStats(Dictionary<string, int> ressourcesGathered)
        {   
            if (TV.InvokeRequired)
            {
                Invoke(new DelegActualize(ActualizeStats), ressourcesGathered);
                return;
            }
            foreach (KeyValuePair<string, int> pair in ressourcesGathered)
            {
                if (DayNode.Nodes.ContainsKey(pair.Key))
                    DayNode.Nodes.RemoveByKey(pair.Key);
                DayNode.Nodes.Add(pair.Key, pair.Key + " : " + pair.Value.ToString());
            }
        }

        private void InitTree()
        {
            if (TV.InvokeRequired)
            {
                Invoke(new Callback(InitTree));
                return;
            }
            DayNode.Nodes.Add("Level", "Level : " + job.Level);
            DayNode.Nodes.Add("Experience", "Experience : " + job.XP);
            DayNode.Nodes.Add("XpLevelFloor", "Next level : " + job.XpNextLevelFloor);
            TV.ExpandAll();
            TV.Invalidate();
        }

        public void UpdateJob()
        {
            //if (TV.InvokeRequired)
            //{
            //    Invoke(new Callback(UpdateJob));
            //    return;
            //}
            //DayNode.Nodes.RemoveByKey("Level");
            //DayNode.Nodes.RemoveByKey("Experience");
            //DayNode.Nodes.RemoveByKey("XpLevelFloor");
            //InitTree();
        }

        //Open the XML file, and start to populate the treeview
        public void populateTreeview()
        {
            PopulateCheckbox();
            try
            {
                //Just a good practice -- change the cursor to a 
                //wait cursor while the nodes populate
                this.Cursor = Cursors.WaitCursor;
                //First, we'll load the Xml document
                XmlDocument xDoc = new XmlDocument();
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BlueSheep", "Accounts", account.AccountName,
                    account.CharacterBaseInformations.name + "-" + job.Name + ".xml");
                if (!File.Exists(path))
                    return;
                xDoc.Load(path);
                //Now, clear out the treeview, 
                //and add the first (root) node
                //TV.Nodes.Clear();
                //TV.Nodes.Add(new
                //  TreeNode(xDoc.DocumentElement.Name.Replace("_", "").Replace("-","/")));
                //TreeNode tNode = new TreeNode();
                //tNode = (TreeNode)TV.Nodes[0];
                foreach (XmlNode xNode in xDoc.ChildNodes[0].ChildNodes)
                {
                    addTreeNode(xNode);
                }
                //We make a call to addTreeNode, 
                //where we'll add all of our nodes
                //Expand the treeview to show all nodes
                TV.ExpandAll();
            }
            catch (XmlException xExc)
            //Exception is thrown is there is an error in the Xml
            {
                MessageBox.Show(xExc.Message);
            }
            catch (Exception ex) //General exception
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default; //Change the cursor back
            }

        }

        //This function is called recursively until all nodes are loaded
        private void addTreeNode(XmlNode xmlNode)
        {
            string temp = "";
            string[] parsed = xmlNode.OuterXml.Replace("\r", "").Replace("\t", "").Split('\n');
            foreach (string s in parsed)
            {
                if (s.StartsWith("<_"))
                {
                    string t = s.Replace("_", "").Replace("-", "/").Replace("<", "").Replace(">", "").Trim();
                    TV.Nodes.Add(t.Split(' ')[0], t);
                    temp = t;
                }
                else if (s.StartsWith("</") && temp != "")
                    continue;
                else
                    TV.Nodes.Find(temp, true)[0].Nodes.Add(s, s);
                //treeNode.Nodes.Add(s, s);
            }
        }


        public void exportToXml()
        {
            if (TV.Nodes.Count <= 0)
                return;
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BlueSheep", "Accounts", account.AccountName,
                    account.CharacterBaseInformations.name + "-" + job.Name + ".xml");
            sr = new StreamWriter(path, false, System.Text.Encoding.UTF8);
            foreach (DataGridViewRow r in g.Rows)
            {
                bool value = Convert.ToBoolean(r.Cells[3].Value);
                if (value != null)
                    sr.WriteLine(value.ToString());
            }
                
            //Write our root node
            string header = "Root";
            sr.WriteLine("<" + header + ">");
            //foreach (TreeNode node in TV.Nodes)
            //{
            //    saveNode(node.Nodes);
            //}
            saveNode(TV.Nodes);
            //Close the root node
            sr.WriteLine("</" + header + ">");
            sr.WriteLine("OpenBag:" + OpenBagCb.Checked.ToString());
            sr.Close();
        }

        private void saveNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    sr.WriteLine("<_" + node.Text.Replace("/","-") + ">");
                    saveNode(node.Nodes);
                    sr.WriteLine("</_" + node.Text.Replace("/", "-") + ">");
                }
                else //No child nodes, so we just write the text
                    sr.WriteLine(node.Text);
            }
        }

        public bool HasRightTool()
        {
            bool h = job.HasRightTool();
            if (!h)
                account.Log(new ErrorTextInformation("L'outil n'est pas équipé :( "),0);
            return h;
        }

        private void AddTreeViewNode()
        {
            if (sadikTabControl1.TabPages[2].InvokeRequired)
            {
                Invoke(new Callback(AddTreeViewNode));
                return;
            }
            TV.Nodes.Add(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
            TV.Invalidate();
        }

        private void Load(List<TreeNode> nodes = null)
        {
            if (nodes != null)
            {
                foreach (TreeNode node in nodes)
                    TV.Nodes.Add(node);
            }
            if (!CheckNodes())
                AddTreeViewNode();
        }

        private bool CheckNodes()
        {
            return !(DayNode == null);
        }

        private void PopulateCheckbox()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                   "BlueSheep", "Accounts", account.AccountName,
                   account.CharacterBaseInformations.name + "-" + job.Name + ".xml");
            try
            {
                StreamReader sr = new StreamReader(path);
                int i = 0;
                List<string> temp = new List<string>();
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    if ((line.StartsWith("T") || line.StartsWith("F")) && line != null)
                    {
                        g.Rows[i].Cells[3].Value = Convert.ToBoolean(line);
                    }
                    else if (line.StartsWith("OpenBag:"))
                    {
                        line = line.Remove(0, 8);
                        OpenBagCb.Checked = Convert.ToBoolean(line);
                    } 
                    else
                        temp.Add(line);
                    i++;
                }
                sr.Close();
                File.WriteAllLines(path, temp.ToArray());
                g.Invalidate();
            }
            catch (FileNotFoundException)
            {
                return;
            }
           
            
        }

        
        #endregion
    }
}
