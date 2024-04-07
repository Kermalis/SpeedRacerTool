using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class SplinesChunk
{
	public struct ArrayData1
	{
		public Vector2 Pos;

		internal ArrayData1(EndianBinaryReader r, XDSFile xds)
		{
			Pos = xds.ReadFileVector2(r);
			SRAssert.Equal(r.ReadUInt32(), 0);
			SRAssert.Equal(r.ReadUInt16(), 0);
		}

		internal readonly void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.Append_ArrayElement(index);
			sb.AppendLine(Pos, indent: false);
		}

		public override readonly string ToString()
		{
			return string.Format("({0}f, {1}f)",
				Pos.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC), Pos.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		}
	}
}