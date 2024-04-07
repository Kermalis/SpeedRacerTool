using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	// 246 times total.
	public sealed partial class Piece
	{
		public string PieceName;
		/// <summary>"none" (204 times), "join" (21 times), or "split" (21 times)</summary>
		public string JunctionType;
		public Magic_OneAyyArray Magic1;
		/// <summary>0x100 (45 times), 0x001 (7 times), 0x000 (194 times)</summary>
		public ushort UnkUshort1;
		/// <summary>0x101 (3 times), 0x100 (240 times), 0x001 (3 times)</summary>
		public ushort UnkUshort2;
		public Magic_OneAyyArray Magic2;
		public Magic_OneAyyArray Magic3;

		// Node data
		/// <summary>Length: 1 ("none"), 2 ("join" or "split")</summary>
		public OneAyyArray<ArrData1> Array1;
		/// <summary>Length: 9 (7 times), 8 (7 times), 7 (11 times), 6 (19 times), 5 (28 times), 4 (20 times), 3 (58 times), 2 (82 times), 1 (14 times)</summary>
		public OneAyyArray<ArrData2> Array2;
		/// <summary>Length: 20 ("none"), 40 ("join" or "split")</summary>
		public OneAyyArray<ArrData3> Array3;

		internal Piece(EndianBinaryReader r, XDSFile xds)
		{
			PieceName = r.ReadString_Count_TrimNullTerminators(0x20);
			JunctionType = r.ReadString_Count_TrimNullTerminators(0x10);

			Magic1 = new Magic_OneAyyArray(r, xds);

			SRAssert.Equal(r.ReadUInt32(), 0);
			SRAssert.Equal(r.ReadUInt32(), 0);
			SRAssert.Equal(r.ReadUInt32(), 0);
			SRAssert.Equal(r.ReadUInt32(), 0);
			SRAssert.Equal(r.ReadUInt32(), 0x80000000);
			SRAssert.Equal(r.ReadUInt32(), 0);

			UnkUshort1 = r.ReadUInt16();
			UnkUshort2 = r.ReadUInt16();
			Magic2 = new Magic_OneAyyArray(r, xds);
			Magic3 = new Magic_OneAyyArray(r, xds);

			// NODE START
			XDSFile.ReadNodeStart(r);

			Array1 = new OneAyyArray<ArrData1>(r);
			Array1.AssertMatch(Magic1);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i] = new ArrData1(r, xds);
			}

			Array2 = new OneAyyArray<ArrData2>(r);
			Array2.AssertMatch(Magic2);
			for (int i = 0; i < Array2.Values.Length; i++)
			{
				Array2.Values[i] = new ArrData2(r, xds);
			}

			Array3 = new OneAyyArray<ArrData3>(r);
			Array3.AssertMatch(Magic3);

			switch (JunctionType)
			{
				case "none":
				{
					SRAssert.Equal(Array1.Values.Length, 1);
					SRAssert.Equal(Array3.Values.Length, 20);
					break;
				}
				case "join":
				case "split":
				{
					SRAssert.Equal(Array1.Values.Length, 2);
					SRAssert.Equal(Array3.Values.Length, 40);
					break;
				}
				default: throw new Exception();
			}

			for (int i = 0; i < Array3.Values.Length; i++)
			{
				Array3.Values[i] = new ArrData3(r, xds);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(PieceName), PieceName);
			sb.AppendLine(nameof(JunctionType), JunctionType);

			sb.AppendLine(nameof(UnkUshort1), UnkUshort1);
			sb.AppendLine(nameof(UnkUshort2), UnkUshort2);

			sb.NewNode();

			sb.NewArray(nameof(Array1), Array1.Values.Length);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.NewArray(nameof(Array2), Array2.Values.Length);
			for (int i = 0; i < Array2.Values.Length; i++)
			{
				Array2.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.NewArray(nameof(Array3), Array3.Values.Length);
			for (int i = 0; i < Array3.Values.Length; i++)
			{
				Array3.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
}