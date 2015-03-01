// Not Generated
using BlueSheep.Common.IO;
using BlueSheep.Engine.Types;

namespace BlueSheep.Common.Protocol.Messages
{
	public class ReloginTokenRequestMessage : Message
	{
		public new const uint ID = 6540;
		public override uint ProtocolID
		{
			get { return ID; }
		}
			
		public override void Serialize(BigEndianWriter writer)
		{
		}

		public override void Deserialize(BigEndianReader reader)
		{
		}

	}

}
