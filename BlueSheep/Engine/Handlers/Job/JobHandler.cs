using BlueSheep.Common.IO;
using BlueSheep.Common.Protocol.Messages;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Engine.Types;
using BlueSheep.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueSheep.Engine.Handlers.Job
{
    class JobHandler
    {
         #region Public methods
        [MessageHandler(typeof(JobDescriptionMessage))]
        public static void JobDescriptionMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            JobDescriptionMessage msg = (JobDescriptionMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }

            foreach (JobDescription job in msg.jobsDescription)
            {
                List<int> skills = new List<int>();
                foreach (SkillActionDescription s in job.skills)
                {
                    skills.Add(s.skillId);
                }
                Core.Job.Job j = new Core.Job.Job(job.jobId, skills, account);
                account.Jobs.Add(j);
            }
        }

        [MessageHandler(typeof(JobExperienceMultiUpdateMessage))]
        public static void JobExperienceMultiUpdateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            JobExperienceMultiUpdateMessage msg = (JobExperienceMultiUpdateMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            foreach (JobExperience i in msg.experiencesUpdate)
            {
                foreach (Core.Job.Job j in account.Jobs)
                {
                    if (i.jobId == j.Id)
                    {
                        j.Level = i.jobLevel;
                        j.XP = (int)i.jobXP;
                        j.XpLevelFloor =(int)i.jobXpLevelFloor;
                        j.XpNextLevelFloor = (int)i.jobXpNextLevelFloor;
                        break;
                    }
                }
            }
            account.ActualizeJobs();
            foreach (JobUC j in account.JobsUC)
            {
                j.populateTreeview();
            }
        }

        [MessageHandler(typeof(JobExperienceUpdateMessage))]
        public static void JobExperienceUpdateMessageTreatment(Message message, byte[] packetDatas, AccountUC account)
        {
            JobExperienceUpdateMessage msg = (JobExperienceUpdateMessage)message;

            using (BigEndianReader reader = new BigEndianReader(packetDatas))
            {
                msg.Deserialize(reader);
            }
            JobExperience i = msg.experiencesUpdate;
            foreach (Core.Job.Job j in account.Jobs)
            {
                if (i.jobId == j.Id)
                {
                    j.Level = i.jobLevel;
                    j.XP = (int)i.jobXP;
                    j.XpLevelFloor = (int)i.jobXpLevelFloor;
                    j.XpNextLevelFloor = (int)i.jobXpNextLevelFloor;
                    break;
                }
            }
            foreach (JobUC j in account.JobsUC)
                j.UpdateJob();
         
        }

#endregion
    }
}
