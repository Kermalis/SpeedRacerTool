using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PhysicsPropsChunk
{
	partial class Entry
	{
		/// <summary>The vertices that are available to the convex</summary>
		public struct ConvexData2
		{
			public Vector3 Data;

			internal ConvexData2(EndianBinaryReader r, XDSFile xds)
			{
				Data = xds.ReadFileVector3(r);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.Append_ArrayElement(index);
				sb.AppendLine(Data, indent: false);
			}

			public override readonly string ToString()
			{
				return Data.ToString();
			}
		}
	}
}