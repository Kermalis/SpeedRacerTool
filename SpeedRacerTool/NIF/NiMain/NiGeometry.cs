using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Describes a visible scene element with vertices like a mesh, a particle system, lines, etc.</summary>
internal abstract class NiGeometry : NiAVObject
{
	public readonly ChunkRef<NiGeometryData> Data;
	public readonly NullableChunkRef<NIFUnknownChunk> SkinInstance; // TODO: Ref<NiSkinInstance>
	public readonly MaterialData MaterialData;

	protected NiGeometry(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Data = new ChunkRef<NiGeometryData>(r);
		SkinInstance = new NullableChunkRef<NIFUnknownChunk>(r);

		SRAssert.Equal(SkinInstance.ChunkIndex, -1);

		MaterialData = new MaterialData(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif).SetParentAndChildren(nif, this);
		SkinInstance.Resolve(nif)?.SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(MaterialData), "TODO MaterialData");
		sb.WriteChunk(nameof(SkinInstance), nif, SkinInstance.Resolve(nif));
		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}