using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PhysicsPropsChunk
{
	partial class Entry
	{
		public struct HeightfieldData
		{
			// TODO: The values indicate how high off the ground this x/z coordinate is, but what unit?
			// TODO: Also it's probably ushort since some stuff is extremely high off the ground (like lamp posts). Just need to visualize it in blender first
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