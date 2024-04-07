using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class TrackRegistryChunk
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

			sb.AppendLine(nameof(TextID), TextID);
			sb.AppendLine(nameof(Text), Text);

			sb.EndNode();
		}

		public override string ToString()
		{
			return $"[{TextID}] = {Text}";
		}
	}
}