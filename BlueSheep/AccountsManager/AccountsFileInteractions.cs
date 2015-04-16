using BlueSheep.Common.Cryptography;
using BlueSheep.Common.IO;
using BlueSheep.Common.Types;
using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace BlueSheep.AccountsManager
{
    class AccountsFileInteractions
    {
        #region Fields
        private List<Account> m_Accounts = new List<Account>();
        private List<AccountUC> m_GroupAccounts = new List<AccountUC>();
        public List<Group> Groups = new List<Group>();
        private readonly string m_SavingFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\accounts.bs";
        private readonly string m_SavingDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep";
        private readonly string m_SavingGroupDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BlueSheep\Groups";
        #endregion

        #region Properties
        public List<Account> Accounts
        {
            get { return m_Accounts; }
        }
        #endregion

        #region Public methods
        public void SaveAccountsInfos(List<Bot> bots)
        {
            foreach (Bot bot in bots)
            {
                bot.Account.Password = CryptageBS.EncryptBS(bot.Account.Password);
                m_Accounts.Add(bot.Account);
            }
            if (!Directory.Exists(m_SavingDirectoryPath))
                Directory.CreateDirectory(m_SavingDirectoryPath);
            using (BigEndianWriter writer = new BigEndianWriter())
            {
                writer.WriteInt(m_Accounts.Count);
                foreach (Account accountObject in m_Accounts)
                {
                    writer.WriteUTF(accountObject.Name);
                    writer.WriteUTF(accountObject.Password);
                }
                IFormatter binaryFormatter = new BinaryFormatter();
                using (Stream stream = new FileStream(m_SavingFilePath, FileMode.Create, FileAccess.Write))
                {
                    binaryFormatter.Serialize(stream, writer);
                }
            }
            foreach (Bot bot in bots)
                bot.Account.Password = CryptageBS.DecryptBS(bot.Account.Password);
        }
        public void RecoverAccountsInfos()
        {
            if (File.Exists(m_SavingFilePath))
            {
                IFormatter binaryFormatter = new BinaryFormatter();
                using (Stream stream = new FileStream(m_SavingFilePath, FileMode.Open, FileAccess.Read))
                {
                    BigEndianWriter writer = (BigEndianWriter)binaryFormatter.Deserialize(stream);
                    using (BigEndianReader reader = new BigEndianReader(writer.Content))
                    {
                        int limite = reader.ReadInt();
                        m_Accounts = new List<Account>();
                        for (int index = 0; index < limite; index++)
                            m_Accounts.Add(new Account(reader.ReadUTF(), reader.ReadUTF()));
                    }
                    writer.Dispose();
                    stream.Close();
                }
            }
            foreach (Account accountObject in m_Accounts)
                accountObject.Password = CryptageBS.DecryptBS(accountObject.Password);
        }
        public void SaveGroup(List<AccountUC> accounts, string groupname)
        {
            //m_GroupAccounts.Clear();
            //foreach (AccountUC bot in accounts)
            //{
            //    bot.AccountPassword = CryptageBS.EncryptBS(bot.AccountPassword);
            //    m_GroupAccounts.Add(bot);
            //}
            if (!Directory.Exists(m_SavingGroupDirectoryPath))
                Directory.CreateDirectory(m_SavingGroupDirectoryPath);
            //if (!File.Exists(m_SavingDirectoryPath + @"\" + groupname))
            // File.Create(m_SavingDirectoryPath + @"\" + groupname);
            m_GroupAccounts = accounts;
            using (BigEndianWriter writer = new BigEndianWriter(File.Create(Path.Combine(m_SavingGroupDirectoryPath, groupname))))
            {
                writer.WriteInt(m_GroupAccounts.Count);
                foreach (AccountUC accountObject in m_GroupAccounts)
                {
                    writer.WriteUTF(accountObject.AccountName);
                    writer.WriteUTF(CryptageBS.EncryptBS(accountObject.AccountPassword));
                }
                //IFormatter binaryFormatter = new BinaryFormatter();
                //using (Stream stream = new FileStream(m_SavingGroupDirectoryPath + @"\" + groupname, FileMode.Create, FileAccess.Write))
                //{
                //    binaryFormatter.Serialize(stream, writer);
                //}
            }
            //foreach (AccountUC bot in accounts)
            //    bot.AccountPassword = CryptageBS.DecryptBS(bot.AccountPassword);
        }
        public void RecoverGroups()
        {
            foreach (FileInfo file in new DirectoryInfo(m_SavingGroupDirectoryPath).GetFiles())
            {
                //IFormatter binaryFormatter = new BinaryFormatter();
                //using (Stream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                //{
                //    BigEndianWriter writer = (BigEndianWriter)binaryFormatter.Deserialize(stream);
                //    using (BigEndianReader reader = new BigEndianReader(writer.Content))
                //    {
                //        int limite = reader.ReadInt();
                //        m_GroupAccounts = new List<AccountUC>();
                //        for (int index = 0; index < limite; index++)
                //            m_GroupAccounts.Add(new AccountUC(reader.ReadUTF(), reader.ReadUTF(), false));
                //        Groups.Add(new Group(m_GroupAccounts, file.Name.Remove((int)file.Name.Length - 3)));
                //        foreach (Account accountObject in m_Accounts)
                //            accountObject.Password = CryptageBS.DecryptBS(accountObject.Password);
                //    }
                //    writer.Dispose();
                //    stream.Close();
                //}
                byte[] content = File.ReadAllBytes(file.FullName);
                using (BigEndianReader reader = new BigEndianReader(content))
                {
                    int limite = reader.ReadInt();
                    m_GroupAccounts = new List<AccountUC>();
                    for (int index = 0; index < limite; index++)
                        m_GroupAccounts.Add(new AccountUC(reader.ReadUTF(), reader.ReadUTF(), false));
                    Groups.Add(new Group(m_GroupAccounts, file.Name.Remove((int)file.Name.Length - 3)));
                    foreach (AccountUC accountObject in m_GroupAccounts)
                        accountObject.AccountPassword = CryptageBS.DecryptBS(accountObject.AccountPassword);
                }
            }
        }
        #endregion
    }
}
