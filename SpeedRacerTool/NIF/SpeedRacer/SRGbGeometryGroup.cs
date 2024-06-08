using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRGbGeometryGroup : NiAVObject
{
	public readonly ChunkRef<NiTriStripsData> UnkChunk1;
	public readonly ChunkRef<NiTriStrips> UnkChunk2;
	// TODO
	public readonly byte[] Data;

	public SRGbGeometryGroup(EndianBinaryReader r, int index, int offset, uint chunkSize)
		: base(r, index, offset)
	{
		UnkChunk1 = new ChunkRef<NiTriStripsData>(r);

		SRAssert.Equal(r.ReadInt32(), -1);
		SRAssert.Equal(r.ReadInt32(), 0);
		SRAssert.Equal(r.ReadInt32(), -1);

		SRAssert.Equal(r.ReadByte(), 1);

		UnkChunk2 = new ChunkRef<NiTriStrips>(r);

		// Sizes vary
		Data = new byte[chunkSize - 0x4A - 0x15];
		r.ReadBytes(Data);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		UnkChunk1.Resolve(nif).SetParentAndChildren(nif, this);
		UnkChunk2.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteChunk(nameof(UnkChunk1), nif, UnkChunk1.Resolve(nif));
		sb.WriteChunk(nameof(UnkChunk2), nif, UnkChunk2.Resolve(nif));

		sb.WriteTODO(nameof(SRGbGeometryGroup));
	}
}