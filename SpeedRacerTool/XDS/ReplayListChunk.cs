using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class ReplayListChunk : XDSChunk
{
	public MagicValue Magic_Entries;

	// Node data
	public OneAyyArray<string> Entries;

	internal ReplayListChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x0106);
		XDSFile.AssertValue(NumNodes, 0x0001);

		uint numTracks = xds.ReadFileUInt32(r); // 5 for both... skorost is missing in PS2 version
		Magic_Entries = new MagicValue(r);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<string>(r);
		XDSFile.AssertValue((ulong)Entries.Values.Length, numTracks);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = r.ReadString_Count_TrimNullTerminators(0x22);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.NewNode();

		sb.NewArray(Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			sb.Append_ArrayElement(i);
			sb.AppendLine_Quotes(Entries.Values[i], indent: false);
		}
		sb.EndArray();

		sb.EndNode();
	}
}