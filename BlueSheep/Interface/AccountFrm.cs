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
        /// <summary>
        /// Container for AccountUC when there is only one account.
        /// </summary>

        #region Constructors
        public AccountFrm()
        {
            InitializeComponent();
        }
        public AccountFrm(string username, string password, bool socket)
        {
            InitializeComponent();
            // Add the UC
            AccountUC Uc = new AccountUC(username, password, socket);
            this.Controls.Add(Uc);
            Uc.Show();

            // Show the form
            this.Show();

            // Not in a group
            Uc.IsMaster = false;
            Uc.IsSlave = false;

            // Fill the account form
            Uc.Dock = DockStyle.Fill;

            // Call socket/mitm init
            if (socket)
                Uc.Init();
            else
                Uc.InitMITM();
        }
        #endregion
    }
}

