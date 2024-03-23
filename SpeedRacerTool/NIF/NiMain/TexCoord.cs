using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Texture coordinates (u,v). As in OpenGL; image origin is in the lower left corner.</summary>
internal readonly struct TexCoord
{
    public readonly float U;
    public readonly float V;

    internal TexCoord(EndianBinaryReader r)
    {
        U = r.ReadSingle();
        V = r.ReadSingle();
    }
}