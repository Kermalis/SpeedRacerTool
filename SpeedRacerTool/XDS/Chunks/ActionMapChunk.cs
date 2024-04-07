using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class ActionMapChunk : XDSChunk
{
	public MagicValue Magic_ActionGroupName;
	public Magic_OneAyyArray Magic_Entries;

	// Node data
	public OneBeeString ActionGroupName;
	public OneAyyArray<Entry> Entries;

	internal ActionMapChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x010F);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_ActionGroupName = new MagicValue(r);
		Magic_Entries = new Magic_OneAyyArray(r, xds);

		// NODE START
		XDSFile.ReadNodeStart(r);

		ActionGroupName = new OneBeeString(r);

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
		sb.NewNode();

		sb.AppendLine(nameof(ActionGroupName), ActionGroupName);

		sb.NewArray(nameof(Entries), Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}