using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class ATextureRenderData : NiObject
{
	public readonly PixelFormat PixFormat;
	public readonly byte BitsPerPixel;
	public readonly int UnkInt1; // Ref?
	public readonly int UnkInt2;
	public readonly byte Flags;
	public readonly uint UnkUint1;
	public readonly byte UnkByte1;
	public readonly ChannelData[] Channels;
	public readonly ChunkRef<NIFUnknownChunk> Palette; // TODO: ChunkRef<NiPalette>
	public readonly uint BytesPerPixel;
	public readonly MipMap[] Mips;

	protected ATextureRenderData(EndianBinaryReader r, int offset)
		: base(offset)
	{
		PixFormat = r.ReadEnum<PixelFormat>();
		BitsPerPixel = r.ReadByte();
		UnkInt1 = r.ReadInt32();

		SRAssert.Equal(UnkInt1, -1);

		UnkInt2 = r.ReadInt32();

		SRAssert.Equal(UnkInt2, 0);

		Flags = r.ReadByte();
		UnkUint1 = r.ReadUInt32();
		UnkByte1 = r.ReadByte();

		Channels = new ChannelData[4];
		for (int i = 0; i < Channels.Length; i++)
		{
			Channels[i] = new ChannelData(r);
		}

		Palette = new ChunkRef<NIFUnknownChunk>(r);
		uint numMips = r.ReadUInt32();
		BytesPerPixel = r.ReadUInt32();

		Mips = new MipMap[numMips];
		for (int i = 0; i < Mips.Length; i++)
		{
			Mips[i] = new MipMap(r);
		}
	}
}