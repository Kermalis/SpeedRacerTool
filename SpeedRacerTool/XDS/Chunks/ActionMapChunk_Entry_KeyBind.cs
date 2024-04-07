using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class ActionMapChunk
{
	partial class Entry
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
				sb.NewObject(index);

				sb.NewNode();

				sb.AppendLine(nameof(Type), Type);
				sb.AppendLine(nameof(KeyFilter), KeyFilter);
				sb.AppendLine(nameof(Key), Key);

				sb.EndNode();

				sb.EndObject();
			}
		}
	}
}