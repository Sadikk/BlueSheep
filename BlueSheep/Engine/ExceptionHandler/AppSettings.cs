using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

//--
//-- Class for returning general settings related to our Application, such as..
//--
//--  ** .config file settings
//--  ** runtime version
//--  ** application version
//--  ** version and build date
//--  ** whether we're being debugged or not
//--  ** our security zone
//--  ** our path and filename
//--  ** any command line arguments we have
//--
//--
//-- Jeff Atwood
//-- http://www.codinghorror.com
//--
namespace BlueSheep.Engine.ExceptionHandler
{
    public class AppSettings
    {

        private static string _strAppBase;
        private static string _strConfigPath;
        private static string _strSecurityZone;
        private static string _strRuntimeVersion;
        private static System.Collections.Specialized.NameValueCollection _objCommandLineArgs = null;

        private static System.Collections.Specialized.NameValueCollection _objAssemblyAttribs = null;
        private AppSettings()
        {
            // to keep this class from being creatable as an instance.
        }

        #region "Properties"

        public static bool DebugMode
        {
            get
            {
                if (CommandLineArgs["debug"] == null)
                {
                    return System.Diagnostics.Debugger.IsAttached;
                }
                else
                {
                    return true;
                }
            }
        }

        public static string AppBuildDate
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["BuildDate"];
            }
        }

        public static string AppProduct
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Product"];
            }
        }

        public static string AppCompany
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Company"];
            }
        }

        public static string AppCopyright
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Copyright"];
            }
        }

        public static string AppDescription
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Description"];
            }
        }

        public static string AppTitle
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Title"];
            }
        }

        public static string AppFileName
        {
            get { return Regex.Match(AppPath, "[^/]*.(exe|dll)", RegexOptions.IgnoreCase).ToString(); }
        }

        public static string AppPath
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["CodeBase"];
            }
        }

        public static string AppFullName
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["FullName"];
            }
        }

        public static System.Collections.Specialized.NameValueCollection CommandLineArgs
        {
            get
            {
                if (_objCommandLineArgs == null)
                {
                    _objCommandLineArgs = GetCommandLineArgs();
                }
                return _objCommandLineArgs;
            }
        }

        public static bool CommandLineHelpRequested
        {
            get
            {
                if (_objCommandLineArgs == null)
                {
                    _objCommandLineArgs = GetCommandLineArgs();
                }
                if (!_objCommandLineArgs.HasKeys())
                {
                    return false;
                }

                const string strHelpRegEx = "^(help|\\?)";

                string strKey = null;
                foreach (string strKey_loopVariable in _objCommandLineArgs.AllKeys)
                {
                    strKey = strKey_loopVariable;
                    if (Regex.IsMatch(strKey, strHelpRegEx, RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                    if (Regex.IsMatch(_objCommandLineArgs[strKey], strHelpRegEx, RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static string RuntimeVersion
        {
            get
            {
                if (_strRuntimeVersion == null)
                {
                    //-- returns 1.0.3705.288; we don't want that much detail
                    _strRuntimeVersion = Regex.Match(System.Environment.Version.ToString(), "\\d+.\\d+.\\d+").ToString();
                }
                return _strRuntimeVersion;
            }
        }

        public static string AppVersion
        {
            get
            {
                if (_objAssemblyAttribs == null)
                {
                    _objAssemblyAttribs = GetAssemblyAttribs();
                }
                return _objAssemblyAttribs["Version"];
            }
        }

        public static string ConfigPath
        {
            get
            {
                if (_strConfigPath == null)
                {
                    _strConfigPath = Convert.ToString(System.AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE"));
                }
                return _strConfigPath;
            }
        }

        public static string AppBase
        {
            get
            {
                if (_strAppBase == null)
                {
                    _strAppBase = Convert.ToString(System.AppDomain.CurrentDomain.GetData("APPBASE"));
                }
                return _strAppBase;
            }
        }

        public static string SecurityZone
        {
            get
            {
                if (_strSecurityZone == null)
                {
                    _strSecurityZone = System.Security.Policy.Zone.CreateFromUrl(AppBase).SecurityZone.ToString();
                }
                return _strSecurityZone;
            }
        }

        #endregion

        private static Assembly GetEntryAssembly()
        {
            if (Assembly.GetEntryAssembly() == null)
            {
                return Assembly.GetCallingAssembly();
            }
            else
            {
                return Assembly.GetEntryAssembly();
            }
        }

        //--
        //-- returns string name / string value pair of all attribs
        //-- for specified assembly
        //--
        //-- note that Assembly* values are pulled from AssemblyInfo file in project folder
        //--
        //-- Product         = AssemblyProduct string
        //-- Copyright       = AssemblyCopyright string
        //-- Company         = AssemblyCompany string
        //-- Description     = AssemblyDescription string
        //-- Title           = AssemblyTitle string
        //--
        private static NameValueCollection GetAssemblyAttribs()
        {
            object[] objAttributes = null;
            object objAttribute = null;
            string strAttribName = null;
            string strAttribValue = null;
            NameValueCollection objNameValueCollection = new NameValueCollection();
            System.Reflection.Assembly objAssembly = GetEntryAssembly();

            objAttributes = objAssembly.GetCustomAttributes(false);
            foreach (object objAttribute_loopVariable in objAttributes)
            {
                objAttribute = objAttribute_loopVariable;
                strAttribName = objAttribute.GetType().ToString();
                strAttribValue = "";
                switch (strAttribName)
                {
                    case "System.Reflection.AssemblyTrademarkAttribute":
                        strAttribName = "Trademark";
                        strAttribValue = ((AssemblyTrademarkAttribute)objAttribute).Trademark.ToString();
                        break;
                    case "System.Reflection.AssemblyProductAttribute":
                        strAttribName = "Product";
                        strAttribValue = ((AssemblyProductAttribute)objAttribute).Product.ToString();
                        break;
                    case "System.Reflection.AssemblyCopyrightAttribute":
                        strAttribName = "Copyright";
                        strAttribValue = ((AssemblyCopyrightAttribute)objAttribute).Copyright.ToString();
                        break;
                    case "System.Reflection.AssemblyCompanyAttribute":
                        strAttribName = "Company";
                        strAttribValue = ((AssemblyCompanyAttribute)objAttribute).Company.ToString();
                        break;
                    case "System.Reflection.AssemblyTitleAttribute":
                        strAttribName = "Title";
                        strAttribValue = ((AssemblyTitleAttribute)objAttribute).Title.ToString();
                        break;
                    case "System.Reflection.AssemblyDescriptionAttribute":
                        strAttribName = "Description";
                        strAttribValue = ((AssemblyDescriptionAttribute)objAttribute).Description.ToString();
                        break;
                    default:
                        break;
                    //Console.WriteLine(strAttribName)
                }
                if (!string.IsNullOrEmpty(strAttribValue))
                {
                    if (string.IsNullOrEmpty(objNameValueCollection[strAttribName]))
                    {
                        objNameValueCollection.Add(strAttribName, strAttribValue);
                    }
                }
            }

            //-- add some extra values that are not in the AssemblyInfo, but nice to have
            var _with1 = objNameValueCollection;
            _with1.Add("CodeBase", objAssembly.CodeBase.Replace("file:///", ""));
            _with1.Add("BuildDate", AssemblyBuildDate(objAssembly).ToString());
            _with1.Add("Version", objAssembly.GetName().Version.ToString());
            _with1.Add("FullName", objAssembly.FullName);

            //-- we must have certain assembly keys to proceed.
            if (objNameValueCollection["Product"] == null)
            {
                throw new MissingFieldException("The AssemblyInfo file for the assembly " + objAssembly.GetName().Name + " must have the <Assembly:AssemblyProduct()> key populated.");
            }
            if (objNameValueCollection["Company"] == null)
            {
                throw new MissingFieldException("The AssemblyInfo file for the assembly " + objAssembly.GetName().Name + " must have the <Assembly: AssemblyCompany()>  key populated.");
            }

            return objNameValueCollection;
        }

        //--
        //--
        //-- when this app is loaded via URL, it is possible to pass in "command line arguments" like so:
        //--
        //-- http://localhost/App.Website/App.Loader.exe?a=1&b=2&c=3
        //--
        //-- string[] args = {
        //--     "C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705\IEExec", 
        //--     "http://localhost/WebCommandLine/App.Loader.exe?a=1&b=2&c=3", 
        //--     "3", "1",  "86474707A3C6F63616C686F6374710000000"};
        //--
        //--

        private static void GetURLCommandLineArgs(string strURL, ref NameValueCollection objNameValueCollection)
	{
		MatchCollection objMatchCollection = default(MatchCollection);
		Match objMatch = default(Match);

		//-- http://localhost/App.Website/App.Loader.exe?a=1&b=2&c=apple
		//-- a = 1
		//-- b = 2
		//-- c = apple
		objMatchCollection = Regex.Matches(strURL, "(?<Key>[^=#&?]+)=(?<Value>[^=#&]*)");
		foreach (Match Match in objMatchCollection) {
			objNameValueCollection.Add(Match.Groups["Key"].ToString(), Match.Groups["Value"].ToString());
		}
	}

        private static bool IsURL(string strAny)
        {
            return strAny.IndexOf("&") > -1 || strAny.StartsWith("?") || strAny.ToLower().StartsWith("http://");
        }

        private static string RemoveArgPrefix(string strArg)
        {
            if (strArg.StartsWith("-") | strArg.StartsWith("/"))
            {
                return strArg.Substring(1);
            }
            else
            {
                return strArg;
            }
        }

        //--
        //-- breaks space delimited command line arguments into key value pairs, if they exist
        //--
        //-- App.Loader.exe -remoting=0 /sample=yes c=true
        //-- remoting = 0
        //-- sample   = yes
        //-- c        = true
        //--
        private static bool GetKeyValueCommandLineArg(string strArg, ref NameValueCollection objNameValueCollection)
	{

		MatchCollection objMatchCollection = default(MatchCollection);
		Match objMatch = default(Match);

		objMatchCollection = Regex.Matches(strArg, "(?<Key>^[^=]+)=(?<Value>[^= ]*$)");
		if (objMatchCollection.Count == 0) {
			return false;
		} else {
			foreach (Match Match in objMatchCollection) {
				objNameValueCollection.Add(RemoveArgPrefix(Match.Groups["Key"].ToString()), Match.Groups["Value"].ToString());
			}
			return true;
		}
	}

        //--
        //-- parses command line arguments, handling special case when app was launched via URL
        //-- note that the default .GetCommandLineArgs is SPACE DELIMITED !
        //--
        private static NameValueCollection GetCommandLineArgs()
        {
            string[] strArgs = Environment.GetCommandLineArgs();
            NameValueCollection objNameValueCollection = new NameValueCollection();

            if (strArgs.Length > 0)
            {
                //--
                //-- handles typical case where app was launched via local .EXE
                //--
                string strArg = null;
                int intArg = 0;
                foreach (string strArg_loopVariable in strArgs)
                {
                    strArg = strArg_loopVariable;
                    if (IsURL(strArg))
                    {
                        GetURLCommandLineArgs(strArg, ref objNameValueCollection);
                    }
                    else
                    {
                        if (!GetKeyValueCommandLineArg(strArg, ref objNameValueCollection))
                        {
                            objNameValueCollection.Add("arg" + intArg, RemoveArgPrefix(strArg));
                            intArg += 1;
                        }
                    }
                }
            }

            return objNameValueCollection;
        }

        //--
        //-- exception-safe file attrib retrieval; we don't care if this fails
        //--
        private static DateTime AssemblyFileTime(System.Reflection.Assembly objAssembly)
        {
            try
            {
                return System.IO.File.GetLastWriteTime(objAssembly.Location);
            }
            catch (Exception ex)
            {
                return DateTime.MaxValue;
            }
        }

        //--
        //-- returns build datetime of assembly
        //-- assumes default assembly value in AssemblyInfo:
        //-- <Assembly: AssemblyVersion("1.0.*")> 
        //--
        //-- filesystem create time is used, if revision and build were overridden by user
        //--
        private static DateTime AssemblyBuildDate(System.Reflection.Assembly objAssembly, bool blnForceFileDate = false)
        {
            System.Version objVersion = objAssembly.GetName().Version;
            DateTime dtBuild = default(DateTime);

            if (blnForceFileDate)
            {
                dtBuild = AssemblyFileTime(objAssembly);
            }
            else
            {
                dtBuild = (new DateTime(2000,1,1)).AddDays(objVersion.Build).AddSeconds(objVersion.Revision * 2);
                if (TimeZone.IsDaylightSavingTime(dtBuild, TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuild.Year)))
                {
                    dtBuild = dtBuild.AddHours(1);
                }
                if (dtBuild > DateTime.Now | objVersion.Build < 730 | objVersion.Revision == 0)
                {
                    dtBuild = AssemblyFileTime(objAssembly);
                }
            }

            return dtBuild;
        }

        //-- Returns the specified application value as a boolean
        //-- True values: 1, True, true
        //-- False values: anything else
        public static bool GetBoolean(string key)
        {
            string strTemp = ConfigurationSettings.AppSettings.Get(key);
            if (strTemp == null)
            {
                return false;
            }
            else
            {
                switch (strTemp.ToLower())
                {
                    case "1":
                    case "true":
                        return true;
                    default:
                        return false;
                }
            }
        }

        //-- Returns the specified String value from the application .config file
        public static string GetString(string key)
        {
            string strTemp = Convert.ToString(ConfigurationSettings.AppSettings.Get(key));
            if (strTemp == null)
            {
                return "";
            }
            else
            {
                return strTemp;
            }
        }


        //--
        //-- Returns the specified Integer value from the application .config file
        //--
        public static int GetInteger(string key)
        {
            int intTemp = Convert.ToInt32(ConfigurationSettings.AppSettings.Get(key));
            if (intTemp == null)
            {
                return 0;
            }
            else
            {
                return intTemp;
            }
        }

    }
}
