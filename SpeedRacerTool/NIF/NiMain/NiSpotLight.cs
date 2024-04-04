using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiSpotLight : NiPointLight
{
	public readonly float CutoffAngle;
	public readonly float UnkFloat1;
	public readonly float Exponent;

	public NiSpotLight(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		CutoffAngle = r.ReadSingle();
		UnkFloat1 = r.ReadSingle();
		Exponent = r.ReadSingle();
	}
}