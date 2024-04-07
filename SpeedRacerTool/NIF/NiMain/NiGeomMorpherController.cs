using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiGeomMorpherController : NiInterpController
{
	public readonly struct MorphWeight
	{
		public readonly ChunkRef<NiInterpController> Interpolator;
		public readonly float Weight;

		internal MorphWeight(EndianBinaryReader r)
		{
			Interpolator = new ChunkRef<NiInterpController>(r);
			Weight = r.ReadSingle();
		}

		public void DebugStr(NIFFile nif, NIFStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine(nameof(Weight), Weight);

			sb.WriteChunk(nameof(Interpolator), nif, Interpolator.Resolve(nif));

			sb.EndObject();
		}
	}

	public readonly ushort ExtraFlags;
	public readonly ChunkRef<NIFUnknownChunk> Data; // TODO: <NiMorphData>
	public readonly byte AlwaysUpdate;
	public readonly MorphWeight[] InterpWeights;

	public NiGeomMorpherController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		ExtraFlags = r.ReadUInt16();
		Data = new ChunkRef<NIFUnknownChunk>(r);
		AlwaysUpdate = r.ReadByte();

		InterpWeights = new MorphWeight[r.ReadUInt32()];
		for (int i = 0; i < InterpWeights.Length; i++)
		{
			InterpWeights[i] = new MorphWeight(r);
		}
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif).SetParentAndChildren(nif, this);

		foreach (MorphWeight m in InterpWeights)
		{
			m.Interpolator.Resolve(nif).SetParentAndChildren(nif, this);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(ExtraFlags), ExtraFlags);
		sb.AppendLine(nameof(AlwaysUpdate), AlwaysUpdate);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));

		sb.NewArray(nameof(InterpWeights), InterpWeights.Length);
		for (int i = 0; i < InterpWeights.Length; i++)
		{
			InterpWeights[i].DebugStr(nif, sb, i);
		}
		sb.EndArray();
	}
}