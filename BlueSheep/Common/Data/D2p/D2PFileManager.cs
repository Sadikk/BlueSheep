using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BlueSheep.Data.D2p
{
    internal sealed class D2pFileManager
    {
        // Methods
        internal D2pFileManager(string Path)
        {
            string File = null;
            foreach (string File_loopVariable in Directory.GetFiles(Path))
            {
                File = File_loopVariable;
                FileInfo info = new FileInfo(File);
                if ((info.Extension.ToUpper() == ".D2P"))
                {
                    this.ListD2pFileDlm.Add(new D2PFileDlm(File));
                }
            }
        }

        internal bool MapExists(string DlmName)
        {
            foreach (D2PFileDlm D2pFileDlm in this.ListD2pFileDlm)
            {
                if (D2pFileDlm.ExistsDlm(DlmName))
                {
                    return true;
                }
            }
            return false;
        }

        internal byte[] method_1(string string_0)
        {
            foreach (D2PFileDlm D2pFileDlm in this.ListD2pFileDlm)
            {
                if (D2pFileDlm.ExistsDlm(string_0))
                {
                    return D2pFileDlm.ReadFile(string_0);
                }
            }
            return null;
        }

        // Fields
        private List<D2PFileDlm> ListD2pFileDlm = new List<D2PFileDlm>();
    }
}
