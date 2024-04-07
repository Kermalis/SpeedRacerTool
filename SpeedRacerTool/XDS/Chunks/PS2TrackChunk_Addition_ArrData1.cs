using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	partial class Addition
	{
		public struct ArrData1
		{
			public Vector3 Value;

			internal ArrData1(EndianBinaryReader r, XDSFile xds)
			{
				Value = xds.ReadFileVector3(r);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.Append_ArrayElement(index);
				sb.AppendLine(Value, indent: false);
			}

			public override readonly string ToString()
			{
				return Value.ToString();
			}
		}
	}
}