using System;
using System.Collections.Generic;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Common.Protocol.Types;
using BlueSheep.Core;
using BlueSheep.Engine.Frame;
using BlueSheep.Engine.Network;

namespace BlueSheep.Common.Types
{
    public class Bot
    {
        #region Fields
        private readonly Account m_Account;
        #endregion

        #region Properties
        public Account Account
        {
            get { return m_Account; }
        }

        public bool IsBegun { get; set; }

        public SocketManager SocketManager { get; set; }

        public DateTime NextMeal { get; set; }

        public string Ticket { get; set; }

        public List<Pet> PetsModifiedList { get; set; }

        public List<Pet> PetsList { get; set; }

        public List<int> GiftsList { get; set; }

        public InteractiveElement Safe { get; set; }

        public List<ObjectItem> InventoryItems { get; set; }

        public CharacterBaseInformations CharacterBaseInformations { get; set; }

        public string State { get; set; }

        public short Sequence { get; set; }

        public LatencyFrame LatencyFrame { get; set; }

        public Pods Pods { get; set; }

        public List<ObjectItem> SafeItems { get; set; }

        public Running Running { get; set; }

        public Queue<short> LastPacketID { get; set; }

        public int MapID { get; set; }
        #endregion

        #region Constructeurs
        public Bot(Account account)
        {
            m_Account = account;

            State = "Déconnecté";

            InventoryItems = new List<ObjectItem>();
            SafeItems = new List<ObjectItem>();
        }
        #endregion

        #region Public methods
        public void Initialize()
        {
            NextMeal = new DateTime();
            Ticket = string.Empty;
            PetsModifiedList = null;
            PetsList = null;
            Safe = null;
            CharacterBaseInformations = null;
            Sequence = 0;
            LatencyFrame = null;
            InventoryItems = new List<ObjectItem>();
            Pods = null;
            SafeItems = new List<ObjectItem>();
            LastPacketID = new Queue<short>();
            Running = null;
        }

        public void Dispose()
        {
            Ticket = string.Empty;
            PetsModifiedList = null;
            Sequence = 0;
            LatencyFrame = null;
            InventoryItems = new List<ObjectItem>();
            Pods = null;
            SafeItems = new List<ObjectItem>();
            LastPacketID = new Queue<short>();
            Running = null;
        }

        public void TryFeeding()
        {
            Running = new Running();
            Running.Init();
        }

        public void SetNextMeal()
        {
            foreach (Pet pet in PetsList)
            {
                if (pet.NextMeal.Year == 1)
                    continue;

                if (pet.NonFeededForMissingFood)
                    continue;

                if (NextMeal.Year == 1)
                {
                    NextMeal = pet.NextMeal;
                    continue;
                }

                if (pet.NextMeal <= NextMeal)
                    NextMeal = pet.NextMeal;
            }
        }
        #endregion
    }
}
