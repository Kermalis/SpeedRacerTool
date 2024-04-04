using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPixelData : ATextureRenderData
{
	public readonly uint NumPixels;
	public readonly uint NumFaces;
	/// <summary>Raw pixel data holding the mipmaps.
	/// Mipmap zero is the full-size texture and they get smaller by half as the number increases.</summary>
	public readonly byte[][] PixelData;

	public NiPixelData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		NumPixels = r.ReadUInt32();
		NumFaces = r.ReadUInt32();

		PixelData = new byte[NumFaces][];
		for (int i = 0; i < PixelData.Length; i++)
		{
			PixelData[i] = new byte[NumPixels];
			r.ReadBytes(PixelData[i]);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiPixelData));
	}
}