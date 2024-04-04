using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiLight : NiDynamicEffect
{
	public readonly float Power;
	public readonly Vector3 AmbientColor;
	public readonly Vector3 DiffuseColor;
	public readonly Vector3 SpecularColor;

	protected NiLight(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Power = r.ReadSingle();
		AmbientColor = r.ReadVector3();
		DiffuseColor = r.ReadVector3();
		SpecularColor = r.ReadVector3();
	}
}