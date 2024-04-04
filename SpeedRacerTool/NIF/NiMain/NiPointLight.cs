using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Not sealed
internal class NiPointLight : NiLight
{
	public readonly float AttenConstant;
	public readonly float AttenLinear;
	public readonly float AttenQuadratic;

	internal NiPointLight(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		AttenConstant = r.ReadSingle();
		AttenLinear = r.ReadSingle();
		AttenQuadratic = r.ReadSingle();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiPointLight));
	}
}