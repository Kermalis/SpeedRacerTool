using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTextureEffect : NiDynamicEffect
{
	public readonly Matrix3x3 ProjectionMat;
	public readonly Vector3 ProjectionTrans;
	public readonly TexFilterMode TexFilter;
	public readonly TexClampMode TexClamp;
	public readonly EffectType TexType;
	public readonly CoordGenType CoordGen;
	public readonly ChunkRef<NiSourceTexture> SourceTex;
	public readonly byte Clipping;
	public readonly Vector3 Unk100;
	public readonly float Unk0;

	internal NiTextureEffect(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		ProjectionMat = new Matrix3x3(r);
		ProjectionTrans = r.ReadVector3();
		TexFilter = r.ReadEnum<TexFilterMode>();
		TexClamp = r.ReadEnum<TexClampMode>();
		TexType = r.ReadEnum<EffectType>();
		CoordGen = r.ReadEnum<CoordGenType>();
		SourceTex = new ChunkRef<NiSourceTexture>(r);
		Clipping = r.ReadByte();
		Unk100 = r.ReadVector3();
		Unk0 = r.ReadSingle();
	}
}