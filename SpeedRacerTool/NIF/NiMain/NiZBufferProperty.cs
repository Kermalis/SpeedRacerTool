using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>This Property controls the Z buffer (OpenGL: depth buffer).</summary>
internal sealed class NiZBufferProperty : NiProperty
{
    public const string NAME = "NiZBufferProperty";

    /// <summary>Bit 0 enables the z test.
    /// Bit 1 controls whether the Z buffer is read only(0) or read/write(1)</summary>
    public readonly ushort Flags; // tFlags

    internal NiZBufferProperty(EndianBinaryReader r, int offset)
        : base(r, offset)
    {
        Flags = r.ReadUInt16();
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Format("Name=\"{0}\" | Flags=0x{1:X}",
            Name.Resolve(nif),
            Flags));
    }
}