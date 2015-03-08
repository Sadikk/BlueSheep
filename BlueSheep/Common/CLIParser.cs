using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Engine.Network;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using BlueSheep.Interface.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Common
{
    class CLIParser
    {
        /* Command Line Parser */

        #region Fields
        private static AccountUC account;

        /// <summary>
        /// The character used to distinguish which command lines parameters.
        /// </summary>
        private const string PARAM_SEPARATOR = "-";

        /// <summary>
        /// Stores the name=value required parameters.
        /// </summary>
        private static Dictionary<string, string> requiredParameters;

        /// <summary>
        /// Optional parameters.
        /// </summary>
        private static Dictionary<string, string> optionalParameters;

        /// <summary>
        /// Stores the list of the supported switches.
        /// </summary>
        private static Dictionary<string, bool> switches;

        /// <summary>
        /// Store the list of missing required parameters.
        /// </summary>
        private static List<string> missingRequiredParameters;

        /// <summary>
        /// Store the list of missing values of parameters.
        /// </summary>
        private static List<string> missingValue;

        /// <summary>
        /// Contains the raw arguments.
        /// </summary>
        private static List<string> rawArguments;

        /// <summary>
        /// Store the result of the command in order to display it.
        /// </summary>
        private static string result;
        #endregion

        #region Public Methods
        /// <summary>
        /// Main function that parse a given line.
        /// </summary>
        /// <param name="expectedParams">
        /// The list of the required parameters.
        /// </param>
        /// <returns>The string to display.</returns>
        public string Parse(string cmdLine)
        {
            result = "";
            string[] split = cmdLine.Split(' ');
            switch (split[0])
            {
                case "/help":
                    return Usage(cmdLine.Split(' ')[1]);
                case "/move":
                    DefineOptionalParameter(new string[] { "-cell = 0", "-dir = null"});
                    ParseArguments(DeleteCommand(split));
                    return Move();
                case "/mapid":
                    return "L'id de la map est : " + account.Map.Id;
                case "/cell":
                    DefineOptionalParameter(new string[] {"-npc = 0", "-elem = 0", "-player = null"} );
                    ParseArguments(DeleteCommand(split));
                    return Cell();
                case "/say":
                    DefineRequiredParameters(new string[] { "-canal", "-message" });
                    DefineOptionalParameter(new string[] { "-dest = null" });
                    ParseArguments(DeleteCommand(split));
                    return Say();
            }
            return Usage();
        }



        
        #endregion

        #region Private Methods
        /// <summary>
        /// Define the required parameters that the user of the program
        /// must provide.
        /// </summary>
        /// <param name="expectedParams">
        /// The list of the required parameters.
        /// </param>
        private static void DefineRequiredParameters(string[] requiredParameterNames)
        {
            CLIParser.requiredParameters = new Dictionary<string, string>();

            foreach (string param in requiredParameterNames)
            {
                string temp = param;
                temp = temp.Trim();
                if (string.IsNullOrEmpty(param))
                {
                    string errorMessage = "Error: The required command line parameter '" + param + "' is empty.";
                    throw new Exception(errorMessage);
                }

                CLIParser.requiredParameters.Add(param, string.Empty);
            }
        }

        /// <summary>
        /// Define the optional parameters. The parameters must be provided with their
        /// default values in the following format "paramName=paramValue".
        /// </summary>
        /// <param name="optionalParameters">
        /// The list of the optional parameters with their default values.
        /// </param>
        private static void DefineOptionalParameter(string[] optionalPerams)
        {
            CLIParser.optionalParameters = new Dictionary<string, string>();

            foreach (string param in optionalPerams)
            {
                string[] tokens = param.Split('=');

                if (tokens.Length != 2)
                {
                    string errorMessage = "Error: The optional command line parameter '" + param + "' has wrong format.\n Expeted param=value.";
                    throw new Exception(errorMessage);
                }

                tokens[0] = tokens[0].Trim();
                if (string.IsNullOrEmpty(tokens[0]))
                {
                    string errorMessage = "Error: The optional command line parameter '" + param + "' has empty name.";
                    throw new Exception(errorMessage);
                }

                tokens[1] = tokens[1].Trim();
                if (string.IsNullOrEmpty(tokens[1]))
                {
                    string errorMessage = "Error: The optional command line parameter '" + param + "' has no value.";
                }

                CLIParser.optionalParameters.Add(tokens[0], tokens[1]);
            }
        }

        /// <summary>
        /// Define the optional parameters. The parameters must be provided with their
        /// default values.
        /// </summary>
        /// <param name="optionalParameters">
        /// The list of the optional parameters with their default values.
        /// </param>
        private static void DefineOptionalParameter(KeyValuePair<string, string>[] optionalParameters)
        {
            CLIParser.optionalParameters = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> param in optionalParameters)
            {
                string key = param.Key;
                key = key.Trim();

                string value = param.Value;
                value = value.Trim();

                if (string.IsNullOrEmpty(key))
                {
                    string errorMessage = "Error: The name of the optional parameter '" + param.Key + "' is empty.";
                    throw new Exception(errorMessage);
                }

                if (string.IsNullOrEmpty(value))
                {
                    string errorMessage = "Error: The value of the optional parameter '" + param.Key + "' is empty.";
                    throw new Exception(errorMessage);
                }

                CLIParser.optionalParameters.Add(param.Key, param.Value);
            }
        }

        /// <summary>
        /// Defines the supported command line switches. Switch is a parameter
        /// without value. When provided it is used to switch on a given feature or
        /// functionality provided by the application. For example a switch for tracing.
        /// </summary>
        /// <param name="switches"></param>
        private static void DefineSwitches(string[] switches)
        {
            CLIParser.switches = new Dictionary<string, bool>(switches.Length);

            foreach (string sw in switches)
            {
                string temp = sw;
                temp = temp.Trim();

                if (string.IsNullOrEmpty(temp))
                {
                    string errorMessage = "Error: The switch '" + sw + "' is empty.";
                    throw new Exception(errorMessage);
                }

                CLIParser.switches.Add(temp, false);
            }
        }

        /// <summary>
        /// Parse the command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        private static void ParseArguments(string[] args)
        {
            rawArguments = new List<string>(args);

            missingRequiredParameters = new List<string>();
            missingValue = new List<string>();

            ParseRequiredParameters();
            ParseOptionalParameters();
            ParseSwitches();

            ThrowIfErrors();
        }

        /// <summary>
        /// Returns the value of the specified parameter.
        /// </summary>
        /// <param name="paramName">The name of the perameter.</param>
        /// <returns>The value of the parameter.</returns>
        private static string GetParamValue(string paramName)
        {
            string paramValue = string.Empty;

            if (requiredParameters.ContainsKey(paramName))
            {
                paramValue = requiredParameters[paramName];
            }
            else if (optionalParameters.ContainsKey(paramName))
            {
                paramValue = optionalParameters[paramName];
            }
            else
            {
                string errorMessage = "Error: The parameter '" + paramName + "' is not supported.";
                throw new Exception(errorMessage);
            }

            return paramValue;
        }

        private static bool IsSwitchOn(string switchName)
        {
            bool switchValue = false;

            if (switches.ContainsKey(switchName))
            {
                switchValue = switches[switchName];
            }
            else
            {
                string errorMessage = "Error: switch '" + switchName + "' not supported.";
                throw new Exception(errorMessage);
            }

            return switchValue;
        }

        private static void ParseRequiredParameters()
        {
            if (CLIParser.requiredParameters == null || CLIParser.requiredParameters.Count == 0)
            {
                return;
            }

            List<string> paramNames = new List<string>(CLIParser.requiredParameters.Keys);

            foreach (string paramName in paramNames)
            {
                int paramInd = rawArguments.IndexOf(paramName);
                if (paramInd < 0)
                {
                    missingRequiredParameters.Add(paramName);
                }
                else
                {
                    if (paramInd + 1 < rawArguments.Count)
                    {
                        //
                        // The argument after the parameter name is expected to be its value.
                        // No check for error is done here.
                        //
                        requiredParameters[paramName] = rawArguments[paramInd + 1];

                        rawArguments.RemoveAt(paramInd);
                        rawArguments.RemoveAt(paramInd);
                    }
                    else
                    {
                        missingValue.Add(paramName);
                        rawArguments.RemoveAt(paramInd);
                    }
                }
            }
        }

        private static void ParseOptionalParameters()
        {
            if (CLIParser.optionalParameters == null || CLIParser.optionalParameters.Count == 0)
            {
                return;
            }

            List<string> paramNames = new List<string>(CLIParser.optionalParameters.Keys);

            foreach (string paramName in paramNames)
            {
                int paramInd = rawArguments.IndexOf(paramName);

                if (paramInd >= 0)
                {
                    if (paramInd + 1 < rawArguments.Count)
                    {
                        optionalParameters[paramName] = rawArguments[paramInd + 1];

                        rawArguments.RemoveAt(paramInd);

                        //
                        // After removing the param name, the index of the value
                        // becomes again paramInd.
                        //
                        rawArguments.RemoveAt(paramInd);
                    }
                    else
                    {
                        missingValue.Add(paramName);
                        rawArguments.RemoveAt(paramInd);
                    }
                }
            }
        }

        private static void ParseSwitches()
        {
            if (CLIParser.switches == null || CLIParser.switches.Count == 0)
            {
                return;
            }

            List<string> paramNames = new List<string>(CLIParser.switches.Keys);

            foreach (string paramName in paramNames)
            {
                int paramInd = rawArguments.IndexOf(paramName);

                if (paramInd >= 0)
                {
                    CLIParser.switches[paramName] = true;
                    rawArguments.RemoveAt(paramInd);
                }
            }
        }

        private static void ThrowIfErrors()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (missingRequiredParameters.Count > 0 || missingValue.Count > 0 || rawArguments.Count > 0)
            {
                errorMessage.Append("Error: Processing Command Line Arguments\n");
            }

            if (missingRequiredParameters.Count > 0)
            {
                errorMessage.Append("Missing Required Parameters\n");
                foreach (string missingParam in missingRequiredParameters)
                {
                    errorMessage.Append("\t" + missingParam + "\n");
                }
            }

            if (missingValue.Count > 0)
            {
                errorMessage.Append("Missing Values\n");
                foreach (string value in missingValue)
                {
                    errorMessage.Append("\t" + value + "\n");
                }
            }

            if (rawArguments.Count > 0)
            {
                errorMessage.Append("Unknown Parameters");
                foreach (string unknown in rawArguments)
                {
                    errorMessage.Append("\t" + unknown + "\n");
                }
            }

            if (errorMessage.Length > 0)
            {
                account.Log(new ErrorTextInformation(errorMessage.ToString()), 0); 
            }
        }

        private static string[] DeleteCommand(string[] cmd)
        {
            List<string> args = cmd.ToList();
            args.RemoveAt(0);
            return args.ToArray();
        }

        /// <summary>
        /// Returns the help of a given command, or the general help if no command is set.
        /// </summary>
        /// <param name="cmd">The name of the command.</param>
        /// <returns>The general help/the command help</returns>
        private static string Usage(string cmd = "")
        {
            StringBuilder sb = new StringBuilder("USAGE:");
            switch (cmd)
            {
                case "":                 
                    sb.AppendLine("    command -arg1 -arg2 ... -argn Value -switch1");
                    sb.AppendLine();
                    sb.AppendLine("Below are the available commands. Type /help with the name of the command for a specific help.");
                    sb.AppendLine("  - move");
                    sb.AppendLine("  - ");
                    sb.AppendLine("  - ");
                    sb.AppendLine("  - ");
                    sb.AppendLine("  - ");
                    sb.AppendLine("EXAMPLE:");
                    sb.AppendLine("1. > /help move");
                    sb.AppendLine("   - Display the help of the move command.");
                    return sb.ToString();
                case "move":
                    sb.AppendLine("/move [-cell <int>] [-dir <string>]");
                    sb.AppendLine("OPTIONS:");
                    sb.AppendLine("  - cell: move to the specified cell.");
                    sb.AppendLine("  - dir : move to the specified direction (right, left, bottom or up).");
                    sb.AppendLine("EXAMPLE:");
                    sb.AppendLine("1. > /move -cell 150");
                    sb.AppendLine("   - Move to the cell 150.");
                    sb.AppendLine();
                    sb.AppendLine("2. > /move -cell 150 -dir right");
                    sb.AppendLine("   - Move to the cell 150 and then move to the map at the right");
                    return sb.ToString();
                case "cell":
                    sb.AppendLine("/cell [-npc <int>] [-elem <int>] [-player <string>]");
                    sb.AppendLine("OPTIONS:");
                    sb.AppendLine("  - npc: Get the cell of the specified npc id.");
                    sb.AppendLine("  - elem : Get the cell of the specified element id.");
                    sb.AppendLine("  - player : Get the cell of the player name.");
                    sb.AppendLine("EXAMPLE:");
                    sb.AppendLine("1. > /cell -npc 10001");
                    sb.AppendLine("   - Get the cell of the npc which has the id 10001.");
                    sb.AppendLine();
                    sb.AppendLine("2. > /cell -player Sadik");
                    sb.AppendLine("   - Get the cell of the player named Sadik.");
                    return sb.ToString();
                case "say":
                    sb.AppendLine("/say -canal <char> -message <string> [-dest <string>]");
                    sb.AppendLine("OPTIONS:");
                    sb.AppendLine("  - canal   : Canal where the message will be displayed (ex: s for general canal).");
                    sb.AppendLine("  - message : Message that will be sent.");
                    sb.AppendLine("  - dest    : The dest. player of the message. Only on private message (-canal w)");
                    sb.AppendLine("EXAMPLE:");
                    sb.AppendLine("1. > /say -canal b -message I sell some things pm me");
                    sb.AppendLine("   - Send the message \"I sell some things pm me in the\" in the business canal.");
                    sb.AppendLine();
                    sb.AppendLine("2. > /say -canal w -message hi -dest Sadik");
                    sb.AppendLine("   - Send the private message \"hi\" to the player named Sadik.");
                    return sb.ToString();

            }
            return "";
        }

        /// <summary>
        /// Do the moving action.
        /// </summary>
        /// <returns>A display string of the action</returns>
        private static string Move()
        {
            int cell = Int32.Parse(CLIParser.GetParamValue("-cell"));
            string dir = CLIParser.GetParamValue("-dir");
            result = "";

            try
            {
                if (cell != 0)
                {
                    account.Map.MoveToCell(cell);
                    result += "Déplacement vers la cellid : " + cell + "\n";
                }

                if (dir != "null")
                {
                    account.Map.ChangeMap(dir);
                    result += "Déplacement vers la/le : " + dir + "\n";
                }
            }
            catch (Exception ex)
            {
                result += "[Error] " + ex.Message + "\n";
                return result;
            }
            if (result == "")
                return Usage("move");
            else
                return result;
        }

        /// <summary>
        /// Get the cell of a specified element.
        /// </summary>
        /// <returns>The cell of the specified element.</returns>
        private static string Cell()
        {
            int npcid = Int32.Parse(CLIParser.GetParamValue("-npc"));
            int elemid = Int32.Parse(CLIParser.GetParamValue("-elem"));
            string player = CLIParser.GetParamValue("-player");

            try
            {
                if (npcid != 0)
                {
                    string name = account.Npc.GetNpcName(npcid);
                    int cell = account.Map.GetCellFromContextId(npcid);
                    if (cell != 0)
                        result += "Le pnj " + name + " est à la cellule " + cell + ". \n";
                    else
                        result += "Pnj introuvable.";
                }
                else if (elemid != 0)
                {
                    int cell = account.Map.GetCellFromContextId(elemid);
                    if (cell != 0)
                        result += "L'élement " + elemid + " est à la cellule " + cell + ". \n";
                    else
                        result += "Cellule introuvable.";
                }
                else if (player != "null")
                {
                    int cell = 0;
                    foreach (BlueSheep.Common.Protocol.Types.GameRolePlayCharacterInformations p in account.Map.Players.Values)
                    {
                        if (p.name == player)
                        {
                            cell = account.Map.GetCellFromContextId(p.contextualId);
                        }
                    }
                    if (cell != 0)
                        result += "Le joueur " + player + " est à la cellule " + cell + ". \n";
                    else
                        result += "Joueur introuvable.";
                }
            }
            catch (Exception ex)
            {
                result += "[Error] " + ex.Message + "\n";
                return result;
            }
            if (result == "")
                return Usage("cell");
            else
                return result;
        }

        /// <summary>
        /// Say a message in the specified canal.
        /// </summary>
        /// <returns>The result.</returns>
        private static string Say()
        {
            char canal = char.Parse(CLIParser.GetParamValue("-canal"));
            string message = CLIParser.GetParamValue("-message");
            string dest = CLIParser.GetParamValue("-dest");

            try
            {
                if (canal == 'w' && dest != "null")
                {
                    using (BigEndianWriter writer = new BigEndianWriter())
                    {
                        ChatClientPrivateMessage msg = new ChatClientPrivateMessage(message, dest);
                        msg.Serialize(writer);
                        writer.Content = account.HumanCheck.hash_function(writer.Content);
                        MessagePackaging pack = new MessagePackaging(writer);
                        pack.Pack((int)msg.ProtocolID);
                        account.SocketManager.Send(pack.Writer.Content);
                        result += "à " + dest + " : " + message + "\n";
                    }
                }
                else
                {
                    switch (canal)
                    {
                        case 'g':
                            account.Flood.SendMessage(2, message);
                            result += "Message envoyé. \n";
                            break;
                        case 'r':
                            account.Flood.SendMessage(6, message);
                                result += "Message envoyé. \n";
                            break;
                        case 'b':
                            account.Flood.SendMessage(5, message);
                            result += "Message envoyé. \n";
                            break;
                        case 'a':
                            account.Flood.SendMessage(3, message);
                            result += "Message envoyé. \n";
                            break;
                        case 's':
                            account.Flood.SendMessage(0, message);
                            result += "Message envoyé. \n";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result += "[Error] " + ex.Message + "\n";
                return result;
            }
            if (result == "")
                return Usage("say");
            else
                return result;
        }
        #endregion
    }

        

        
    
}
