using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

/// <summary>Describes a visible scene element with vertices like a mesh, a particle system, lines, etc.</summary>
internal abstract class NiGeometry : NiAVObject
{
	public readonly ChunkRef<NiGeometryData> Data;
	public readonly ChunkRef<UnknownChunk> SkinInstance; // TODO: Ref<NiSkinInstance>
	public readonly MaterialData MaterialData;

	protected NiGeometry(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Data = new ChunkRef<NiGeometryData>(r);
		SkinInstance = new ChunkRef<UnknownChunk>(r);
		MaterialData = new MaterialData(r);
	}
}