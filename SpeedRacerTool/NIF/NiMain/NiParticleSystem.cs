using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Not sealed
internal class NiParticleSystem : NiParticles
{
	public readonly bool IsWorldSpace;
	public readonly ChunkRef<NIFUnknownChunk>[] Modifiers; // TODO: Ref<NiPSysModifier>

	internal NiParticleSystem(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		IsWorldSpace = r.ReadBoolean();

		Modifiers = new ChunkRef<NIFUnknownChunk>[r.ReadUInt32()];
		ChunkRef<NIFUnknownChunk>.ReadArray(r, Modifiers);
	}
}