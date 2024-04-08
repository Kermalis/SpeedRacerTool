using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	partial class Revision
	{
		public struct Change
		{
			public string JoinID;
			public bool UnkBool;

			internal Change(EndianBinaryReader r)
			{
				JoinID = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkBool = r.ReadSafeBoolean();

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.NewObject(index);

				sb.AppendLine(nameof(JoinID), JoinID);
				sb.AppendLine_Boolean(nameof(UnkBool), UnkBool);

				sb.EndObject();
			}

			public override readonly string ToString()
			{
				return JoinID + ' ' + UnkBool;
			}
		}
	}
}