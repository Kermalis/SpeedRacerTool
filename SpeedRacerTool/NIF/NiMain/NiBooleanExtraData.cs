using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBooleanExtraData : NiExtraData
{
	public readonly bool Data;

	public NiBooleanExtraData(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Data = r.ReadBoolean();
	}
}