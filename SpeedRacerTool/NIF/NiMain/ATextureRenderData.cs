using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class ATextureRenderData : NiObject
{
	public readonly PixelFormat PixFormat;
	public readonly byte BitsPerPixel;
	public readonly byte Flags;
	public readonly ChannelData[] Channels;
	public readonly NullableChunkRef<NIFUnknownChunk> Palette; // TODO: NullableChunkRef<NiPalette>
	/// <summary>Can be 0. For example, 4 bits per pixel</summary>
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

		SRAssert.Equal(r.ReadUInt32(), 0);
		SRAssert.Equal(r.ReadByte(), 0);

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
		sb.AppendLine(nameof(PixFormat), PixFormat.ToString());
		sb.AppendLine(nameof(BitsPerPixel), BitsPerPixel, hex: false);
		sb.AppendLine(nameof(Flags), Flags);

		sb.NewArray(nameof(Channels), Channels.Length);
		for (int i = 0; i < Channels.Length; i++)
		{
			Channels[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.WriteChunk(nameof(Palette), nif, Palette.Resolve(nif));
		sb.AppendLine(nameof(BytesPerPixel), BytesPerPixel, hex: false);

		sb.NewArray(nameof(Mips), Mips.Length);
		for (int i = 0; i < Mips.Length; i++)
		{
			Mips[i].DebugStr(sb, i);
		}
		sb.EndArray();
	}
}