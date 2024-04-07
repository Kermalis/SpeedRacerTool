using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PhysicsPropsChunk
{
	partial class Entry
	{
		public struct HeightfieldData
		{
			public short Val;

			internal HeightfieldData(EndianBinaryReader r, XDSFile xds)
			{
				Val = xds.ReadFileInt16(r);
				SRAssert.Equal(r.ReadUInt16(), 0);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.Append_ArrayElement(index);
				sb.AppendLine(Val, indent: false);
			}

			public override readonly string ToString()
			{
				return Val.ToString();
			}
		}
	}
}