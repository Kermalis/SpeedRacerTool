using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Property of vertex colors.
/// This object is referred to by the root object of the NIF file whenever some <see cref="NiTriShapeData"/> object has vertex colors with non-default settings; if not present, vertex colors have vertex_mode=2 and lighting_mode=1.</summary>
internal sealed class NiVertexColorProperty : NiProperty
{
	/// <summary>Bits 0-2: Unknown.
	/// Bit 3: Lighting Mode?
	/// Bits 4-5: Vertex Mode?</summary>
	public readonly ushort Flags;

	public NiVertexColorProperty(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Flags = r.ReadUInt16();
	}
}