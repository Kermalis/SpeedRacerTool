using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiExtraData : NiObject
{
	public readonly StringIndex Name;

	protected NiExtraData(EndianBinaryReader r, int offset)
		: base(offset)
	{
		Name = new StringIndex(r);
	}
}