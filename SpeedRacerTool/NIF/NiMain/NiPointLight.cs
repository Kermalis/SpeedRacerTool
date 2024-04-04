using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Not sealed
internal class NiPointLight : NiLight
{
	public readonly float AttenConstant;
	public readonly float AttenLinear;
	public readonly float AttenQuadratic;

	internal NiPointLight(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		AttenConstant = r.ReadSingle();
		AttenLinear = r.ReadSingle();
		AttenQuadratic = r.ReadSingle();
	}
}