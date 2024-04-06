using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class ATextureRenderData : NiObject
{
	public readonly PixelFormat PixFormat;
	public readonly byte BitsPerPixel;
	public readonly byte Flags;
	public readonly uint UnkUint1;
	public readonly byte UnkByte1;
	public readonly ChannelData[] Channels;
	public readonly NullableChunkRef<NIFUnknownChunk> Palette; // TODO: NullableChunkRef<NiPalette>
	public readonly uint BytesPerPixel;
	public readonly MipMap[] Mips;

	protected ATextureRenderData(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		PixFormat = r.ReadEnum<PixelFormat>();
		BitsPerPixel = r.ReadByte();

		SRAssert.Equal(r.ReadInt32(), -1); // Ref?
		SRAssert.Equal(r.ReadInt32(), 0);

		Flags = r.ReadByte();
		UnkUint1 = r.ReadUInt32();
		UnkByte1 = r.ReadByte();

		Channels = new ChannelData[4];
		for (int i = 0; i < Channels.Length; i++)
		{
			Channels[i] = new ChannelData(r);
		}

		Palette = new NullableChunkRef<NIFUnknownChunk>(r);
		uint numMips = r.ReadUInt32();
		BytesPerPixel = r.ReadUInt32();

		Mips = new MipMap[numMips];
		for (int i = 0; i < Mips.Length; i++)
		{
			Mips[i] = new MipMap(r);
		}
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Palette.Resolve(nif)?.SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(ATextureRenderData));
	}
}