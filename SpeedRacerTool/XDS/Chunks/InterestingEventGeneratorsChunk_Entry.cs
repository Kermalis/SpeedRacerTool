using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class InterestingEventGeneratorsChunk
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
			sb.NewObject(index);

			sb.AppendLine(nameof(EventID), EventID);

			sb.NewNode();

			sb.AppendLine(nameof(EventParams), EventParams);

			sb.EndNode();

			sb.EndObject();
		}
	}
}