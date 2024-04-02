using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class PS2TrackChunk : XDSChunk
{
	public sealed class Array1Data
	{
		public sealed class ArrArr1Data
		{
			public struct ArrArrArr1Data
			{
				public float[] Data;

				internal ArrArrArr1Data(EndianBinaryReader r, XDSFile xds)
				{
					Data = new float[10];
					xds.ReadFileSingles(r, Data);

					SRAssert.Equal(r.ReadUInt16(), 0x0000);
				}

				internal readonly void DebugStr(XDSStringBuilder sb, int index)
				{
					sb.AppendLine_ArrayElement(index);
					sb.NewObject();

					sb.NewArray(Data.Length);
					for (int i = 0; i < Data.Length; i++)
					{
						sb.Append_ArrayElement(i);
						sb.AppendLine(Data[i], indent: false);
					}
					sb.EndArray();

					sb.EndObject();
				}
			}

			public Magic_OneAyyArray Magic1;

			// Node data
			public OneAyyArray<ArrArrArr1Data> Array1;

			internal ArrArr1Data(EndianBinaryReader r, XDSFile xds)
			{
				Magic1 = new Magic_OneAyyArray(r, xds);

				// NODE START
				XDSFile.ReadNodeStart(r);

				Array1 = new OneAyyArray<ArrArrArr1Data>(r);
				Array1.AssertMatch(Magic1);
				for (int i = 0; i < Array1.Values.Length; i++)
				{
					Array1.Values[i] = new ArrArrArr1Data(r, xds);
				}

				XDSFile.ReadNodeEnd(r);
				// NODE END
			}

			internal void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.AppendLine_ArrayElement(index);
				sb.NewObject();

				sb.NewNode();

				sb.NewArray(Array1.Values.Length);
				for (int i = 0; i < Array1.Values.Length; i++)
				{
					Array1.Values[i].DebugStr(sb, i);
				}
				sb.EndArray();

				sb.EndNode();

				sb.EndObject();
			}
		}
		public sealed class ArrArr2Data
		{
			public uint UnkUint1;
			public float Unk4;
			public string UnkStr1;
			public string UnkStr2;
			public float UnkFloat1;
			public uint UnkUint2;
			public float[] Data;

			internal ArrArr2Data(EndianBinaryReader r, XDSFile xds)
			{
				UnkUint1 = r.ReadUInt32();
				Unk4 = xds.ReadFileSingle(r);
				UnkStr1 = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkStr2 = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkFloat1 = xds.ReadFileSingle(r);
				UnkUint2 = r.ReadUInt32();

				Data = new float[6];
				xds.ReadFileSingles(r, Data);

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.AppendLine_ArrayElement(index);
				sb.NewObject();

				sb.AppendLine(nameof(UnkUint1), UnkUint1);
				sb.AppendLine(nameof(Unk4), Unk4);
				sb.AppendLine_Quotes(UnkStr1);
				sb.AppendLine_Quotes(UnkStr2);
				sb.AppendLine(nameof(UnkFloat1), UnkFloat1);
				sb.AppendLine(nameof(UnkUint2), UnkUint2);

				sb.NewArray(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					sb.Append_ArrayElement(i);
					sb.AppendLine(Data[i], indent: false);
				}
				sb.EndArray();

				sb.EndObject();
			}
		}
		public struct ArrArr3Data
		{
			public float[] Data;

			internal ArrArr3Data(EndianBinaryReader r, XDSFile xds)
			{
				Data = new float[8];
				xds.ReadFileSingles(r, Data);

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.AppendLine_ArrayElement(index);
				sb.NewObject();

				sb.NewArray(Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					sb.Append_ArrayElement(i);
					sb.AppendLine(Data[i], indent: false);
				}
				sb.EndArray();

				sb.EndObject();
			}
		}

		public string PieceName;
		public string UnkStr20;
		public Magic_OneAyyArray Magic1;
		public ushort UnkUshort1;
		public ushort UnkUshort2;
		public Magic_OneAyyArray Magic2;
		public Magic_OneAyyArray Magic3;

		// Node data
		public OneAyyArray<ArrArr1Data> Array1;
		public OneAyyArray<ArrArr2Data> Array2;
		public OneAyyArray<ArrArr3Data> Array3;

		internal Array1Data(EndianBinaryReader r, XDSFile xds)
		{
			PieceName = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr20 = r.ReadString_Count_TrimNullTerminators(0x10);

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

			Array1 = new OneAyyArray<ArrArr1Data>(r);
			Array1.AssertMatch(Magic1);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i] = new ArrArr1Data(r, xds);
			}

			Array2 = new OneAyyArray<ArrArr2Data>(r);
			Array2.AssertMatch(Magic2);
			for (int i = 0; i < Array2.Values.Length; i++)
			{
				Array2.Values[i] = new ArrArr2Data(r, xds);
			}

			Array3 = new OneAyyArray<ArrArr3Data>(r);
			Array3.AssertMatch(Magic3);
			for (int i = 0; i < Array3.Values.Length; i++)
			{
				Array3.Values[i] = new ArrArr3Data(r, xds);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(PieceName);
			sb.AppendLine_Quotes(UnkStr20);

			sb.AppendLine(nameof(UnkUshort1), UnkUshort1);
			sb.AppendLine(nameof(UnkUshort2), UnkUshort2);

			sb.NewNode();

			sb.NewArray(Array1.Values.Length);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.NewArray(Array2.Values.Length);
			for (int i = 0; i < Array2.Values.Length; i++)
			{
				Array2.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.NewArray(Array3.Values.Length);
			for (int i = 0; i < Array3.Values.Length; i++)
			{
				Array3.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
	public sealed class TrackJoinData
	{
		public string JoinID;
		public string Piece1;
		public string Piece2;

		internal TrackJoinData(EndianBinaryReader r)
		{
			JoinID = r.ReadString_Count_TrimNullTerminators(0x20);
			Piece1 = r.ReadString_Count_TrimNullTerminators(0x24);
			Piece2 = r.ReadString_Count_TrimNullTerminators(0x24);
			SRAssert.Equal(r.ReadUInt16(), 0x0000);
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(JoinID);
			sb.AppendLine_Quotes(Piece1);
			sb.AppendLine_Quotes(Piece2);

			sb.EndObject();
		}
	}
	public sealed class Array3Data
	{
		public struct ArrArr1Data
		{
			public string JoinID;
			public bool UnkBool;

			internal ArrArr1Data(EndianBinaryReader r)
			{
				JoinID = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkBool = r.ReadBoolean();

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal readonly void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.AppendLine_ArrayElement(index);
				sb.NewObject();

				sb.AppendLine_Quotes(JoinID);
				sb.AppendLine_Boolean(nameof(UnkBool), UnkBool);

				sb.EndObject();
			}

			public override readonly string ToString()
			{
				return JoinID;
			}
		}

		public string TrackType;
		public Magic_OneAyyArray Magic1;

		// Node data
		public OneAyyArray<ArrArr1Data> Array1;

		internal Array3Data(EndianBinaryReader r, XDSFile xds)
		{
			TrackType = r.ReadString_Count_TrimNullTerminators(0x20);
			Magic1 = new Magic_OneAyyArray(r, xds);

			// NODE START
			XDSFile.ReadNodeStart(r);

			// In editor_template.xds, 5 elements from 0x1E1E-0x1ECC. 0xAF / 5 = 0x23
			Array1 = new OneAyyArray<ArrArr1Data>(r);
			Array1.AssertMatch(Magic1);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i] = new ArrArr1Data(r);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(TrackType);

			sb.NewNode();

			sb.NewArray(Array1.Values.Length);
			for (int i = 0; i < Array1.Values.Length; i++)
			{
				Array1.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
	public sealed class Array4Data
	{
		public struct ArrArr1Data
		{
			public Vector3 Value;

			internal ArrArr1Data(EndianBinaryReader r, XDSFile xds)
			{
				Value = xds.ReadFileVector3(r);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			public override readonly string ToString()
			{
				return Value.ToString();
			}
		}

		public string UnkStr1;
		public string UnkStr2;
		public uint UnkUint1;
		public float UnkFloat1;
		public string UnkStr3;
		public string UnkStr4;
		public string UnkStr5;
		public float UnkFloat2;
		public float UnkFloat3;
		public Magic_OneAyyArray Magic;

		// Node data
		public OneAyyArray<ArrArr1Data> Array;

		internal Array4Data(EndianBinaryReader r, XDSFile xds)
		{
			UnkStr1 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr2 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkUint1 = r.ReadUInt32();
			UnkFloat1 = xds.ReadFileSingle(r);
			UnkStr3 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr4 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr5 = r.ReadString_Count_TrimNullTerminators(0x2C);
			SRAssert.Equal(r.ReadUInt32(), 0x80000000);
			UnkFloat2 = xds.ReadFileSingle(r);
			SRAssert.Equal(r.ReadUInt32(), 0x80000000);
			UnkFloat3 = xds.ReadFileSingle(r);
			Magic = new Magic_OneAyyArray(r, xds); // Still has value even when 0

			// NODE START
			XDSFile.ReadNodeStart(r);

			Array = new OneAyyArray<ArrArr1Data>(r);
			Array.AssertMatch(Magic);
			for (int i = 0; i < Array.Values.Length; i++)
			{
				Array.Values[i] = new ArrArr1Data(r, xds);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(UnkStr1);
			sb.AppendLine_Quotes(UnkStr1);
			sb.AppendLine(nameof(UnkUint1), UnkUint1);
			sb.AppendLine(nameof(UnkFloat1), UnkFloat1);
			sb.AppendLine_Quotes(UnkStr1);
			sb.AppendLine_Quotes(UnkStr1);
			sb.AppendLine_Quotes(UnkStr1);
			sb.AppendLine(nameof(UnkFloat2), UnkFloat2);
			sb.AppendLine(nameof(UnkFloat3), UnkFloat3);

			sb.NewNode();

			sb.NewArray(Array.Values.Length);
			for (int i = 0; i < Array.Values.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine(Array.Values[i].Value, indent: false);
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}

	public string UnkXML;
	public Magic_OneAyyArray Magic1;
	public Magic_OneAyyArray Magic_TrackJoins;
	public Magic_OneAyyArray Magic3;
	public Magic_OneAyyArray Magic4;

	// Node data
	public OneAyyArray<Array1Data> Array1;
	public OneAyyArray<TrackJoinData> TrackJoins;
	public OneAyyArray<Array3Data> Array3;
	public OneAyyArray<Array4Data> Array4;

	internal PS2TrackChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0131);
		SRAssert.Equal(NumNodes, 0x0001);

		UnkXML = r.ReadString_Count_TrimNullTerminators(0x48);
		Magic1 = new Magic_OneAyyArray(r, xds);
		Magic_TrackJoins = new Magic_OneAyyArray(r, xds);
		Magic3 = new Magic_OneAyyArray(r, xds);
		Magic4 = new Magic_OneAyyArray(r, xds);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Array1 = new OneAyyArray<Array1Data>(r);
		Array1.AssertMatch(Magic1);
		for (int i = 0; i < Array1.Values.Length; i++)
		{
			Array1.Values[i] = new Array1Data(r, xds);
		}

		TrackJoins = new OneAyyArray<TrackJoinData>(r);
		TrackJoins.AssertMatch(Magic_TrackJoins);
		for (int i = 0; i < TrackJoins.Values.Length; i++)
		{
			TrackJoins.Values[i] = new TrackJoinData(r);
		}

		Array3 = new OneAyyArray<Array3Data>(r);
		Array3.AssertMatch(Magic3);
		for (int i = 0; i < Array3.Values.Length; i++)
		{
			Array3.Values[i] = new Array3Data(r, xds);
		}

		// In editor_template.xds, 1 element from 0x1ED5-0x1FAA
		Array4 = new OneAyyArray<Array4Data>(r);
		Array4.AssertMatch(Magic4);
		for (int i = 0; i < Array4.Values.Length; i++)
		{
			Array4.Values[i] = new Array4Data(r, xds);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine_Quotes(UnkXML);

		sb.NewNode();

		sb.NewArray(Array1.Values.Length);
		for (int i = 0; i < Array1.Values.Length; i++)
		{
			Array1.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(TrackJoins.Values.Length);
		for (int i = 0; i < TrackJoins.Values.Length; i++)
		{
			TrackJoins.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(Array3.Values.Length);
		for (int i = 0; i < Array3.Values.Length; i++)
		{
			Array3.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(Array4.Values.Length);
		for (int i = 0; i < Array4.Values.Length; i++)
		{
			Array4.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}