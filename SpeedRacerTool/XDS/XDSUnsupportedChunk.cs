using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSUnsupportedChunk : XDSChunk
{
	public readonly byte[] Data;

	internal XDSUnsupportedChunk(EndianBinaryReader r, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		Data = new byte[r.Stream.Length - r.Stream.Position - 2];
		r.ReadBytes(Data);
	}
}