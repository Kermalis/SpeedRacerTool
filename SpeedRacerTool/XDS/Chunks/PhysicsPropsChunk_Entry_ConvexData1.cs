using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PhysicsPropsChunk
{
	partial class Entry
	{
		/// <summary>The 3 vertices to use to create a triangle, like how .obj works</summary>
		public struct ConvexData1
		{
			public ushort VertID0;
			public ushort VertID1;
			public ushort VertID2;

			internal ConvexData1(EndianBinaryReader r, XDSFile xds)
			{
				VertID0 = xds.ReadFileUInt16(r);
				VertID1 = xds.ReadFileUInt16(r);
				VertID2 = xds.ReadFileUInt16(r);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.Append_ArrayElement(index);
				sb.AppendLine_NoQuotes(ToString(), indent: false);
			}

			public override readonly string ToString()
			{
				return string.Format("{0}, {1}, {2}", VertID0, VertID1, VertID2);
			}
		}
	}
}