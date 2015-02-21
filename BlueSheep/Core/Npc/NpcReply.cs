using BlueSheep.Common.Data.D2o;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Core.Npc
{
        public class NpcReply
        {
            public int Id { get; private set; }
            public int NpcId { get; set; }
            public NpcReply(int npcId, int id)
            {
                NpcId = npcId;
                Id = id;
            }

            public string GetText()
            {
                DataClass npc = GameData.GetDataObject(D2oFileEnum.Npcs, NpcId);
                if (npc == null)
                    return String.Empty;

                ArrayList lreplies = (ArrayList)npc.Fields["dialogReplies"];
                int replies = 0;
                for (int i = 0; i <= lreplies.Count - 1; i++)
                {
                    ArrayList l = (ArrayList)lreplies[i];
                    if ((int)l[0] == Id)
                    {
                        replies = (int)l[1];
                        break;
                    }
                }

                if (replies == null)
                    return String.Empty;

                return BlueSheep.Common.Data.I18N.GetText(replies);


            }
    }
}
