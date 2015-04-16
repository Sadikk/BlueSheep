using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BlueSheep.Engine.ExceptionHandler
{
    //--
    //-- a simple class for trivial SMTP mail support
    //-- 
    //-- basic features:
    //--
    //--   ** trivial SMTP implementation
    //--   ** HTML body
    //--   ** plain text body
    //--   ** one file attachment
    //--   ** basic retry mechanism
    //--
    //-- Jeff Atwood
    //-- http://www.codinghorror.com
    //--
    public class SimpleMail
    {

        public class SMTPMailMessage
        {
            public string From;
            public string To;
            public string Subject;
            public string Body;
            public string BodyHTML;
            public string AttachmentPath;
        }

        public class SMTPClient
        {

            private const int _intBufferSize = 1024;
            private const int _intResponseTimeExpected = 10;
            private const int _intResponseTimeMax = 750;
            private const string _strAddressSeperator = ";";
            private const int _intMaxRetries = 5;
            private const bool _blnDebugMode = false;

            private const bool _blnPlainTextOnly = false;
            private string _strDefaultDomain = "smtp.gmail.com";
            private string _strServer = "smtp.gmail.com";

            private int _intPort = 25;
            private string _strUserName = "errorsheep@gmail.com";

            private string _strUserPassword = "errorHandler";
            private int _intRetries = 1;

            private string _strLastResponse;
            public string AuthUser
            {
                get { return _strUserName; }
                set { _strUserName = value; }
            }

            public string AuthPassword
            {
                get { return _strUserPassword; }
                set { _strUserPassword = value; }
            }

            public int Port
            {
                get { return _intPort; }
                set { _intPort = value; }
            }

            public string Server
            {
                get { return _strServer; }
                set { _strServer = value; }
            }

            public string DefaultDomain
            {
                get { return _strDefaultDomain; }
                set { _strDefaultDomain = value; }
            }

            //-- 
            //-- send data over the current network connection
            //--
            private void SendData(TcpClient tcp, string strData)
            {
                NetworkStream objNetworkStream = tcp.GetStream();
                byte[] bytWriteBuffer = new byte[strData.Length + 1];
                System.Text.UTF8Encoding en = new System.Text.UTF8Encoding();

                bytWriteBuffer = en.GetBytes(strData);
                objNetworkStream.Write(bytWriteBuffer, 0, bytWriteBuffer.Length);
            }

            //--
            //-- get data from the current network connection
            //--
            private string GetData(TcpClient tcp)
            {
                System.Net.Sockets.NetworkStream objNetworkStream = tcp.GetStream();

                if (objNetworkStream.DataAvailable)
                {
                    byte[] bytReadBuffer = null;
                    int intStreamSize = 0;
                    bytReadBuffer = new byte[_intBufferSize + 1];
                    intStreamSize = objNetworkStream.Read(bytReadBuffer, 0, bytReadBuffer.Length);
                    System.Text.UTF8Encoding en = new System.Text.UTF8Encoding();
                    return en.GetString(bytReadBuffer);
                }
                else
                {
                    return "";
                }
            }

            //--
            //-- issue a required SMTP command
            //--

            private void Command(TcpClient tcp, string strCommand, string strExpectedResponse = "250")
            {
                if (!CommandInternal(tcp, strCommand, strExpectedResponse))
                {
                    tcp.Close();
                    throw new Exception("SMTP server at " + _strServer.ToString() + ":" + _intPort.ToString() + " was provided command '" + strCommand + "', but did not return the expected response '" + strExpectedResponse + "':" + Environment.NewLine + _strLastResponse);
                }

            }

            //--
            //-- issue a SMTP command
            //--
            private bool CommandInternal(TcpClient tcp, string strCommand, string strExpectedResponse = "250")
            {

                int intResponseTime = 0;

                //-- send the command over the socket with a trailing cr/lf
                if (strCommand.Length > 0)
                {
                    SendData(tcp, strCommand + Environment.NewLine);
                }

                //-- wait until we get a response, or time out
                _strLastResponse = "";
                intResponseTime = 0;
                while ((string.IsNullOrEmpty(_strLastResponse)) & (intResponseTime <= _intResponseTimeMax))
                {
                    intResponseTime += _intResponseTimeExpected;
                    _strLastResponse = GetData(tcp);
                    Thread.Sleep(_intResponseTimeExpected);
                }

                //-- this is helpful for debugging SMTP problems
                if (_blnDebugMode)
                {
                    Debug.WriteLine("SMTP >> " + strCommand + " (after " + intResponseTime.ToString() + "ms)");
                    Debug.WriteLine("SMTP << " + _strLastResponse);
                }

                //-- if we have a response, check the first 10 characters for the expected response code
                if (string.IsNullOrEmpty(_strLastResponse))
                {
                    if (_blnDebugMode)
                    {
                        Debug.WriteLine("** EXPECTED RESPONSE " + strExpectedResponse + " NOT RETURNED **");
                    }
                    return false;
                }
                else
                {
                    return (_strLastResponse.IndexOf(strExpectedResponse, 0, 10) != -1);
                }
            }

            //--
            //-- send mail with integrated retry mechanism
            //--
            //public bool SendMail(SMTPMailMessage mail)
            //{
            //    int intRetryInterval = 333;
            //    try
            //    {
            //        SendMailInternal(mail);
            //    }
            //    catch (Exception ex)
            //    {
            //        _intRetries += 1;
            //        if (_intRetries <= _intMaxRetries)
            //        {
            //            Thread.Sleep(intRetryInterval);
            //            SendMail(mail);
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    //Console.WriteLine("sent after " & _intRetries.ToString)
            //    _intRetries = 1;
            //    return true;
            //}

            //--
            //-- send an email via trivial SMTP
            //--
            private void SendMailInternal(SMTPMailMessage mail)
            {
                IPHostEntry iphost = default(IPHostEntry);
                TcpClient tcp = new TcpClient();

                //-- resolve server text name to an IP address
                try
                {
                    iphost = Dns.GetHostByName(_strServer);
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to resolve server name " + _strServer, e);
                }

                //-- attempt to connect to the server by IP address and port number
                try
                {
                    tcp.Connect(iphost.AddressList[0], _intPort);
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to connect to SMTP server at " + _strServer.ToString()+ ":" + _intPort.ToString(), e);
                }

                //-- make sure we get the SMTP welcome message
                Command(tcp, "", "220");
                Command(tcp, "HELO " + Environment.MachineName);

                //--
                //-- authenticate if we have username and password
                //-- http://www.ietf.org/rfc/rfc2554.txt
                //--
                if ((_strUserName + _strUserPassword).Length > 0)
                {
                    Command(tcp, "auth login", "334 VXNlcm5hbWU6");
                    //VXNlcm5hbWU6=base64'Username:'
                   Command(tcp, ToBase64(_strUserName), "334 UGFzc3dvcmQ6");
                    //UGFzc3dvcmQ6=base64'Password:'
                    Command(tcp, ToBase64(_strUserPassword), "235");
                }

                if (string.IsNullOrEmpty(mail.From))
                {
                    mail.From = System.AppDomain.CurrentDomain.FriendlyName.ToLower() + "@" + Environment.MachineName.ToLower() + "." + _strDefaultDomain;
                }
                Command(tcp, "MAIL FROM: <" + mail.From + ">");

                //-- send email to more than one recipient
                string[] arRecipients = mail.To.Split(_strAddressSeperator.ToCharArray());
                string strRecipient = null;
                foreach (string strRecipient_loopVariable in arRecipients)
                {
                    strRecipient = strRecipient_loopVariable;
                    Command(tcp, "RCPT TO: <" + strRecipient + ">");
                }

               Command(tcp, "DATA", "354");

                StringBuilder objStringBuilder = new StringBuilder();
                var _with1 = objStringBuilder;
                //-- write common email headers
                _with1.Append("To: " + mail.To + Environment.NewLine);
                _with1.Append("From: " + mail.From + Environment.NewLine);
                _with1.Append("Subject: " + mail.Subject + Environment.NewLine);

                if (_blnPlainTextOnly)
                {
                    //-- write plain text body
                    _with1.Append(Environment.NewLine + mail.Body + Environment.NewLine);
                }
                else
                {
                    string strContentType = null;
                    //-- typical case; mixed content will be displayed side-by-side
                    strContentType = "multipart/mixed";
                    //-- unusual case; text and HTML body are both included, let the reader determine which it can handle
                    if (!string.IsNullOrEmpty(mail.Body) & !string.IsNullOrEmpty(mail.BodyHTML))
                    {
                        strContentType = "multipart/alternative";
                    }

                    _with1.Append("MIME-Version: 1.0" + Environment.NewLine);
                    _with1.Append("Content-Type: " + strContentType + "; boundary=\"NextMimePart\"" + Environment.NewLine);
                    _with1.Append("Content-Transfer-Encoding: 7bit" + Environment.NewLine);
                    // -- default content (for non-MIME compliant email clients, should be extremely rare)
                    _with1.Append("This message is in MIME format. Since your mail reader does not understand " + Environment.NewLine);
                    _with1.Append("this format, some or all of this message may not be legible." + Environment.NewLine);
                    //-- handle text body (if any)
                    if (!string.IsNullOrEmpty(mail.Body))
                    {
                        _with1.Append(Environment.NewLine + "--NextMimePart" + Environment.NewLine);
                        _with1.Append("Content-Type: text/plain;" + Environment.NewLine);
                        _with1.Append(Environment.NewLine + mail.Body + Environment.NewLine);
                    }
                    // -- handle HTML body (if any)
                    if (!string.IsNullOrEmpty(mail.BodyHTML))
                    {
                        _with1.Append(Environment.NewLine + "--NextMimePart" + Environment.NewLine);
                        _with1.Append("Content-Type: text/html; charset=iso-8859-1" + Environment.NewLine);
                        _with1.Append(Environment.NewLine + mail.BodyHTML + Environment.NewLine);
                    }
                    //-- handle attachment (if any)
                    if (!string.IsNullOrEmpty(mail.AttachmentPath))
                    {
                        _with1.Append(FileToMimeString(mail.AttachmentPath));
                    }
                }
                //-- <crlf>.<crlf> marks end of message content
                _with1.Append(Environment.NewLine + "." + Environment.NewLine);

               Command(tcp, objStringBuilder.ToString());
                Command(tcp, "QUIT", "");
                tcp.Close();
            }

            public void SendMail(MailMessage m)
            {
                try
                {
                    SmtpClient client = new SmtpClient();
                    client.Port = 465;
                    client.Host = "smtp.laposte.net";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("adrien.tosssam@laposte.net", "Baban974");
                    client.Send(m);
                }
                catch (SmtpException)
                {
                }
                
            }

            //--
            //-- turn a file into a base 64 string
            //--
            private string FileToMimeString(string strFilepath)
            {

                System.IO.FileStream objFilestream = default(System.IO.FileStream);
                StringBuilder objStringBuilder = new StringBuilder();
                //-- note that chunk size is equal to maximum line width
                const int intChunkSize = 75;
                byte[] bytRead = new byte[intChunkSize + 1];
                int intRead = 0;
                string strFilename = null;

                //-- get just the filename out of the path
                strFilename = System.IO.Path.GetFileName(strFilepath);

                var _with2 = objStringBuilder;
                _with2.Append(Environment.NewLine + "--NextMimePart" + Environment.NewLine);
                _with2.Append("Content-Type: application/octet-stream; name=\"" + strFilename + "\"" + Environment.NewLine);
                _with2.Append("Content-Transfer-Encoding: base64" + Environment.NewLine);
                _with2.Append("Content-Disposition: attachment; filename=\"" + strFilename + "\"" + Environment.NewLine);
                _with2.Append(Environment.NewLine);

                objFilestream = new System.IO.FileStream(strFilepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                intRead = objFilestream.Read(bytRead, 0, intChunkSize);
                while (intRead > 0)
                {
                    objStringBuilder.Append(System.Convert.ToBase64String(bytRead, 0, intRead));
                    objStringBuilder.Append(Environment.NewLine);
                    intRead = objFilestream.Read(bytRead, 0, intChunkSize);
                }
                objFilestream.Close();

                return objStringBuilder.ToString();
            }

            private static string ToBase64(string data)
            {
                System.Text.UTF8Encoding Encoder = new System.Text.UTF8Encoding();
                return Convert.ToBase64String(Encoder.GetBytes(data));
            }

        }

    }

}
