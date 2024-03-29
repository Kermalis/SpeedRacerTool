using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class ActionMapChunk : XDSChunk
{
	public sealed class Entry
	{
		public sealed class KeyBind
		{
			public MagicValue Magic_Type;
			public MagicValue Magic_KeyFilter;
			public MagicValue Magic_Key;

			// Node data
			public OneBeeString Type;
			public OneBeeString KeyFilter;
			public OneBeeString Key;

			internal KeyBind(EndianBinaryReader r)
			{
				Magic_Type = new MagicValue(r);
				Magic_KeyFilter = new MagicValue(r);
				Magic_Key = new MagicValue(r);

				// NODE START
				XDSFile.ReadNodeStart(r);

				Type = new OneBeeString(r);
				KeyFilter = new OneBeeString(r);
				Key = new OneBeeString(r);

				XDSFile.ReadNodeEnd(r);
				// NODE END
			}

			internal void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.AppendLine_ArrayElement(index);
				sb.NewObject();

				sb.NewNode();

				sb.AppendLine(Type);
				sb.AppendLine(KeyFilter);
				sb.AppendLine(Key);

				sb.EndNode();

				sb.EndObject();
			}
		}

		public MagicValue Magic_ActionName;
		public MagicValue Magic_KeyBinds;

		// Node data
		public OneBeeString ActionName;
		public OneAyyArray<KeyBind> KeyBinds;

		internal Entry(EndianBinaryReader r, XDSFile xds)
		{
			Magic_ActionName = new MagicValue(r);
			uint numBinds = xds.ReadFileUInt32(r);
			Magic_KeyBinds = new MagicValue(r);

			// NODE START
			XDSFile.ReadNodeStart(r);

			ActionName = new OneBeeString(r);

			KeyBinds = new OneAyyArray<KeyBind>(r);
			XDSFile.AssertValue((ulong)KeyBinds.Values.Length, numBinds);
			for (int i = 0; i < KeyBinds.Values.Length; i++)
			{
				KeyBinds.Values[i] = new KeyBind(r);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.NewNode();

			sb.AppendLine(ActionName);

			sb.NewArray(KeyBinds.Values.Length);
			for (int i = 0; i < KeyBinds.Values.Length; i++)
			{
				KeyBinds.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}

	public MagicValue Magic_ActionGroupName;
	public MagicValue Magic_Entries;

	// Node data
	public OneBeeString ActionGroupName;
	public OneAyyArray<Entry> Entries;

	internal ActionMapChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x010F);
		XDSFile.AssertValue(NumNodes, 0x0001);

		Magic_ActionGroupName = new MagicValue(r);
		uint numEntries = xds.ReadFileUInt32(r);
		Magic_Entries = new MagicValue(r);

		// NODE START
		XDSFile.ReadNodeStart(r);

		ActionGroupName = new OneBeeString(r);

		Entries = new OneAyyArray<Entry>(r);
		XDSFile.AssertValue((ulong)Entries.Values.Length, numEntries);
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

		sb.AppendLine(ActionGroupName);

		sb.NewArray(Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}