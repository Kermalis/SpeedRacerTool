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
}