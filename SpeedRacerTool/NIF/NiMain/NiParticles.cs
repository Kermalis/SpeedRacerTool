using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiParticles : NiGeometry
{
	protected NiParticles(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		//
	}
}