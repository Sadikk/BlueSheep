using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace BlueSheep.Engine.ExceptionHandler
{
    //--
    //-- Generic HANDLED error handling class
    //--
    //-- It's like MessageBox, but specific to handled exceptions, and supports email notifications
    //--
    //-- Jeff Atwood
    //-- http://www.codinghorror.com
    //--
    public class HandledExceptionManager
    {

        private static bool _blnHaveException = false;
        private static bool _blnEmailError = true;
        private static string _strEmailBody;
        private static string _strExceptionType;

        private const string _strDefaultMore = "No further information is available. If the problem persists, contact (contact).";
        public static bool EmailError
        {
            get { return _blnEmailError; }
            set { _blnEmailError = value; }
        }

        public enum UserErrorDefaultButton
        {
            Default = 0,
            Button1 = 1,
            Button2 = 2,
            Button3 = 3
        }

        //-- 
        //-- replace generic constants in strings with specific values
        //--
        private static string ReplaceStringVals(string strOutput)
        {
            string strTemp = null;
            if (strOutput == null)
            {
                strTemp = "";
            }
            else
            {
                strTemp = strOutput;
            }
            strTemp = strTemp.Replace("(app)", AppSettings.AppProduct);
            strTemp = strTemp.Replace("(contact)", AppSettings.GetString("UnhandledExceptionManager/ContactInfo"));
            return strTemp;
        }

        //--
        //-- make sure "More" text is populated with something useful
        //--
        private static string GetDefaultMore(string strMoreDetails)
        {
            if (string.IsNullOrEmpty(strMoreDetails))
            {
                System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
                var _with1 = objStringBuilder;
                _with1.Append(_strDefaultMore);
                _with1.Append(Environment.NewLine);
                _with1.Append(Environment.NewLine);
                _with1.Append("Basic technical information follows: " + Environment.NewLine);
                _with1.Append("---" + Environment.NewLine);
                _with1.Append(UnhandledExceptionManager.SysInfoToString(true));
                return objStringBuilder.ToString();
            }
            else
            {
                return strMoreDetails;
            }
        }

        //--
        //-- converts exception to a formatted "more" string
        //--
        private static string ExceptionToMore(System.Exception objException)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var _with2 = sb;
            if (_blnEmailError)
            {
                _with2.Append("Information about this problem was automatically mailed to ");
                _with2.Append(AppSettings.GetString("UnhandledExceptionManager/EmailTo"));
                _with2.Append(Environment.NewLine + Environment.NewLine);
            }
            _with2.Append("Detailed technical information follows: " + Environment.NewLine);
            _with2.Append("---" + Environment.NewLine);
            string x = UnhandledExceptionManager.ExceptionToString(objException);
            _with2.Append(x);
            return sb.ToString();
        }

        //--
        //-- perform our string replacements for (app) and (contact), etc etc
        //-- also make sure More has default values if it is blank.
        //--       

        private static void ProcessStrings(ref string strWhatHappened, ref string strHowUserAffected, ref string strWhatUserCanDo, ref string strMoreDetails)
        {
            strWhatHappened = ReplaceStringVals(strWhatHappened);
            strHowUserAffected = ReplaceStringVals(strHowUserAffected);
            strWhatUserCanDo = ReplaceStringVals(strWhatUserCanDo);
            strMoreDetails = ReplaceStringVals(GetDefaultMore(strMoreDetails));
        }

        //-- 
        //-- simplest possible error dialog
        //--
        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo)
        {
            _blnHaveException = false;
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, "", MessageBoxButtons.OK, MessageBoxIcon.Warning, UserErrorDefaultButton.Default);
        }

        //--
        //-- advanced error dialog with Exception object
        //--
        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, System.Exception objException, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.Warning, UserErrorDefaultButton DefaultButton = UserErrorDefaultButton.Default)
        {

            _blnHaveException = true;
            _strExceptionType = objException.GetType().FullName;
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, ExceptionToMore(objException), Buttons, Icon, DefaultButton);
        }


        //--
        //-- advanced error dialog with More string
        //-- leave "more" string blank to get the default
        //--
        public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.Warning, UserErrorDefaultButton DefaultButton = UserErrorDefaultButton.Default)
        {

            _blnHaveException = false;
            return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, strMoreDetails, Buttons, Icon, DefaultButton);
        }

        //-- 
        //-- internal method to show error dialog
        //--
        private static DialogResult ShowDialogInternal(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons, MessageBoxIcon Icon, UserErrorDefaultButton DefaultButton)
        {

            //-- set default values, etc
            ProcessStrings(ref strWhatHappened, ref strHowUserAffected, ref strWhatUserCanDo, ref strMoreDetails);

            ExceptionDialog objForm = new ExceptionDialog();
            var _with3 = objForm;
            _with3.Text = ReplaceStringVals(objForm.Text);
            _with3.ErrorBox.Text = strWhatHappened;
            _with3.ScopeBox.Text = strHowUserAffected;
            _with3.ActionBox.Text = strWhatUserCanDo;
            _with3.txtMore.Text = strMoreDetails;

            //-- determine what button text, visibility, and defaults are
            var _with4 = objForm;

            //if (_blnEmailError)
            //{
            SendNotificationEmail(strWhatHappened, strHowUserAffected, strWhatUserCanDo, strMoreDetails);
            //}
            //_with3.SendMail();

            //-- show the user our error dialog
            return objForm.ShowDialog();
        }


        //--
        //-- this is the code that executes in the spawned thread
        //--
        private static void ThreadHandler()
        {
            //SimpleMail.SMTPClient smtp = new SimpleMail.SMTPClient();
            //SimpleMail.SMTPMailMessage mail = new SimpleMail.SMTPMailMessage();
            //var _with5 = mail;
            //_with5.To = AppSettings.GetString("UnhandledExceptionManager/EmailTo");
            //if (_blnHaveException)
            //{
            //    _with5.Subject = "Handled Exception notification - " + _strExceptionType;
            //}
            //else
            //{
            //    _with5.Subject = "HandledExceptionManager notification";
            //}
            //_with5.Body = _strEmailBody;
            ////-- try to send email, but we don't care if it succeeds (for now)
            //try
            //{
            //    smtp.SendMail(mail);
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine("** SMTP email failed to send!");
            //    Debug.WriteLine("** " + e.Message);
            //}
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.laposte.net");

            mail.From = new MailAddress("adrien.tosssam@laposte.net");
            mail.To.Add("adrien.tosssam@laposte.net");
            if (_blnHaveException)
            {
                mail.Subject = "Handled Exception notification - " + _strExceptionType;
            }
            else
            {
                mail.Subject = "HandledExceptionManager notification";
            }
            mail.Body = _strEmailBody;
            SmtpServer.EnableSsl = true;
            SmtpServer.Timeout = 10000;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Port = 465;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("adrien.tosssam@laposte.net", "Baban974");

            SmtpServer.Send(mail);

        }

        //--
        //-- send notification about this error via e-mail
        //--

        private static void SendNotificationEmail(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails)
        {
            //-- ignore debug exceptions (eg, development testing)?
            if (UnhandledExceptionManager.IgnoreDebugErrors)
            {
                if (AppSettings.DebugMode)
                    return;
            }

            System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
            var _with6 = objStringBuilder;
            _with6.Append("What happened:");
            _with6.Append(Environment.NewLine);
            _with6.Append(strWhatHappened);
            _with6.Append(Environment.NewLine);
            _with6.Append(Environment.NewLine);
            _with6.Append("How this will affect the user:");
            _with6.Append(Environment.NewLine);
            _with6.Append(strHowUserAffected);
            _with6.Append(Environment.NewLine);
            _with6.Append(Environment.NewLine);
            _with6.Append("What the user can do about it:");
            _with6.Append(Environment.NewLine);
            _with6.Append(strWhatUserCanDo);
            _with6.Append(Environment.NewLine);
            _with6.Append(Environment.NewLine);
            _with6.Append("More information:");
            _with6.Append(Environment.NewLine);
            _with6.Append(strMoreDetails);
            _with6.Append(Environment.NewLine);
            _with6.Append(Environment.NewLine);
            SendEmail(objStringBuilder.ToString());
        }

        private static void SendEmail(string strEmailBody)
        {
            _strEmailBody = strEmailBody;
            //-- spawn off the email send attempt as a thread for improved throughput
            Thread objThread = new Thread(new ThreadStart(ThreadHandler));
            objThread.Name = "HandledExceptionEmail";
            objThread.Start();
        }

    }
}


