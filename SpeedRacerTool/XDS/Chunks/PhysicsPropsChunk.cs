using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class PhysicsPropsChunk : XDSChunk
{
	public MagicValue Magic_Name;
	public Magic_OneAyyArray Magic_Entries;

	// Node data
	public OneBeeString Name;
	public OneAyyArray<Entry> Entries;

	internal PhysicsPropsChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0124);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_Name = new MagicValue(r);
		Magic_OneAyyArray.ReadEmpty(r);
		Magic_Entries = new Magic_OneAyyArray(r, xds);
		Magic_OneAyyArray.ReadEmpty(r);

		for (int i = 0; i < 6; i++)
		{
			SRAssert.Equal(r.ReadUInt32(), 0x00000000);
		}

		// NODE START
		XDSFile.ReadNodeStart(r);

		Name = new OneBeeString(r);

		OneAyyArray<object>.ReadEmpty(r);

		Entries = new OneAyyArray<Entry>(r);
		Entries.AssertMatch(Magic_Entries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		OneAyyArray<object>.ReadEmpty(r);

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.NewNode();

		sb.AppendLine(nameof(Name), Name);
		sb.EmptyArray();

		sb.NewArray(nameof(Entries), Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EmptyArray();

		sb.EndNode();
	}
}