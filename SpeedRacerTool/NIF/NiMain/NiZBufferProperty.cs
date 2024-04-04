using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>This Property controls the Z buffer (OpenGL: depth buffer).</summary>
internal sealed class NiZBufferProperty : NiProperty
{
	public readonly ZBufferFlags Flags;

	internal NiZBufferProperty(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Flags = r.ReadEnum<ZBufferFlags>();
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(NiZBufferProperty), string.Format("Name=\"{0}\" | Flags=({1})",
			Name.Resolve(nif),
			Flags));
	}
}