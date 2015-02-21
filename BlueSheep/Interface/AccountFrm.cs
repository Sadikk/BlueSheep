using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BlueSheep.Interface
{
    public partial class AccountFrm : Form
    {
        #region Constructeurs
        public AccountFrm()
        {
            InitializeComponent();
        }
        public AccountFrm(string username, string password, bool socket)
        {
            InitializeComponent();
            AccountUC Uc = new AccountUC(username, password, socket);
            this.Controls.Add(Uc);
            Uc.Show();
            this.Show();
            Uc.IsMaster = false;
            Uc.IsSlave = false;
            Uc.Dock = DockStyle.Fill;
            if (socket)
                Uc.Init();
            else
                Uc.InitMITM();
        }
    }
}
#endregion
