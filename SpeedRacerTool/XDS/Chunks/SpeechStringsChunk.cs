using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class SpeechStringsChunk : XDSChunk
{
	public MagicValue[] Magics;
	public Entry[] Entries;

	internal SpeechStringsChunk(EndianBinaryReader r, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0106);
		SRAssert.NotEqual(NumNodes, 0x0000);

		Magics = new MagicValue[NumNodes * 2];
		MagicValue.ReadArray(r, Magics);

		Entries = new Entry[NumNodes];
		for (int i = 0; i < Entries.Length; i++)
		{
			Entries[i] = new Entry(r);
		}
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		foreach (Entry e in Entries)
		{
			e.DebugStr(sb);
		}
	}
}