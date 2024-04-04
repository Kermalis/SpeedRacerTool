using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Abstract base class for NiObjects that support names, extra data, and time controllers.</summary>
internal abstract class NiObjectNET : NiObject
{
	public readonly StringIndex Name;
	public readonly ChunkRef<NiExtraData>[] ExtraDataList;
	public readonly NullableChunkRef<NIFUnknownChunk> Controller; // TODO: Ref<NiTimeController>

	protected NiObjectNET(EndianBinaryReader r, int offset)
		: base(offset)
	{
		Name = new StringIndex(r);

		ExtraDataList = new ChunkRef<NiExtraData>[r.ReadInt32()];
		ChunkRef<NiExtraData>.ReadArray(r, ExtraDataList);

		Controller = new NullableChunkRef<NIFUnknownChunk>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		foreach (ChunkRef<NiExtraData> r in ExtraDataList)
		{
			r.Resolve(nif).SetParentAndChildren(nif, this);
		}
		Controller.Resolve(nif)?.SetParentAndChildren(nif, this);
	}
}