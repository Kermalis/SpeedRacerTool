using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

internal abstract class NiExtraData : Chunk
{
	public readonly StringIndex Name;

	protected NiExtraData(EndianBinaryReader r, int offset)
		: base(offset)
	{
		Name = new StringIndex(r);
	}
}