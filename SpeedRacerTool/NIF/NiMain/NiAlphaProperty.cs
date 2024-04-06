using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiAlphaProperty : NiProperty
{
	/// <summary>Bit 0 : alpha blending enable
	/// Bits 1-4 : source blend mode
	/// Bits 5-8 : destination blend mode
	/// Bit 9 : alpha test enable
	/// Bit 10-12 : alpha test mode
	/// Bit 13 : no sorter flag (disables triangle sorting)
	/// 
	/// blend modes(glBlendFunc):
	/// 0000 GL_ONE
	/// 0001 GL_ZERO
	/// 0010 GL_SRC_COLOR
	/// 0011 GL_ONE_MINUS_SRC_COLOR
	/// 0100 GL_DST_COLOR
	/// 0101 GL_ONE_MINUS_DST_COLOR
	/// 0110 GL_SRC_ALPHA
	/// 0111 GL_ONE_MINUS_SRC_ALPHA
	/// 1000 GL_DST_ALPHA
	/// 1001 GL_ONE_MINUS_DST_ALPHA
	/// 1010 GL_SRC_ALPHA_SATURATE
	/// 
	/// test modes(glAlphaFunc) :
	/// 000 GL_ALWAYS
	/// 001 GL_LESS
	/// 010 GL_EQUAL
	/// 011 GL_LEQUAL
	/// 100 GL_GREATER
	/// 101 GL_NOTEQUAL
	/// 110 GL_GEQUAL
	/// 111 GL_NEVER</summary>
	public readonly ushort Flags;
	/// <summary>Threshold for alpha testing (see: glAlphaFunc)</summary>
	public readonly byte Threshold;

	public NiAlphaProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadUInt16();
		Threshold = r.ReadByte();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags);
		sb.AppendLine(nameof(Threshold), Threshold);
	}
}