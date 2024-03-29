using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class InterestingEventGeneratorsChunk : XDSChunk
{
	public sealed class Entry
	{
		public string EventID;
		public MagicValue Magic_EventParams;

		// Node data
		public OneBeeString EventParams;

		internal Entry(EndianBinaryReader r)
		{
			EventID = r.ReadString_Count_TrimNullTerminators(0x40);
			Magic_EventParams = new MagicValue(r);

			// NODE START
			XDSFile.ReadNodeStart(r);

			EventParams = new OneBeeString(r);

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(EventID);

			sb.NewNode();

			sb.AppendLine(EventParams);

			sb.EndNode();

			sb.EndObject();
		}
	}

	public Magic_OneAyyArray Magic_Entries;

	// Node data
	public OneAyyArray<Entry> Entries;

	internal InterestingEventGeneratorsChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x0108);
		XDSFile.AssertValue(NumNodes, 0x0001);

		Magic_Entries = new Magic_OneAyyArray(r, xds);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<Entry>(r);
		Entries.AssertMatch(Magic_Entries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r);
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
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}