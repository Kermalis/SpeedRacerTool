using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class WIITrackChunk : XDSChunk
{
	public string UnkXML;

	internal WIITrackChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x012F);
		XDSFile.AssertValue(NumNodes, 0x0001);

		UnkXML = XDSFile.DEBUG_READ_SAFE_STR(r, 0x48);

		uint unk = xds.ReadFileUInt32(r);
	}
}