using Kermalis.EndianBinaryIO;
using System.IO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PhysicsPropsChunk
{
	public sealed partial class Entry
	{
		public MagicValue Magic_CollisionShape;
		public Magic_OneAyyArray Magic_ConvexArray1;
		public Magic_OneAyyArray Magic_ConvexArray2;
		public uint HeightfieldWL1; // Which is width and which is length? They are equal in t01 for example
		public uint HeightfieldWL2;
		public Magic_OneAyyArray Magic_HeightfieldDatas;
		public float Radius;
		public float CapsuleHeight;
		public Vector3 BoxScale;
		public Vector3 EulerRot;
		public Vector3 Pos;

		// Node data
		public OneBeeString CollisionShape;
		public OneAyyArray<ConvexData1> ConvexArray1;
		public OneAyyArray<ConvexData2> ConvexArray2;
		public OneAyyArray<HeightfieldData> HeightfieldDatas;

		internal Entry(EndianBinaryReader r, XDSFile xds)
		{
			Magic_CollisionShape = new MagicValue(r);
			MagicValue.ReadEmpty(r); // Magic value for the empty string
			Magic_ConvexArray1 = new Magic_OneAyyArray(r, xds);
			Magic_ConvexArray2 = new Magic_OneAyyArray(r, xds);
			HeightfieldWL1 = xds.ReadFileUInt32(r);
			HeightfieldWL2 = xds.ReadFileUInt32(r);
			Magic_OneAyyArray.ReadEmpty(r);
			Magic_HeightfieldDatas = new Magic_OneAyyArray(r, xds);
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
					Magic_HeightfieldDatas.AssertIs0();
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
					Magic_HeightfieldDatas.AssertIs0();
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
					Magic_HeightfieldDatas.AssertIs0();
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
					Magic_HeightfieldDatas.AssertEqual(HeightfieldWL1 * HeightfieldWL2);
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
					Magic_HeightfieldDatas.AssertIs0();
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
					Magic_HeightfieldDatas.AssertIs0();
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

			HeightfieldDatas = new OneAyyArray<HeightfieldData>(r);
			HeightfieldDatas.AssertMatch(Magic_HeightfieldDatas);
			for (int i = 0; i < HeightfieldDatas.Values.Length; i++)
			{
				HeightfieldDatas.Values[i] = new HeightfieldData(r, xds);
			}

			OneAyyArray<object>.ReadEmpty(r);

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(HeightfieldWL1), HeightfieldWL1, hex: false);
			sb.AppendLine(nameof(HeightfieldWL2), HeightfieldWL2, hex: false);
			sb.AppendLine(nameof(Radius), Radius);
			sb.AppendLine(nameof(CapsuleHeight), CapsuleHeight);
			sb.AppendLine(nameof(BoxScale), BoxScale);
			sb.AppendLine(nameof(EulerRot), EulerRot);
			sb.AppendLine(nameof(Pos), Pos);

			sb.NewNode();

			sb.AppendLine(nameof(CollisionShape), CollisionShape);

			sb.EmptyArray();

			sb.NewArray(nameof(ConvexArray1), ConvexArray1.Values.Length);
			for (int i = 0; i < ConvexArray1.Values.Length; i++)
			{
				ConvexArray1.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.NewArray(nameof(ConvexArray2), ConvexArray2.Values.Length);
			for (int i = 0; i < ConvexArray2.Values.Length; i++)
			{
				ConvexArray2.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EmptyArray();

			sb.NewArray(nameof(HeightfieldDatas), HeightfieldDatas.Values.Length);
			for (int i = 0; i < HeightfieldDatas.Values.Length; i++)
			{
				HeightfieldDatas.Values[i].DebugStr(sb, i);
			}
			sb.EndArray();

			sb.EmptyArray();

			sb.EndNode();

			sb.EndObject();
		}

		public void TestOBJHeightfield(OBJBuilder obj)
		{
			// Pos is <-1000.32, 170.721, -513.68> in t01. In blender I had to <x, -z, y> for it to match.
			// Maybe the pivot should be in the center?
			// TODO: Rotation is correct, but again the pivot needs testing
			float xScale = 1f / (HeightfieldWL1 - 1);
			float zScale = 1f / (HeightfieldWL2 - 1);

			int i = 0;
			for (int z = 0; z < HeightfieldWL2; z++)
			{
				for (int x = 0; x < HeightfieldWL1; x++)
				{
					var v = new Vector3(x * xScale, HeightfieldDatas.Values[i++].Val, z * zScale);
					obj.AddVertex(v * BoxScale);
				}
			}
		}

		public override string ToString()
		{
			return CollisionShape.ToString();
		}
	}
}