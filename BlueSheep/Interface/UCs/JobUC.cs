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

namespace BlueSheep.Interface
{
    public partial class JobUC : UserControl
    {
        private AccountUC account;
        private Job job;
        private delegate void DelegGatherPie(Dictionary<string,int> ressources);

        public JobUC(AccountUC Account, Job j)
        {
            InitializeComponent();
            account = Account;
            job = j;
            sadikTabControl1.TabPages[0].Controls.Add(g);
            sadikTabControl1.TabPages[1].Controls.Add(gg);
            sadikTabControl1.TabPages[2].Controls.Add(GatherPie);
            this.Dock = DockStyle.Fill;
            g.Columns.Add("SkillName", "Skills");
            g.Columns.Add("RessourceName", "Ressources");
            g.Columns.Add("RessourceId", "Id");
            g.ReadOnly = true;

            gg.Columns.Add("SkillName", "Skills");
            gg.Columns.Add("RecipeName", "Recettes");
            gg.Columns[1].Width = 200;
            gg.Columns.Add("RecipeId", "Id");
            gg.ReadOnly = true;
        }

        public void ActualizeStats(Dictionary<string, int> ressourcesGathered)
        {
            if (GatherPie.InvokeRequired)
            {
                Invoke(new DelegGatherPie(ActualizeStats), ressourcesGathered);
                return;
            }
            if (GatherPie.Titles.Count < 1)
                GatherPie.Titles.Add("Ressources");
            GatherPie.Series.Clear();        
            GatherPie.ChartAreas[0].BackColor = Color.Transparent;
            Series series1 = new Series
            {
                Name = "series1",
                IsVisibleInLegend = true,
                Color = System.Drawing.Color.DeepSkyBlue,
                ChartType = SeriesChartType.Pie
            };
            GatherPie.Series.Add(series1);
            for (int z = 0; z < ressourcesGathered.Keys.Count; z++)
            {
                if (!job.CanGatherThis(ressourcesGathered.Keys.ToList()[z]))
                    ressourcesGathered.Remove(ressourcesGathered.Keys.ToList()[z]);
            }
            int i =0;
            foreach (KeyValuePair<string,int> pair in ressourcesGathered)
            {
                series1.Points.Add(pair.Value);
                var p1 = series1.Points[i];
                p1.AxisLabel = pair.Key + " (" + pair.Value + ")";
                p1.LegendText = pair.Key;
                i += 1;
            }
            //series1.Points.Add(70000);
            //series1.Points.Add(30000);
            //var p1 = series1.Points[0];
            //p1.AxisLabel = "70000";
            //p1.LegendText = "Hiren Khirsaria";
            //var p2 = series1.Points[1];
            //p2.AxisLabel = "30000";
            //p2.LegendText = "ABC XYZ";
            GatherPie.Invalidate();
        }

        public bool HasRightTool()
        {
            bool h = job.HasRightTool();
            if (!h)
                account.Log(new ErrorTextInformation("L'outil n'est pas équipé :( "),0);
            return job.HasRightTool();
        }
    }
}
