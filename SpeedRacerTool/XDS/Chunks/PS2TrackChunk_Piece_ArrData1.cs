using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	partial class Piece
	{
		public sealed partial class ArrData1
		{
			public Magic_OneAyyArray Magic1;

			// Node data
			public OneAyyArray<ArrArrData1> Array1;

			internal ArrData1(EndianBinaryReader r, XDSFile xds)
			{
				Magic1 = new Magic_OneAyyArray(r, xds);

				// NODE START
				XDSFile.ReadNodeStart(r);

				Array1 = new OneAyyArray<ArrArrData1>(r);
				Array1.AssertMatch(Magic1);
				for (int i = 0; i < Array1.Values.Length; i++)
				{
					Array1.Values[i] = new ArrArrData1(r, xds);
				}

				XDSFile.ReadNodeEnd(r);
				// NODE END
			}

			internal void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.NewObject(index);

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
}