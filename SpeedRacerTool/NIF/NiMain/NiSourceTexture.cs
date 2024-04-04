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

	public NiSourceTexture(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		IsExternal = r.ReadBoolean();
		FileName = new StringIndex(r);
		PixelData = new ChunkRef<ATextureRenderData>(r);
		PixelLayout = r.ReadEnum<PixelLayout>();
		MipMap = r.ReadEnum<MipMapFormat>();
		Alpha = r.ReadEnum<AlphaFormat>();
		IsStatic = r.ReadBoolean();
		IsDirectRender = r.ReadBoolean();
		ShouldPersistData = r.ReadBoolean();
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		PixelData.Resolve(nif).SetParentAndChildren(nif, this);
	}
}