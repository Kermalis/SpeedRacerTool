using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiIntegerExtraData : NiExtraData
{
	public readonly uint Data;

	public NiIntegerExtraData(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Data = r.ReadUInt32();
	}
}