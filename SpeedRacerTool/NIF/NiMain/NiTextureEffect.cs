using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;
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
	public readonly ChunkRef<NiSourceTexture> SourceTex; // TODO: Child?
	public readonly byte Clipping;
	public readonly Vector3 Unk100;
	public readonly float Unk0;

	internal NiTextureEffect(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
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

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(ProjectionTrans), ProjectionTrans);
		sb.AppendLine(nameof(TexFilter), TexFilter.ToString());
		sb.AppendLine(nameof(TexClamp), TexClamp.ToString());
		sb.AppendLine(nameof(TexType), TexType.ToString());
		sb.AppendLine(nameof(CoordGen), CoordGen.ToString());
		sb.AppendLine(nameof(Clipping), Clipping);
		sb.AppendLine(nameof(Unk100), Unk100);
		sb.AppendLine(nameof(Unk0), Unk0);
		sb.AppendLine(nameof(ProjectionMat), ProjectionMat);

		sb.WriteChunk(nameof(SourceTex), nif, SourceTex.Resolve(nif));
	}
}