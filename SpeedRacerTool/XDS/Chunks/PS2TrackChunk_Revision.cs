using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	public sealed partial class Revision
	{
		/// <summary>"short", "medium", or "long"</summary>
		public string Type;
		public Magic_OneAyyArray Magic_Changes;

		// Node data
		public OneAyyArray<Change> Changes;

		internal Revision(EndianBinaryReader r, XDSFile xds)
		{
			Type = r.ReadString_Count_TrimNullTerminators(0x20);
			Magic_Changes = new Magic_OneAyyArray(r, xds);

			// NODE START
			XDSFile.ReadNodeStart(r);

			Changes = new OneAyyArray<Change>(r);
			Changes.AssertMatch(Magic_Changes);
			for (int i = 0; i < Changes.Values.Length; i++)
			{
				Changes.Values[i] = new Change(r);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(Type), Type);

			sb.NewNode();

			sb.NewArray(nameof(Changes), Changes.Values.Length);
			for (int i = 0; i < Changes.Values.Length; i++)
			{
				Changes.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
}