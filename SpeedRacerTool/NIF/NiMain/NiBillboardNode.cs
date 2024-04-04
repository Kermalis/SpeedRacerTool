using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBillboardNode : NiNode
{
	public readonly BillboardMode Mode;

	internal NiBillboardNode(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Mode = r.ReadEnum<BillboardMode>();
	}
}