using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BlueSheep.Interface
{
    public partial class LoginForm : MetroFramework.Forms.MetroForm
    {
        /// <summary>
        /// Old login form. WARNING : DEPRECATED.
        /// </summary>

        #region Fields
        MainForm mainfrm;
        public string version;
        #endregion

        #region Constructors
        public LoginForm(MainForm main)
        {
            InitializeComponent();
            mainfrm = main;
        }

        public LoginForm(string ver)
        {
            InitializeComponent();
            this.Focus();
            version = ver;
        }
        #endregion

        #region Interface methods
        private void linkLabelForgottenPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bluesheepbot.com");
        }

        private void linkLabelCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bluesheepbot.com/signup.php");
        }

        private void textBoxPassword_GotFocus(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = '*';

            if (textBoxPassword.Text == "Entrez le mot de passe...")
            {
                // Vide la TextBox de commande
                textBoxPassword.Text = string.Empty;

                // Change la mise en forme
                textBoxPassword.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
        }

        private void textBoxAccountName_GotFocus(object sender, EventArgs e)
        {
            if (textBoxAccount.Text == "Entrez le nom d'utilisateur...")
            {
                // Vide la TextBox de commande
                textBoxAccount.Text = string.Empty;

                // Change la mise en forme
                textBoxAccount.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            }
        }

        private void textBoxPassword_LostFocus(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "")
            {
                textBoxPassword.PasswordChar = '\0';

                textBoxPassword.Text = "Entrez le mot de passe...";

                textBoxPassword.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            }
        }

        private void textBoxAccountName_LostFocus(object sender, EventArgs e)
        {
            if (textBoxAccount.Text == "")
            {
                textBoxAccount.Text = "Entrez le nom d'utilisateur...";

                textBoxAccount.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            }
        }

        private void textBoxPassword_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonConnect_Click(null, null);
        }

        private void textBoxAccountName_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonConnect_Click(null, null);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string username = ConvertMD5(textBoxAccount.Text);
            string password = ConvertSHA1(textBoxPassword.Text);

            try
            {
                System.Net.HttpWebRequest WebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create("http://bluesheepbot.com/c.php?mdp=" + username + password);

                System.Net.HttpWebResponse WebResponse = (System.Net.HttpWebResponse)WebRequest.GetResponse();
                System.IO.StreamReader STR = new System.IO.StreamReader(WebResponse.GetResponseStream());
                string ReadSource = STR.ReadToEnd();
                //MessageBox.Show(ReadSource);
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("Account" + "=(\\d+)");
                MatchCollection matches = Regex.Matches(ReadSource);
                int nbPremium = 0;
                foreach (Match match in matches)
                {
                    string[] RegSplit = match.ToString().Split('=');
                    nbPremium = Convert.ToInt32(RegSplit[1]);
                }
                mainfrm = new MainForm(version);
                mainfrm.Show();
                Hide();
                
            }
            catch (WebException exception)
            {
                MessageBox.Show("Echec de connexion. Vérifiez votre connexion :\n" + exception.Message);
            }
        }
#endregion

        #region Crypt functions
        //MD5
        private string ConvertMD5(string value)
        {
            MD5 md = MD5.Create();
            byte[] data = md.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        //SHA1
        private string ConvertSHA1(string value)
        {
            SHA1 sha = SHA1.Create();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        //SHA256
        private string ConvertSHA256(string value)
        {
            SHA256 sha = SHA256.Create();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
    }
}
