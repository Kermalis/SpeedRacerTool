using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiSourceTexture : NiTexture
{
	public readonly bool IsExternal;
	public readonly StringIndex FileName;
	public readonly ChunkRef<ATextureRenderData> PixelData;
	public readonly PixelLayout PixelLayout;
	public readonly MipMapFormat MipMap;
	public readonly AlphaFormat Alpha;
	public readonly bool IsStatic;
	public readonly bool IsDirectRender;
	public readonly bool ShouldPersistData;

	public NiSourceTexture(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		IsExternal = r.ReadSafeBoolean();
		FileName = new StringIndex(r);
		PixelData = new ChunkRef<ATextureRenderData>(r);
		PixelLayout = r.ReadEnum<PixelLayout>();
		MipMap = r.ReadEnum<MipMapFormat>();
		Alpha = r.ReadEnum<AlphaFormat>();
		IsStatic = r.ReadSafeBoolean();
		IsDirectRender = r.ReadSafeBoolean();
		ShouldPersistData = r.ReadSafeBoolean();
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		PixelData.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine_Boolean(nameof(IsExternal), IsExternal);
		sb.AppendLine(nameof(FileName), FileName.Resolve(nif));
		sb.AppendLine(nameof(PixelLayout), PixelLayout.ToString());
		sb.AppendLine(nameof(MipMap), MipMap.ToString());
		sb.AppendLine(nameof(Alpha), Alpha.ToString());
		sb.AppendLine_Boolean(nameof(IsStatic), IsStatic);
		sb.AppendLine_Boolean(nameof(IsDirectRender), IsDirectRender);
		sb.AppendLine_Boolean(nameof(ShouldPersistData), ShouldPersistData);

		sb.WriteChunk(nameof(PixelData), nif, PixelData.Resolve(nif));
	}
}