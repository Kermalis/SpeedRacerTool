using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiStencilProperty : NiProperty
{
	/// <summary>Bit 0: Stencil Enable.
	/// Bits 1-3: Fail Action.
	/// Bits 4-6: Z Fail Action.
	/// Bits 7-9: Pass Action.
	/// Bits 10-11: Draw Mode.
	/// Bits 12-14: Stencil Function.</summary>
	public readonly ushort Flags;
	public readonly uint Mask;

	public NiStencilProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadUInt16();
		SRAssert.Equal(Flags, 0x4D80);

		SRAssert.Equal(r.ReadUInt32(), 0);

		Mask = r.ReadUInt32();
		SRAssert.Equal(Mask, uint.MaxValue);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags.ToString());
		sb.AppendLine(nameof(Mask), Mask);
	}
}