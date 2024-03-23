using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

internal abstract class NiProperty : NiObjectNET
{
    protected NiProperty(EndianBinaryReader r, int offset)
        : base(r, offset)
    {
        //
    }
}