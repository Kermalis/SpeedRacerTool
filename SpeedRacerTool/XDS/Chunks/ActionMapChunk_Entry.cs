using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class ActionMapChunk
{
	public sealed partial class Entry
	{
		public MagicValue Magic_ActionName;
		public Magic_OneAyyArray Magic_KeyBinds;

		// Node data
		public OneBeeString ActionName;
		public OneAyyArray<KeyBind> KeyBinds;

		internal Entry(EndianBinaryReader r, XDSFile xds)
		{
			Magic_ActionName = new MagicValue(r);
			Magic_KeyBinds = new Magic_OneAyyArray(r, xds);

			// NODE START
			XDSFile.ReadNodeStart(r);

			ActionName = new OneBeeString(r);

			KeyBinds = new OneAyyArray<KeyBind>(r);
			KeyBinds.AssertMatch(Magic_KeyBinds);
			for (int i = 0; i < KeyBinds.Values.Length; i++)
			{
				KeyBinds.Values[i] = new KeyBind(r);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.NewNode();

			sb.AppendLine(nameof(ActionName), ActionName);

			sb.NewArray(nameof(KeyBinds), KeyBinds.Values.Length);
			for (int i = 0; i < KeyBinds.Values.Length; i++)
			{
				KeyBinds.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
}