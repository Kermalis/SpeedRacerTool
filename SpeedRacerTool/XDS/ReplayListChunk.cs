using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class ReplayListChunk : XDSChunk
{
	public Magic_OneAyyArray Magic_Entries;

	// Node data
	public OneAyyArray<string> Entries;

	internal ReplayListChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0106);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_Entries = new Magic_OneAyyArray(r, xds); // 5 for both... skorost is missing in PS2 version

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<string>(r);
		Entries.AssertMatch(Magic_Entries);
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