using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;

namespace Kermalis.SpeedRacerTool.XDS;

internal abstract class XDSChunk
{
	public int Offset;

	protected XDSChunk(int offset)
	{
		Offset = offset;
	}

	internal static XDSChunk ReadNextChunk(EndianBinaryReader r)
	{
		return null!;
	}

	protected string DebugStr(string name, string contents)
	{
		return string.Format("{0}@0x{1:X} = ({2})", name, Offset, contents);
	}

	internal virtual string DebugStr(NIFFile nif)
	{
		return ToString()!;
	}
}