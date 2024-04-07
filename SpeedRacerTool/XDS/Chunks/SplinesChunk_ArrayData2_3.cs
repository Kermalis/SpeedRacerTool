using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class SplinesChunk
{
	public struct ArrayData2_3
	{
		public float Val;

		internal ArrayData2_3(EndianBinaryReader r, XDSFile xds, bool mustBeOneIDK)
		{
			Val = xds.ReadFileSingle(r);
			if (mustBeOneIDK)
			{
				SRAssert.Equal(Val, 1f);
			}
			SRAssert.Equal(r.ReadUInt16(), 0);
		}

		internal readonly void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.Append_ArrayElement(index);
			sb.AppendLine(Val, indent: false);
		}

		public override readonly string ToString()
		{
			return Val.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC) + 'f';
		}
	}
}