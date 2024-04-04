using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRGbGeometryGroup : NiAVObject
{
	public readonly byte[] Data;

	public SRGbGeometryGroup(EndianBinaryReader r, int offset, uint chunkSize)
		: base(r, offset)
	{
		long end = offset + chunkSize;
		Data = new byte[end - r.Stream.Position];
		r.ReadBytes(Data);
	}
}