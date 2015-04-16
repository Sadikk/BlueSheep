using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace BlueSheep.Engine.ExceptionHandler
{
    public partial class ExceptionDialog : MetroFramework.Forms.MetroForm
    {
        public ExceptionDialog()
        {
            InitializeComponent();
        }

        const int _intSpacing = 10;

        private void LaunchLink(string strUrl)
        {
            try
            {
                System.Diagnostics.Process.Start(strUrl);
            }
            catch (System.Security.SecurityException ex)
            {
                //-- do nothing; we can't launch without full trust.
            }
        }

        private void SizeBox(System.Windows.Forms.RichTextBox ctl)
        {
            Graphics g = default(Graphics);
            try
            {
                //-- note that the height is taken as MAXIMUM, so size the label for maximum desired height!
                g = Graphics.FromHwnd(ctl.Handle);
                SizeF objSizeF = g.MeasureString(ctl.Text, ctl.Font, new SizeF(ctl.Width, ctl.Height));
                g.Dispose();
                ctl.Height = Convert.ToInt32(objSizeF.Height) + 5;
            }
            catch (System.Security.SecurityException ex)
            {
                //-- do nothing; we can't set control sizes without full trust
            }
            finally
            {
                if ((g != null))
                    g.Dispose();
            }
        }


        private void UserErrorDialog_Load(System.Object sender, System.EventArgs e)
        {
            //-- make sure our window is on top
            this.TopMost = true;
            this.TopMost = false;

            //-- More >> has to be expanded
            this.txtMore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtMore.Visible = false;

            //-- size the labels' height to accommodate the amount of text in them
            SizeBox(ScopeBox);
            SizeBox(ActionBox);
            SizeBox(ErrorBox);

            //-- now shift everything up
            lblScopeHeading.Top = ErrorBox.Top + ErrorBox.Height + _intSpacing;
            ScopeBox.Top = lblScopeHeading.Top + lblScopeHeading.Height + _intSpacing;

            lblActionHeading.Top = ScopeBox.Top + ScopeBox.Height + _intSpacing;
            ActionBox.Top = lblActionHeading.Top + lblActionHeading.Height + _intSpacing;

            lblMoreHeading.Top = ActionBox.Top + ActionBox.Height + _intSpacing;
            btnMore.Top = lblMoreHeading.Top - 3;

            this.Height = btnMore.Top + btnMore.Height + _intSpacing + 45;

            this.CenterToScreen();
        }

        public void SendMail()
        {
            SimpleMail.SMTPClient smtp = new SimpleMail.SMTPClient();
            MailMessage mm = new MailMessage("donotreply@domain.com", "errorsheep@gmail.com");
            mm.Body = txtMore.Text;
            mm.Subject = ErrorBox.Text;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            smtp.SendMail(mm);
        }


        private void ErrorBox_LinkClicked(System.Object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            LaunchLink(e.LinkText);
        }

        private void ScopeBox_LinkClicked(System.Object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            LaunchLink(e.LinkText);
        }

        private void ActionBox_LinkClicked(System.Object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            LaunchLink(e.LinkText);
        }

        private void btnMore_Click_1(object sender, EventArgs e)
        {
            if (btnMore.Text == ">>")
            {
                this.Height = this.Height + 300;
                var _with1 = txtMore;
                _with1.Location = new System.Drawing.Point(lblMoreHeading.Left, lblMoreHeading.Top + lblMoreHeading.Height + _intSpacing);
                _with1.Height = this.ClientSize.Height - txtMore.Top - 45;
                _with1.Width = this.ClientSize.Width - 2 * _intSpacing;
                _with1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                _with1.Visible = true;
                btnMore.Text = "<<";
            }
            else
            {
                this.SuspendLayout();
                btnMore.Text = ">>";
                this.Height = btnMore.Top + btnMore.Height + _intSpacing + 45;
                txtMore.Visible = false;
                txtMore.Anchor = AnchorStyles.None;
                this.ResumeLayout();
            }
        }

        private void sadikButton1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
