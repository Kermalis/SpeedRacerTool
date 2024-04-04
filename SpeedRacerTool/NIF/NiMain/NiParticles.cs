using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiParticles : NiGeometry
{
	protected NiParticles(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}