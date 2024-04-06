using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPixelData : ATextureRenderData
{
	public readonly uint NumBytesPerFace;
	public readonly uint NumFaces;
	/// <summary>Raw pixel data holding the mipmaps.
	/// Mipmap zero is the full-size texture and they get smaller by half as the number increases.</summary>
	public readonly byte[][] PixelData;

	public NiPixelData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		NumBytesPerFace = r.ReadUInt32();
		NumFaces = r.ReadUInt32();

		PixelData = new byte[NumFaces][];
		for (int i = 0; i < PixelData.Length; i++)
		{
			PixelData[i] = new byte[NumBytesPerFace];
			r.ReadBytes(PixelData[i]);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(NumBytesPerFace), NumBytesPerFace, hex: false);
		sb.AppendLine(nameof(NumFaces), NumFaces, hex: false);

		sb.AppendLine(nameof(PixelData), string.Format("byte[{0}][{1}]", NumFaces, NumBytesPerFace));
	}
}