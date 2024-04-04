using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Describes a visible scene element with vertices like a mesh, a particle system, lines, etc.</summary>
internal abstract class NiGeometry : NiAVObject
{
	public readonly ChunkRef<NiGeometryData> Data;
	public readonly ChunkRef<NIFUnknownChunk> SkinInstance; // TODO: Ref<NiSkinInstance>
	public readonly MaterialData MaterialData;

	protected NiGeometry(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Data = new ChunkRef<NiGeometryData>(r);
		SkinInstance = new ChunkRef<NIFUnknownChunk>(r);

		//SRAssert.Equal(SkinInstance.ChunkIndex, -1);

		MaterialData = new MaterialData(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
		sb.WriteChunk(nameof(SkinInstance), nif, SkinInstance.Resolve(nif));
		sb.AppendLine(nameof(MaterialData), "TODO MaterialData");
	}
}