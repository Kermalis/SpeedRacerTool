using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Not sealed
internal class NiParticleSystem : NiParticles
{
	public readonly bool IsWorldSpace;
	public readonly ChunkRef<NIFUnknownChunk>[] Modifiers; // TODO: Ref<NiPSysModifier>

	internal NiParticleSystem(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		IsWorldSpace = r.ReadSafeBoolean();

		Modifiers = new ChunkRef<NIFUnknownChunk>[r.ReadUInt32()];
		r.ReadArray(Modifiers);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiParticleSystem));
	}
}