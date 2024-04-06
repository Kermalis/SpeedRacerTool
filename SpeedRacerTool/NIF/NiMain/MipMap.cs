using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class MipMap
{
	public readonly uint Width;
	public readonly uint Height;
	/// <summary>Offset into the pixel data array where this mipmap starts</summary>
	public readonly uint Offset;

	public MipMap(EndianBinaryReader r)
	{
		Width = r.ReadUInt32();
		Height = r.ReadUInt32();
		Offset = r.ReadUInt32();
	}

	public void DebugStr(NIFStringBuilder sb, int index)
	{
		sb.AppendLine_ArrayElement(index);
		sb.NewObject();

		sb.AppendLine(nameof(Width), Width, hex: false);
		sb.AppendLine(nameof(Height), Height, hex: false);
		sb.AppendLine(nameof(Offset), Offset);

		sb.EndObject();
	}
}