using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiShadeProperty : NiProperty
{
	public readonly ShadeFlags Flags;

	public NiShadeProperty(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Flags = r.ReadEnum<ShadeFlags>();
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(NiShadeProperty), string.Format("Name=\"{0}\" | Flags=({1})",
			Name.Resolve(nif),
			Flags));
	}
}