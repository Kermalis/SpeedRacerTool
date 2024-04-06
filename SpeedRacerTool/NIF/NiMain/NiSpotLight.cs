using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiSpotLight : NiPointLight
{
	public readonly float CutoffAngle;
	public readonly float UnkFloat1;
	public readonly float Exponent;

	public NiSpotLight(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		CutoffAngle = r.ReadSingle();
		UnkFloat1 = r.ReadSingle();
		Exponent = r.ReadSingle();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(CutoffAngle), CutoffAngle);
		sb.AppendLine(nameof(UnkFloat1), UnkFloat1);
		sb.AppendLine(nameof(Exponent), Exponent);
	}
}