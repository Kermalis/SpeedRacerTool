using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>This Property controls the Z buffer (OpenGL: depth buffer).</summary>
internal sealed class NiZBufferProperty : NiProperty
{
	public readonly ZBufferFlags Flags;

	internal NiZBufferProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadEnum<ZBufferFlags>();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags.ToString());
	}
}