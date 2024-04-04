using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRGbGeometryGroup : NiAVObject
{
	// TODO
	public readonly byte[] Data;

	public SRGbGeometryGroup(EndianBinaryReader r, int index, int offset, uint chunkSize)
		: base(r, index, offset)
	{
		long end = offset + chunkSize;
		Data = new byte[end - r.Stream.Position];
		r.ReadBytes(Data);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(SRGbGeometryGroup));
	}
}