using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiTimeController : NiObject
{
	public readonly NullableChunkRef<NiTimeController> Next;
	/// <summary>Controller flags (usually 0x000C). Probably controls loops.
	/// Bit 0 : Anim type, 0=APP_TIME 1=APP_INIT
	/// Bit 1-2 : Cycle type  00=Loop 01=Reverse 10=Loop
	/// Bit 3 : Active
	/// Bit 4 : Play backwards</summary>
	public readonly ushort Flags;
	public readonly float Frequency;
	public readonly float Phase;
	public readonly float StartTime;
	public readonly float StopTime;
	public readonly ChunkPtr<NiObjectNET> Target;

	protected NiTimeController(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Next = new NullableChunkRef<NiTimeController>(r);
		Flags = r.ReadUInt16();
		Frequency = r.ReadSingle();
		Phase = r.ReadSingle();
		StartTime = r.ReadSingle();
		StopTime = r.ReadSingle();
		Target = new ChunkPtr<NiObjectNET>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Next.Resolve(nif)?.SetParentAndChildren(nif, parent); // They share the parent
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Flags), Flags);
		sb.AppendLine(nameof(Frequency), Frequency);
		sb.AppendLine(nameof(Phase), Phase);
		sb.AppendLine(nameof(StartTime), StartTime);
		sb.AppendLine(nameof(StopTime), StopTime);

		sb.WriteChunkPtr(nameof(Target), Target.Resolve(nif));

		sb.WriteChunk(nameof(Next), nif, Next.Resolve(nif));
	}
}