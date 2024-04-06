using Kermalis.EndianBinaryIO;
using System.IO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class PhysicsPropsChunk : XDSChunk
{
	public sealed class Entry
	{
		/// <summary>The 3 vertices to use to create a triangle, like how .obj works</summary>
		public struct ConvexData1
		{
			public ushort[] VertexIndices;

			internal ConvexData1(EndianBinaryReader r, XDSFile xds)
			{
				VertexIndices = new ushort[3];
				xds.ReadFileUInt16s(r, VertexIndices);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			public override readonly string ToString()
			{
				return string.Join(", ", VertexIndices);
			}
		}
		/// <summary>The vertices that are available to the convex</summary>
		public struct ConvexData2
		{
			public Vector3 Data;

			internal ConvexData2(EndianBinaryReader r, XDSFile xds)
			{
				Data = xds.ReadFileVector3(r);
				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			public override readonly string ToString()
			{
				return Data.ToString();
			}
		}

		public MagicValue Magic_CollisionShape;
		public Magic_OneAyyArray Magic_ConvexArray1;
		public Magic_OneAyyArray Magic_ConvexArray2;
		public uint HeightfieldWL1; // Which is width and which is length?
		public uint HeightfieldWL2;
		public Magic_OneAyyArray Magic_HeightfieldData;
		public float Radius;
		public float CapsuleHeight;
		public Vector3 BoxScale;
		public Vector3 EulerRot; // TODO: Verify
		public Vector3 Pos; // TODO: Verify

		// Node data
		public OneBeeString CollisionShape;
		public OneAyyArray<ConvexData1> ConvexArray1;
		public OneAyyArray<ConvexData2> ConvexArray2;
		public OneAyyArray<uint> HeightfieldData; // TODO: The values indicate how high off the ground this x/z coordinate is, but what unit?

		internal Entry(EndianBinaryReader r, XDSFile xds)
		{
			Magic_CollisionShape = new MagicValue(r);
			MagicValue.ReadEmpty(r); // Magic value for the empty string
			Magic_ConvexArray1 = new Magic_OneAyyArray(r, xds);
			Magic_ConvexArray2 = new Magic_OneAyyArray(r, xds);
			HeightfieldWL1 = xds.ReadFileUInt32(r);
			HeightfieldWL2 = xds.ReadFileUInt32(r);
			Magic_OneAyyArray.ReadEmpty(r);
			Magic_HeightfieldData = new Magic_OneAyyArray(r, xds);
			Magic_OneAyyArray.ReadEmpty(r);
			Radius = xds.ReadFileSingle(r);
			CapsuleHeight = xds.ReadFileSingle(r);
			BoxScale = xds.ReadFileVector3(r);
			EulerRot = xds.ReadFileVector3(r);
			Pos = xds.ReadFileVector3(r);

			// NODE START
			XDSFile.ReadNodeStart(r);

			CollisionShape = new OneBeeString(r);

			switch (CollisionShape.Str)
			{
				case "BOX":
				{
					SRAssert.Equal(Radius, 0);
					SRAssert.Equal(CapsuleHeight, 0);
					SRAssert.NotEqual(BoxScale, Vector3.Zero);
					SRAssert.Equal(HeightfieldWL1, 0);
					SRAssert.Equal(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertIs0();
					Magic_ConvexArray2.AssertIs0();
					Magic_HeightfieldData.AssertIs0();
					break;
				}
				case "CAPSULE":
				{
					SRAssert.NotEqual(Radius, 0);
					SRAssert.NotEqual(CapsuleHeight, 0);
					SRAssert.Equal(BoxScale, Vector3.Zero);
					SRAssert.Equal(HeightfieldWL1, 0);
					SRAssert.Equal(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertIs0();
					Magic_ConvexArray2.AssertIs0();
					Magic_HeightfieldData.AssertIs0();
					break;
				}
				case "CONVEX": // WII version doesn't use this
				{
					SRAssert.Equal(Radius, 0);
					SRAssert.Equal(CapsuleHeight, 0);
					SRAssert.Equal(BoxScale, Vector3.Zero);
					SRAssert.Equal(EulerRot, Vector3.Zero);
					SRAssert.Equal(Pos, Vector3.Zero);
					SRAssert.Equal(HeightfieldWL1, 0);
					SRAssert.Equal(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertNot0();
					Magic_ConvexArray2.AssertNot0();
					Magic_HeightfieldData.AssertIs0();
					break;
				}
				case "HEIGHTFIELD":
				{
					SRAssert.Equal(Radius, 0);
					SRAssert.Equal(CapsuleHeight, 0);
					SRAssert.NotEqual(BoxScale, Vector3.Zero);
					SRAssert.NotEqual(HeightfieldWL1, 0);
					SRAssert.NotEqual(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertIs0();
					Magic_ConvexArray2.AssertIs0();
					Magic_HeightfieldData.AssertEqual(HeightfieldWL1 * HeightfieldWL2);
					break;
				}
				case "MESH": // PS2 version doesn't use this
				{
					SRAssert.Equal(Radius, 0);
					SRAssert.Equal(CapsuleHeight, 0);
					SRAssert.Equal(BoxScale, Vector3.Zero);
					SRAssert.Equal(EulerRot, Vector3.Zero);
					SRAssert.Equal(Pos, Vector3.Zero);
					SRAssert.Equal(HeightfieldWL1, 0);
					SRAssert.Equal(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertNot0();
					Magic_ConvexArray2.AssertNot0();
					Magic_HeightfieldData.AssertIs0();
					break;
				}
				case "SPHERE":
				{
					SRAssert.NotEqual(Radius, 0);
					SRAssert.Equal(CapsuleHeight, 0);
					SRAssert.Equal(BoxScale, Vector3.Zero);
					SRAssert.Equal(HeightfieldWL1, 0);
					SRAssert.Equal(HeightfieldWL2, 0);
					Magic_ConvexArray1.AssertIs0();
					Magic_ConvexArray2.AssertIs0();
					Magic_HeightfieldData.AssertIs0();
					break;
				}
				default: throw new InvalidDataException();
			}

			OneBeeString.ReadEmpty(r);

			ConvexArray1 = new OneAyyArray<ConvexData1>(r);
			ConvexArray1.AssertMatch(Magic_ConvexArray1);
			for (int i = 0; i < ConvexArray1.Values.Length; i++)
			{
				ConvexArray1.Values[i] = new ConvexData1(r, xds);
			}

			ConvexArray2 = new OneAyyArray<ConvexData2>(r);
			ConvexArray2.AssertMatch(Magic_ConvexArray2);
			for (int i = 0; i < ConvexArray2.Values.Length; i++)
			{
				ConvexArray2.Values[i] = new ConvexData2(r, xds);
			}

			OneAyyArray<object>.ReadEmpty(r);

			HeightfieldData = new OneAyyArray<uint>(r);
			HeightfieldData.AssertMatch(Magic_HeightfieldData);
			r.ReadUInt32s(HeightfieldData.Values);

			OneAyyArray<object>.ReadEmpty(r);

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine(nameof(HeightfieldWL1), HeightfieldWL1, hex: false);
			sb.AppendLine(nameof(HeightfieldWL2), HeightfieldWL2, hex: false);
			sb.AppendLine(nameof(Radius), Radius);
			sb.AppendLine(nameof(CapsuleHeight), CapsuleHeight);
			sb.AppendLine(nameof(BoxScale), BoxScale);
			sb.AppendLine(nameof(EulerRot), EulerRot);
			sb.AppendLine(nameof(Pos), Pos);

			// TODO
			sb.NewNode();

			sb.AppendLine(nameof(CollisionShape), CollisionShape);

			sb.EmptyArray();

			sb.NewArray(ConvexArray1.Values.Length);
			for (int i = 0; i < ConvexArray1.Values.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine_NoQuotes(ConvexArray1.Values[i].ToString(), indent: false);
			}
			sb.EndArray();

			sb.NewArray(ConvexArray2.Values.Length);
			for (int i = 0; i < ConvexArray2.Values.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine(ConvexArray2.Values[i].Data, indent: false);
			}
			sb.EndArray();

			sb.EmptyArray();

			sb.NewArray(HeightfieldData.Values.Length);
			for (int i = 0; i < HeightfieldData.Values.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine_NoQuotes("0x" + HeightfieldData.Values[i].ToString("X"), indent: false);
			}
			sb.EndArray();

			sb.EmptyArray();

			sb.EndNode();

			sb.EndObject();
		}

		public override string ToString()
		{
			return CollisionShape.ToString();
		}
	}

	public MagicValue Magic_Name;
	public Magic_OneAyyArray Magic_Entries;

	// Node data
	public OneBeeString Name;
	public OneAyyArray<Entry> Entries;

	internal PhysicsPropsChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0124);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_Name = new MagicValue(r);
		Magic_OneAyyArray.ReadEmpty(r);
		Magic_Entries = new Magic_OneAyyArray(r, xds);
		Magic_OneAyyArray.ReadEmpty(r);

		for (int i = 0; i < 6; i++)
		{
			SRAssert.Equal(r.ReadUInt32(), 0x00000000);
		}

		// NODE START
		XDSFile.ReadNodeStart(r);

		Name = new OneBeeString(r);

		OneAyyArray<object>.ReadEmpty(r);

		Entries = new OneAyyArray<Entry>(r);
		Entries.AssertMatch(Magic_Entries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		OneAyyArray<object>.ReadEmpty(r);

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.NewNode();

		sb.AppendLine(nameof(Name), Name);
		sb.EmptyArray();

		sb.NewArray(Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EmptyArray();

		sb.EndNode();
	}
}