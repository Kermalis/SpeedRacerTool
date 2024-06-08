using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTextureTransformController : NiFloatInterpController
{
	public readonly TexType Slot;
	public readonly TexTransform Operation;

	public NiTextureTransformController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		SRAssert.Equal(r.ReadByte(), 0);

		Slot = r.ReadEnum<TexType>();
		Operation = r.ReadEnum<TexTransform>();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Slot), Slot.ToString());
		sb.AppendLine(nameof(Operation), Operation.ToString());
	}
}