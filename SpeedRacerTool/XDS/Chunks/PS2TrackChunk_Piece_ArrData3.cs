using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	partial class Piece
	{
		public struct ArrData3
		{
			public float[] Data;

			internal ArrData3(EndianBinaryReader r, XDSFile xds)
			{
				Data = new float[8];
				xds.ReadFileSingles(r, Data);

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.NewObject(index);

				sb.NewArray(nameof(Data), Data.Length);
				if (sb.IsVerbose)
				{
					for (int i = 0; i < Data.Length; i++)
					{
						sb.Append_ArrayElement(i);
						sb.AppendLine(Data[i], indent: false);
					}
				}
				sb.EndArray();

				sb.EndObject();
			}
		}
	}
}