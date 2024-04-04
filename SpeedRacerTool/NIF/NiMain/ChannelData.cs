using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class ChannelData
{
	public readonly ChannelType Type;
	public readonly ChannelConvention Convention;
	public readonly byte BitsPerChannel;
	public readonly byte UnkByte1;

	public ChannelData(EndianBinaryReader r)
	{
		Type = r.ReadEnum<ChannelType>();
		Convention = r.ReadEnum<ChannelConvention>();
		BitsPerChannel = r.ReadByte();
		UnkByte1 = r.ReadByte();
	}
}