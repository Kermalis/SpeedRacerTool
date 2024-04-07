using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class PS2TrackChunk : XDSChunk
{
	public string UnkXML;
	public Magic_OneAyyArray Magic_Pieces;
	public Magic_OneAyyArray Magic_Joins;
	public Magic_OneAyyArray Magic_Revisions;
	public Magic_OneAyyArray Magic_Additions;

	// Node data
	public OneAyyArray<Piece> Pieces;
	public OneAyyArray<Join> Joins;
	public OneAyyArray<Revision> Revisions;
	public OneAyyArray<Addition> Additions;

	internal PS2TrackChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0131);
		SRAssert.Equal(NumNodes, 0x0001);

		UnkXML = r.ReadString_Count_TrimNullTerminators(0x48);
		Magic_Pieces = new Magic_OneAyyArray(r, xds);
		Magic_Joins = new Magic_OneAyyArray(r, xds);
		Magic_Revisions = new Magic_OneAyyArray(r, xds);
		Magic_Additions = new Magic_OneAyyArray(r, xds);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Pieces = new OneAyyArray<Piece>(r);
		Pieces.AssertMatch(Magic_Pieces);
		for (int i = 0; i < Pieces.Values.Length; i++)
		{
			Pieces.Values[i] = new Piece(r, xds);
		}

		Joins = new OneAyyArray<Join>(r);
		Joins.AssertMatch(Magic_Joins);
		for (int i = 0; i < Joins.Values.Length; i++)
		{
			Joins.Values[i] = new Join(r);
		}

		Revisions = new OneAyyArray<Revision>(r);
		Revisions.AssertMatch(Magic_Revisions);
		for (int i = 0; i < Revisions.Values.Length; i++)
		{
			Revisions.Values[i] = new Revision(r, xds);
		}

		Additions = new OneAyyArray<Addition>(r);
		Additions.AssertMatch(Magic_Additions);
		for (int i = 0; i < Additions.Values.Length; i++)
		{
			Additions.Values[i] = new Addition(r, xds);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine(nameof(UnkXML), UnkXML);

		sb.NewNode();

		sb.NewArray(nameof(Pieces), Pieces.Values.Length);
		for (int i = 0; i < Pieces.Values.Length; i++)
		{
			Pieces.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(nameof(Joins), Joins.Values.Length);
		for (int i = 0; i < Joins.Values.Length; i++)
		{
			Joins.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(nameof(Revisions), Revisions.Values.Length);
		for (int i = 0; i < Revisions.Values.Length; i++)
		{
			Revisions.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(nameof(Additions), Additions.Values.Length);
		for (int i = 0; i < Additions.Values.Length; i++)
		{
			Additions.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}