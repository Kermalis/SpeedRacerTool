using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal struct OneBeeString
{
	public string Str;

	internal OneBeeString(EndianBinaryReader r)
	{
		XDSFile.AssertValue(r.ReadUInt16(), 0x001B);
		XDSFile.AssertValue(r.ReadUInt16(), 0x0002);

		Str = r.ReadString_Count(r.ReadUInt16());
	}

	public override readonly string ToString()
	{
		return $"\"{Str}\"";
	}
}