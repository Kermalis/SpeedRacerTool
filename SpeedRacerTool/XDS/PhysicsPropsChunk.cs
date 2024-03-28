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

		public override string ToString()
		{
			return CollisionShape.ToString();
		}
	}

	public MagicValue Magic28;
	public MagicValue Magic38;

	public OneBeeString Name;
	public OneAyyArray<Entry> Entries;

	internal PhysicsPropsChunk(EndianBinaryReader r, XDSFile xds)
	{
		XDSFile.AssertValue(xds.Unk24, 0x24);
		XDSFile.AssertValue(xds.NumMabStreamNodes, 0x0001);

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

		XDSFile.ReadChunkEnd(r);
	}

	// t01_phx_props_nodmg.xds - track data
	//  0x00-0x0F = Header
	//   fileType = 0xAB90DE70
	//  0x10-0x25 = MabStream header
	//   len = 0x50F
	//   Unk24 = 0x24
	//   NumNodes = 0x0001
	//  0x28 = (uint_LE) = [magic1] 0x0034BA98 in PS2, 0x003ABCC0 in WII
	//  0x2C-0x33 = all 00s (8 00s to be exact, which is room for 2 uints)

	//  0x34 (uint) = 8 (Uses file endianness)
	//  0x38 = (uint_LE) = [magic1] 0x0034BAB8 in PS2, 0x003ABCE0 in WII

	//  0x3C-0x5B = all 00s (room for 8 uints)
	//  0x5C = (LE)0x0009
	//  <
	//   0x5E: [OneBeeString] = "t01_phx_props_nodmg"
	//   0x77: [OneAyyArray](0)
	//   0x7D: [OneAyyArray](8) // each entry is variable length
	//   {
	//    [magic1]
	//    0x3C 00s (which is room for 16 uints)
	//    9 floats using file endianness
	//    (LE)0x0009
	//    <
	//     [OneBeeString] // collision shape
	//     [OneBeeString] = ""
	//     [OneAyyArray](0)
	//     [OneAyyArray](0)
	//     [OneAyyArray](0)
	//     [OneAyyArray](0)
	//     [OneAyyArray](0)
	//     (LE)0x001C
	//    >
	//   }
	//   [OneAyyArray](0)
	//   (LE)0x001C
	//  >
	//  (LE)0x0000
}