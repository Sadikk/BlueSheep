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
    public partial class AccountFrm : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Container for AccountUC when there is only one account.
        /// </summary>

        #region Fields
        private string m_user;
        private string m_pass;
        private bool m_socket;
        private AccountUC m_UC;
        #endregion

        private delegate void Callback();

        #region Constructors
        public AccountFrm()
        {
            InitializeComponent();
        }

        public AccountFrm(string username, string password, bool socket)
        {
            InitializeComponent();
            // Add the UC
            m_user = username;
            m_pass = password;
            m_socket = socket;
            Init();
        }

        public void Reconnect()
        {
            if (this.Controls[0].InvokeRequired)
            {
                Invoke(new Callback(Reconnect));
                return;
            }                
            this.Controls.Remove(m_UC);
            Init();
        }

        private void Init()
        {
            AccountUC Uc = new AccountUC(m_user, m_pass, m_socket, this);
            m_UC = Uc;
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
            if (m_socket)
                Uc.Init();
            else
                Uc.InitMITM();
        }

        private void SaveConfig(object sender , object e)
        {
            m_UC.ConfigManager.SaveConfig();
        }
        #endregion
    }
}

