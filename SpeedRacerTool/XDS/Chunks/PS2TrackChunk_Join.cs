using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	public sealed class Join
	{
		public string JoinID;
		public string Piece1;
		public string Piece2;

		internal Join(EndianBinaryReader r)
		{
			JoinID = r.ReadString_Count_TrimNullTerminators(0x20);
			Piece1 = r.ReadString_Count_TrimNullTerminators(0x24);
			Piece2 = r.ReadString_Count_TrimNullTerminators(0x24);
			SRAssert.Equal(r.ReadUInt16(), 0x0000);
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(JoinID), JoinID);
			sb.AppendLine(nameof(Piece1), Piece1);
			sb.AppendLine(nameof(Piece2), Piece2);

			sb.EndObject();
		}
	}
}