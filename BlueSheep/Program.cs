using System;
using System.Windows.Forms;
using BlueSheep.Interface;
using Microsoft.Win32;
using System.IO;
using BlueSheep.Engine.ExceptionHandler;

namespace BlueSheep
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args[0] == "ok")
            {
                try
                {
                    UnhandledExceptionManager.AddHandler();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    RegistryKey reg;
                    Registry.CurrentUser.DeleteSubKeyTree("Software\\BlueSheep", false);
                    reg = Registry.CurrentUser.CreateSubKey("Software\\BlueSheep");
                    reg = Registry.CurrentUser.OpenSubKey("Software\\BlueSheep", true);
                    if (reg.ValueCount > 1)
                    {
                        reg.DeleteValue("Version");
                        reg.DeleteValue("Minor");
                        System.Threading.Thread.Sleep(1000);
                    }
                    reg.SetValue("Version", 0.9);
                    reg.SetValue("Minor", "8,1");
                    Application.Run(new MainForm("0.9.7.1"));
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message + ex.StackTrace); }
                
            }
            else
            {
               System.Windows.Forms.MessageBox.Show("Veuillez lancer BlueSheep via l'updater !");
               Environment.Exit(0);
            }

            /* Changelog :
             * Flood : Modos
             * Déplacements : Fix
             * Trajets : Implantation d'un watchdog
             * Trajets : Fix des variables %PODS%
             * */
        }

    }
}
