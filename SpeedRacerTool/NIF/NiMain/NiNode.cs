using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Generic node object for grouping.</summary>
internal class NiNode : NiAVObject
{
	public readonly NullableChunkRef<NiAVObject>[] Children;
	public readonly ChunkRef<NiDynamicEffect>[] Effects;

	internal NiNode(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Children = new NullableChunkRef<NiAVObject>[r.ReadUInt32()];
		r.ReadArray(Children);

		Effects = new ChunkRef<NiDynamicEffect>[r.ReadUInt32()];
		r.ReadArray(Effects);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		foreach (NullableChunkRef<NiAVObject> r in Children)
		{
			r.Resolve(nif)?.SetParentAndChildren(nif, this);
		}
		foreach (ChunkRef<NiDynamicEffect> r in Effects)
		{
			r.Resolve(nif).SetParentAndChildren(nif, this);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.NewArray(nameof(Effects), Effects.Length);
		for (int i = 0; i < Effects.Length; i++)
		{
			sb.WriteChunk(i, nif, Effects[i].Resolve(nif));
		}
		sb.EndArray();

		sb.NewArray(nameof(Children), Children.Length);
		for (int i = 0; i < Children.Length; i++)
		{
			sb.WriteChunk(i, nif, Children[i].Resolve(nif));
		}
		sb.EndArray();
	}
}