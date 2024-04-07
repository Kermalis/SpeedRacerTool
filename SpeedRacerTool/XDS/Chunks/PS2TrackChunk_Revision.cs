using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	public sealed partial class Revision
	{
		public string RevisionType;
		public Magic_OneAyyArray Magic1;

		// Node data
		public OneAyyArray<ArrData1> Array1;

		internal Revision(EndianBinaryReader r, XDSFile xds)
		{
			RevisionType = r.ReadString_Count_TrimNullTerminators(0x20);
			Magic1 = new Magic_OneAyyArray(r, xds);

			// NODE START
			XDSFile.ReadNodeStart(r);

			// In editor_template.xds, 5 elements from 0x1E1E-0x1ECC. 0xAF / 5 = 0x23
			Array1 = new OneAyyArray<ArrData1>(r);
			Array1.AssertMatch(Magic1);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i] = new ArrData1(r);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(RevisionType), RevisionType);

			sb.NewNode();

			sb.NewArray(nameof(Array1), Array1.Values.Length);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
}