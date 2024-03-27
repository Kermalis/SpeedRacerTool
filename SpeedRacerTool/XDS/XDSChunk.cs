namespace Kermalis.SpeedRacerTool.XDS;

internal abstract class XDSChunk
{
	internal virtual string DebugStr()
	{
		return ToString()!;
	}
}