using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>A shape node that refers to data organized into strips of triangles.</summary>
internal sealed class NiTriStrips : NiTriBasedGeom
{
    public const string NAME = "NiTriStrips";

    internal NiTriStrips(EndianBinaryReader r, int offset)
        : base(r, offset)
    {
        //
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Format("Name=\"{0}\"", Name.Resolve(nif)));
    }
}