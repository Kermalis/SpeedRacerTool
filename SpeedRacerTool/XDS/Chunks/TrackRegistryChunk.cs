using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed class TrackRegistryChunk : XDSChunk
{
	public sealed class Entry
	{
		public OneBeeString TextID;
		public OneBeeString Text;

		internal Entry(EndianBinaryReader r)
		{
			// NODE START
			XDSFile.ReadNodeStart(r);

			TextID = new OneBeeString(r);
			Text = new OneBeeString(r);

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb)
		{
			sb.NewNode();

			sb.AppendLine_NoQuotes(ToString());

			sb.EndNode();
		}

		public override string ToString()
		{
			return $"[{TextID}] = {Text}";
		}
	}

	public MagicValue[] Magics;
	public Entry[] Entries;

	internal TrackRegistryChunk(EndianBinaryReader r, int offset, ushort opcode, ushort numNodes)
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