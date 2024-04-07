using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal sealed class ChannelData
{
    public readonly ChannelType Type;
    public readonly ChannelConvention Convention;
    public readonly byte BitsPerChannel;

    public ChannelData(EndianBinaryReader r)
    {
        Type = r.ReadEnum<ChannelType>();
        Convention = r.ReadEnum<ChannelConvention>();
        BitsPerChannel = r.ReadByte();

        SRAssert.Equal(r.ReadByte(), 0);
    }

    public void DebugStr(NIFStringBuilder sb, int index)
    {
        sb.AppendLine_ArrayElement(index);
        sb.NewObject();

        sb.AppendLine(nameof(Type), Type.ToString());
        sb.AppendLine(nameof(Convention), Convention.ToString());
        sb.AppendLine(nameof(BitsPerChannel), BitsPerChannel, hex: false);

        sb.EndObject();
    }
}