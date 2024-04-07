using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTexturingProperty : NiProperty
{
	public sealed class BumpMapData
	{
		public readonly TexDesc BumpTex;
		public readonly float LumaScale;
		public readonly float LumaOffset;
		public readonly Matrix2x2 Mat;

		public BumpMapData(EndianBinaryReader r)
		{
			BumpTex = new TexDesc(r);
			LumaScale = r.ReadSingle();
			LumaOffset = r.ReadSingle();
			Mat = new Matrix2x2(r);
		}

		public static void DebugStr(NIFFile nif, NIFStringBuilder sb, string name, BumpMapData? b)
		{
			if (b is null)
			{
				sb.AppendName(name);
				sb.AppendLine_Null();
			}
			else
			{
				sb.NewObject(name, nameof(TexDesc) + '.' + nameof(BumpMapData));

				b.DebugStr(nif, sb);

				sb.EndObject();
			}
		}
		private void DebugStr(NIFFile nif, NIFStringBuilder sb)
		{
			sb.AppendLine(nameof(LumaScale), LumaScale);
			sb.AppendLine(nameof(LumaOffset), LumaOffset);
			sb.AppendLine(nameof(Mat), Mat);

			TexDesc.DebugStr(nif, sb, nameof(BumpTex), BumpTex);
		}
	}
	public sealed class UnkData
	{
		public readonly TexDesc UnkTex;
		public readonly float UnkFloat1;

		public UnkData(EndianBinaryReader r)
		{
			UnkTex = new TexDesc(r);
			UnkFloat1 = r.ReadSingle();
		}

		public static void DebugStr(NIFFile nif, NIFStringBuilder sb, string name, UnkData? u)
		{
			if (u is null)
			{
				sb.AppendName(name);
				sb.AppendLine_Null();
			}
			else
			{
				sb.NewObject(name, nameof(TexDesc) + '.' + nameof(UnkData));

				u.DebugStr(nif, sb);

				sb.EndObject();
			}
		}
		private void DebugStr(NIFFile nif, NIFStringBuilder sb)
		{
			sb.AppendLine(nameof(UnkFloat1), UnkFloat1);

			TexDesc.DebugStr(nif, sb, nameof(UnkTex), UnkTex);
		}
	}

	public readonly ushort Flags;
	public readonly TexDesc? BaseTex;
	public readonly TexDesc? DarkTex;
	public readonly TexDesc? DetailTex;
	public readonly TexDesc? GlossTex;
	public readonly TexDesc? GlowTex;
	public readonly BumpMapData? BumpMapTex;
	public readonly TexDesc? NormalTex;
	public readonly UnkData? UnkTex;
	public readonly TexDesc? Decal0Tex;
	public readonly TexDesc? Decal1Tex;
	public readonly TexDesc? Decal2Tex;
	public readonly TexDesc? Decal3Tex;
	public readonly ShaderTexDesc?[] ShaderTextures;

	public NiTexturingProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadUInt16();

		uint numTex = r.ReadUInt32();

		if (r.ReadSafeBoolean())
		{
			BaseTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			DarkTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			DetailTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			GlossTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			GlowTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			BumpMapTex = new BumpMapData(r);
		}
		if (r.ReadSafeBoolean())
		{
			NormalTex = new TexDesc(r);
		}
		if (r.ReadSafeBoolean())
		{
			UnkTex = new UnkData(r);
		}
		if (r.ReadSafeBoolean())
		{
			Decal0Tex = new TexDesc(r);
		}
		if (numTex >= 10 && r.ReadSafeBoolean())
		{
			Decal1Tex = new TexDesc(r);
		}
		if (numTex >= 11 && r.ReadSafeBoolean())
		{
			Decal2Tex = new TexDesc(r);
		}
		if (numTex >= 12 && r.ReadSafeBoolean())
		{
			Decal3Tex = new TexDesc(r);
		}

		ShaderTextures = new ShaderTexDesc?[r.ReadUInt32()];
		for (int i = 0; i < ShaderTextures.Length; i++)
		{
			ShaderTextures[i] = ShaderTexDesc.Read(r);
		}
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		BaseTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		DarkTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		DetailTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		GlossTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		GlowTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		BumpMapTex?.BumpTex.Source.Resolve(nif).SetParentAndChildren(nif, this);
		NormalTex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		UnkTex?.UnkTex.Source.Resolve(nif).SetParentAndChildren(nif, this);
		Decal0Tex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		Decal1Tex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		Decal2Tex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		Decal3Tex?.Source.Resolve(nif).SetParentAndChildren(nif, this);

		foreach (ShaderTexDesc? d in ShaderTextures)
		{
			d?.Tex?.Source.Resolve(nif).SetParentAndChildren(nif, this);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags);

		TexDesc.DebugStr(nif, sb, nameof(BaseTex), BaseTex);
		TexDesc.DebugStr(nif, sb, nameof(DarkTex), DarkTex);
		TexDesc.DebugStr(nif, sb, nameof(DetailTex), DetailTex);
		TexDesc.DebugStr(nif, sb, nameof(GlossTex), GlossTex);
		TexDesc.DebugStr(nif, sb, nameof(GlowTex), GlowTex);
		BumpMapData.DebugStr(nif, sb, nameof(BumpMapTex), BumpMapTex);
		TexDesc.DebugStr(nif, sb, nameof(NormalTex), NormalTex);
		UnkData.DebugStr(nif, sb, nameof(UnkTex), UnkTex);
		TexDesc.DebugStr(nif, sb, nameof(Decal0Tex), Decal0Tex);
		TexDesc.DebugStr(nif, sb, nameof(Decal1Tex), Decal1Tex);
		TexDesc.DebugStr(nif, sb, nameof(Decal2Tex), Decal2Tex);
		TexDesc.DebugStr(nif, sb, nameof(Decal3Tex), Decal3Tex);

		sb.NewArray(nameof(ShaderTextures), ShaderTextures.Length);
		for (int i = 0; i < ShaderTextures.Length; i++)
		{
			ShaderTexDesc.DebugStr(nif, sb, i, ShaderTextures[i]);
		}
		sb.EndArray();
	}
}