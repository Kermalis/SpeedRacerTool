using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiMaterialProperty : NiProperty
{
	public readonly Vector3 AmbientColor;
	public readonly Vector3 DiffuseColor;
	public readonly Vector3 SpecularColor;
	public readonly Vector3 EmissiveColor;
	public readonly float Glossiness;
	/// <summary>The material transparency (1=non-transparant). When alpha is not 1, refer to a <see cref="NiAlphaProperty"/> in this material's parent <see cref="NiTriShape"/>.</summary>
	public readonly float Alpha;

	public NiMaterialProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		AmbientColor = r.ReadVector3();
		DiffuseColor = r.ReadVector3();
		SpecularColor = r.ReadVector3();
		EmissiveColor = r.ReadVector3();
		Glossiness = r.ReadSingle();
		Alpha = r.ReadSingle();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(AmbientColor), AmbientColor);
		sb.AppendLine(nameof(DiffuseColor), DiffuseColor);
		sb.AppendLine(nameof(SpecularColor), SpecularColor);
		sb.AppendLine(nameof(EmissiveColor), EmissiveColor);
		sb.AppendLine(nameof(Glossiness), Glossiness);
		sb.AppendLine(nameof(Alpha), Alpha);
	}
}