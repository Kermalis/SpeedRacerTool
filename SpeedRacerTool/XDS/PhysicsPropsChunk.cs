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
				XDSFile.AssertValue(r.ReadUInt16(), 0x0000);
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
				XDSFile.AssertValue(r.ReadUInt16(), 0x0000);
			}

			public override readonly string ToString()
			{
				return Data.ToString();
			}
		}

		public MagicValue Magic0;
		public MagicValue MagicC;
		public MagicValue Magic14;
		public uint HeightfieldWL1; // Which is width and which is length?
		public uint HeightfieldWL2;
		public MagicValue Magic2C;
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
			Magic0 = new MagicValue(r);

			XDSFile.AssertValue(r.ReadUInt32(), 0x00000000);

			uint numConvex1 = xds.ReadFileUInt32(r);
			MagicC = new MagicValue(r);
			uint numConvex2 = xds.ReadFileUInt32(r);
			Magic14 = new MagicValue(r);
			HeightfieldWL1 = xds.ReadFileUInt32(r);
			HeightfieldWL2 = xds.ReadFileUInt32(r);

			for (int i = 0; i < 2; i++)
			{
				XDSFile.AssertValue(r.ReadUInt32(), 0x00000000);
			}

			uint numHeightfieldData = xds.ReadFileUInt32(r);
			Magic2C = new MagicValue(r);

			for (int i = 0; i < 2; i++)
			{
				XDSFile.AssertValue(r.ReadUInt32(), 0x00000000);
			}

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
					XDSFile.AssertValue(Radius, 0);
					XDSFile.AssertValue(CapsuleHeight, 0);
					XDSFile.AssertValueNot(BoxScale, Vector3.Zero);
					XDSFile.AssertValue(HeightfieldWL1, 0);
					XDSFile.AssertValue(HeightfieldWL2, 0);
					XDSFile.AssertValue(numConvex1, 0);
					XDSFile.AssertValue(numConvex2, 0);
					XDSFile.AssertValue(numHeightfieldData, 0);
					break;
				}
				case "CAPSULE":
				{
					XDSFile.AssertValueNot(Radius, 0);
					XDSFile.AssertValueNot(CapsuleHeight, 0);
					XDSFile.AssertValue(BoxScale, Vector3.Zero);
					XDSFile.AssertValue(HeightfieldWL1, 0);
					XDSFile.AssertValue(HeightfieldWL2, 0);
					XDSFile.AssertValue(numConvex1, 0);
					XDSFile.AssertValue(numConvex2, 0);
					XDSFile.AssertValue(numHeightfieldData, 0);
					break;
				}
				case "CONVEX": // WII version doesn't use this
				{
					XDSFile.AssertValue(Radius, 0);
					XDSFile.AssertValue(CapsuleHeight, 0);
					XDSFile.AssertValue(BoxScale, Vector3.Zero);
					XDSFile.AssertValue(EulerRot, Vector3.Zero);
					XDSFile.AssertValue(Pos, Vector3.Zero);
					XDSFile.AssertValue(HeightfieldWL1, 0);
					XDSFile.AssertValue(HeightfieldWL2, 0);
					XDSFile.AssertValueNot(numConvex1, 0);
					XDSFile.AssertValueNot(numConvex2, 0);
					XDSFile.AssertValue(numHeightfieldData, 0);
					break;
				}
				case "HEIGHTFIELD":
				{
					XDSFile.AssertValue(Radius, 0);
					XDSFile.AssertValue(CapsuleHeight, 0);
					XDSFile.AssertValueNot(BoxScale, Vector3.Zero);
					XDSFile.AssertValueNot(HeightfieldWL1, 0);
					XDSFile.AssertValueNot(HeightfieldWL2, 0);
					XDSFile.AssertValue(numConvex1, 0);
					XDSFile.AssertValue(numConvex2, 0);
					XDSFile.AssertValueNot(numHeightfieldData, 0);
					XDSFile.AssertValue(numHeightfieldData, HeightfieldWL1 * HeightfieldWL2);
					break;
				}
				case "MESH": // PS2 version doesn't use this
				{
					XDSFile.AssertValue(Radius, 0);
					XDSFile.AssertValue(CapsuleHeight, 0);
					XDSFile.AssertValue(BoxScale, Vector3.Zero);
					XDSFile.AssertValue(EulerRot, Vector3.Zero);
					XDSFile.AssertValue(Pos, Vector3.Zero);
					XDSFile.AssertValue(HeightfieldWL1, 0);
					XDSFile.AssertValue(HeightfieldWL2, 0);
					XDSFile.AssertValueNot(numConvex1, 0);
					XDSFile.AssertValueNot(numConvex2, 0);
					XDSFile.AssertValue(numHeightfieldData, 0);
					break;
				}
				case "SPHERE":
				{
					XDSFile.AssertValueNot(Radius, 0);
					XDSFile.AssertValue(CapsuleHeight, 0);
					XDSFile.AssertValue(BoxScale, Vector3.Zero);
					XDSFile.AssertValue(HeightfieldWL1, 0);
					XDSFile.AssertValue(HeightfieldWL2, 0);
					XDSFile.AssertValue(numConvex1, 0);
					XDSFile.AssertValue(numConvex2, 0);
					XDSFile.AssertValue(numHeightfieldData, 0);
					break;
				}
				default: throw new InvalidDataException();
			}

			var emptyStr = new OneBeeString(r);
			XDSFile.AssertValue((ulong)emptyStr.Str.Length, 0);

			ConvexArray1 = new OneAyyArray<ConvexData1>(r);
			XDSFile.AssertValue((ulong)ConvexArray1.Values.Length, numConvex1);
			for (int i = 0; i < ConvexArray1.Values.Length; i++)
			{
				ConvexArray1.Values[i] = new ConvexData1(r, xds);
			}

			ConvexArray2 = new OneAyyArray<ConvexData2>(r);
			XDSFile.AssertValue((ulong)ConvexArray2.Values.Length, numConvex2);
			for (int i = 0; i < ConvexArray2.Values.Length; i++)
			{
				ConvexArray2.Values[i] = new ConvexData2(r, xds);
			}

			var emptyArr = new OneAyyArray<object>(r);
			XDSFile.AssertValue((ulong)emptyArr.Values.Length, 0);

			HeightfieldData = new OneAyyArray<uint>(r);
			XDSFile.AssertValue((ulong)HeightfieldData.Values.Length, numHeightfieldData);
			r.ReadUInt32s(HeightfieldData.Values);

			emptyArr = new OneAyyArray<object>(r);
			XDSFile.AssertValue((ulong)emptyArr.Values.Length, 0);

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

			sb.AppendLine(CollisionShape);

			sb.EmptyArray();

			sb.NewArray(ConvexArray1.Values.Length);
			for (int i = 0; i < ConvexArray1.Values.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine(ConvexArray1.Values[i].ToString(), indent: false);
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
				sb.AppendLine("0x" + HeightfieldData.Values[i].ToString("X"), indent: false);
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

	public MagicValue Magic28;
	public MagicValue Magic38;

	// Node data
	public OneBeeString Name;
	public OneAyyArray<Entry> Entries;

	internal PhysicsPropsChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x0124);
		XDSFile.AssertValue(NumNodes, 0x0001);

		Magic28 = new MagicValue(r);

		for (int i = 0; i < 2; i++)
		{
			XDSFile.AssertValue(r.ReadUInt32(), 0x00000000);
		}

		uint numEntries = xds.ReadFileUInt32(r);
		Magic38 = new MagicValue(r);

		for (int i = 0; i < 8; i++)
		{
			XDSFile.AssertValue(r.ReadUInt32(), 0x00000000);
		}

		// NODE START
		XDSFile.ReadNodeStart(r);

		Name = new OneBeeString(r);

		var emptyArr = new OneAyyArray<object>(r);
		XDSFile.AssertValue((ulong)emptyArr.Values.Length, 0);

		Entries = new OneAyyArray<Entry>(r);
		XDSFile.AssertValue((ulong)Entries.Values.Length, numEntries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		emptyArr = new OneAyyArray<object>(r);
		XDSFile.AssertValue((ulong)emptyArr.Values.Length, 0);

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.NewNode();

		sb.AppendLine(Name);
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