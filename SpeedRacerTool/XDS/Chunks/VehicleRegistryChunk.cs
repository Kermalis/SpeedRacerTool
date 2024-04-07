using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class VehicleRegistryChunk : XDSChunk
{
	public Magic_OneAyyArray Magic_Entries;
	public string Timestamp;

	// Node data
	public OneAyyArray<Entry> Entries;

	internal VehicleRegistryChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0106);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_Entries = new Magic_OneAyyArray(r, xds);

		Timestamp = r.ReadString_Count_TrimNullTerminators(0x20);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<Entry>(r);
		Entries.AssertMatch(Magic_Entries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine(nameof(Timestamp), Timestamp);

		sb.NewNode();

		sb.NewArray(nameof(Entries), Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}