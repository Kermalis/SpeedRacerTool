using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Abstract base class for NiObjects that support names, extra data, and time controllers.</summary>
internal abstract class NiObjectNET : NiObject
{
	public readonly StringIndex Name;
	public readonly ChunkRef<NiExtraData>[] ExtraDataList;
	public readonly NullableChunkRef<NiTimeController> Controller;

	protected NiObjectNET(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Name = new StringIndex(r);

		ExtraDataList = new ChunkRef<NiExtraData>[r.ReadInt32()];
		r.ReadArray(ExtraDataList);

		Controller = new NullableChunkRef<NiTimeController>(r);
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

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Name), Name.Resolve(nif));

		sb.NewArray(nameof(ExtraDataList), ExtraDataList.Length);
		for (int i = 0; i < ExtraDataList.Length; i++)
		{
			sb.WriteChunk(i, nif, ExtraDataList[i].Resolve(nif));
		}
		sb.EndArray();

		sb.WriteChunk(nameof(Controller), nif, Controller.Resolve(nif));
	}
}