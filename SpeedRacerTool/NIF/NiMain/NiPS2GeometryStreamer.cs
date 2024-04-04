using Kermalis.EndianBinaryIO;
using Kermalis.SRGLTF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPS2GeometryStreamer : NiObject
{
	private sealed class Mesh
	{
		private struct Vertex
		{
			public Vector3 Pos;
			// Either 0x0 or 0x8000, doesn't seem related to textures.
			public uint D;

			public Vertex(EndianBinaryReader r)
			{
				Pos = r.ReadVector3();
				D = r.ReadUInt32();
				if (D is not 0 and not 0x8000)
				{
					throw new Exception();
				}
			}

			public override readonly string ToString()
			{
				return string.Format("(X={0} | Y={1} | Z={2} | D=0x{3:X})",
					Pos.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC),
					Pos.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC),
					Pos.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC),
					D);
			}
		}
		public struct SecondThing
		{
			// Seems to be TexCoords related

			// geo files: [0, 8192], continuous values. But doesn't match expected U coordinates... Just wraps around the track pieces horizontally, even for tunnels
			// model files: ???
			public short A;
			// So far [-4569, 1788], continuous values. Seems to represent V coordinates but also weird... the values are mostly negative
			// TODO: Check how this looks as two bytes
			public short B;

			public static HashSet<short> _aVals = [];
			public static HashSet<short> _bVals = [];

			public SecondThing(EndianBinaryReader r)
			{
				A = r.ReadInt16();
				B = r.ReadInt16();

				_bVals.Add(B);
			}

			public static void Test()
			{
				Console.WriteLine("2Thing A values: [{0}]", string.Join(", ", _aVals.OrderDescending()));
				Console.WriteLine("2Thing B values: [{0}]", string.Join(", ", _bVals.OrderDescending()));
			}

			public override readonly string ToString()
			{
				string s16 = string.Format("({0} | {1})", A, B);
				string u16 = string.Format("(0x{0:X4} | 0x{1:X4})", (ushort)A, (ushort)B);
				string half = string.Format("({0} | {1})", BitConverter.Int16BitsToHalf(A).ToString(SRUtils.TOSTRING_NO_SCIENTIFIC), BitConverter.Int16BitsToHalf(B).ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
				uint u = (ushort)A | ((uint)B << 16);
				string s32 = string.Format("{0}", (int)u);
				string u32 = string.Format("0x{0:X8}", u);

				return string.Format("(S16={0} ... U16={1} ... H={2} ... S32={3} ... U32={4})", s16, u16, half, s32, u32);
			}
		}
		public struct ThirdThing
		{
			/// <summary>Baked shadows, though not really visible in-game at all. Higher values are in light, lower values in shade. Also considers point lights.</summary>
			public short Shade;
			// TODO: "B" looks similar to "Shade" but I can't put my finger on what's weird about it
			public ushort B;

			public static HashSet<ushort> _bVals = [];

			public ThirdThing(EndianBinaryReader r)
			{
				Shade = r.ReadInt16();
				B = r.ReadUInt16();

				_bVals.Add(B);
			}

			public static void Test()
			{
				Console.WriteLine("3Thing B values: [{0}]", string.Join(", ", _bVals.OrderDescending())); // Why is this sometimes cut off in the log file?
			}

			public override readonly string ToString()
			{
				string s16 = string.Format("({0} | {1})", Shade, B);
				string u16 = string.Format("(0x{0:X4} | 0x{1:X4})", (ushort)Shade, B);
				string half = string.Format("({0} | {1})", BitConverter.Int16BitsToHalf(Shade).ToString(SRUtils.TOSTRING_NO_SCIENTIFIC), BitConverter.Int16BitsToHalf((short)B).ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
				uint u = (ushort)Shade | ((uint)B << 16);
				string s32 = string.Format("{0}", (int)u);
				string u32 = string.Format("0x{0:X8}", u);

				return string.Format("(S16={0} ... U16={1} ... H={2} ... S32={3} ... U32={4})", s16, u16, half, s32, u32);
			}
		}

		private readonly Vertex[] _vertices;
		// Makes way more sense as s16 at least in the ones I checked...
		private readonly SecondThing[]? _unknown2;
		// Looks like two u16 flags
		private readonly ThirdThing[]? _unknown3;

		// TODO: Where are the normals?
		// TODO: What specifies texture index?

		public readonly ChunkPtr<NiTriBasedGeomData> TriData;
		public readonly byte Kind;
		public readonly byte Unk;

		public Mesh(EndianBinaryReader r)
		{
			// tunnelcap (NumVerts=142 | NumTris=80 | NumStrips=43)
			//   0xA60 bytes of first stuff (166 thingies of 16 bytes each). 2 empty rows after (0xA80 total).
			//   0x2A0 bytes of second stuff with 0x60 empty after. 0x298 are used. 166 rows of 4 with 26 empty rows after (0x300 total).
			//   0x2A0 bytes of third stuff with 0x60 empty after. 0x298 are used. 166 rows of 4 with 26 empty rows after (0x300 total).

			// endcap (NumVerts=92 | NumTris=76 | NumStrips=32)
			//   0x8C0 bytes of first stuff (140 thingies of 16 bytes each). 4 empty rows after (0x900 total).
			//   0x230 bytes of second stuff with 0x50 empty after. 140 rows of 4 with 20 empty rows after (0x280 total).
			//   0x230 bytes of third stuff (0xFF) with 0x50 empty after. 140 rows of 4 with 20 empty rows after (0x280 total).

			long start = r.Stream.Position;

			TriData = new ChunkPtr<NiTriBasedGeomData>(r);

			// 1: 18/16_644. Example: "ps2_ps2\tracks\t01\models\t01gtl.nif"
			// 3: 512/16_644. Example: "ps2_ps2\game\models\v01_mach6\v01gsra.nif"
			// 4: 385/16_644. Example: "ps2_ps2\tracks\t06\models\t06gsk.nif"
			// 5: 15_517/16_644
			// 6: 1/16_644. Only in "ps2_ps2\tracks\t03\models\t03gfji.nif"
			// 7: 211/16_644. Example: "ps2_ps2\game\models\v01_mach6\v01gsrp.nif"
			Kind = r.ReadByte();

			int numVertices = r.ReadInt32();

			// 1: 16_525/16_644. maybe every model uses it at least once
			// 2: 106/16_644. Example: "ps2_ps2\game\models\v01_mach6\v01gsrp.nif"
			// 3: 13/16_644. Example: "ps2_ps2\game\models\v05_taejotogokahn\v05gtta.nif"
			Unk = r.ReadByte();

			uint remainingByteLen = r.ReadUInt32(); // remainingByteLen. Used to easily skip this mesh
			long end = r.Stream.Position + remainingByteLen;

			int expectNear = (numVertices * 4 * sizeof(float)) + (numVertices * 2 * sizeof(short)) + (numVertices * 2 * sizeof(short));
			//Console.WriteLine("Mesh @ 0x{0:X}: Verts = {1} | ExpectNear = 0x{2:X} | Len = 0x{3:X} | Kind = {4} | Unk = {5}",
			//start, numVertices, expectNear, remainingByteLen, Kind, Unk);

			long dataBegin = r.Stream.Position;
			long portionBegin = dataBegin;

			_vertices = new Vertex[numVertices];
			for (int i = 0; i < _vertices.Length; i++)
			{
				_vertices[i] = new Vertex(r);
			}

			// TODO: Each type has different data following. The code below was for kind 5
			//bool skipThisNonsense = Kind != 5;
			bool skipThisNonsense = true;
			if (skipThisNonsense)
			{
				r.Stream.Position = end;
				return;
			}

			portionBegin = JumpToNextPortion(portionBegin, r.Stream.Position);
			r.Stream.Position = portionBegin;

			_unknown2 = new SecondThing[numVertices];
			for (int i = 0; i < _unknown2.Length; i++)
			{
				_unknown2[i] = new SecondThing(r);
			}

			portionBegin = JumpToNextPortion(portionBegin, r.Stream.Position);
			r.Stream.Position = portionBegin;

			_unknown3 = new ThirdThing[numVertices];
			for (int i = 0; i < _unknown3.Length; i++)
			{
				_unknown3[i] = new ThirdThing(r);
			}

			r.Stream.Position = JumpToNextPortion(portionBegin, r.Stream.Position);
		}

		private static long JumpToNextPortion(long portionBegin, long offset)
		{
			offset -= portionBegin;
			while (offset % 0x80 != 0)
			{
				offset++;
			}
			return offset + portionBegin;
		}

		public void Output(OBJBuilder obj, StringBuilder sbDebug, NIFFile nif, int index)
		{
			// GroupID, Keep Flags, DataFlags = always 0 in Fuji
			// CompressFlags = always 31 in Fuji (0b0001_1111)
			// TODO: What are the CompressFlags? Can it explain the nonsense above?

			NiTriBasedGeomData data = TriData.Resolve(nif);

			Console.WriteLine("Saving {0} ({1})", index, data.GetType().Name);

			obj.AddObject(string.Format("\"[{0}] K={1} U={2} C={3} F={4}\"", index, Kind, Unk, data.BoundingSphere.Center, data.CompressFlags));
			sbDebug.AppendLine(string.Format("Kind: {0}, Unk: {1}, Center: {2}, CompFlags: {3}", Kind, Unk, data.BoundingSphere.Center, data.CompressFlags));

			// Output verts
			for (int i = 0; i < _vertices.Length; i++)
			{
				ref Vertex v = ref _vertices[i];

				sbDebug.Append(i);
				sbDebug.AppendLine(":");
				sbDebug.AppendLine("\tVertex " + v);

				if (_unknown2 is null || _unknown3 is null)
				{
					obj.AddVertex(v.Pos);
				}
				else
				{
					ref SecondThing st = ref _unknown2[i];
					ref ThirdThing tt = ref _unknown3[i];

					// Vertex colors test
					float dThing = v.D / 32_768f;

					/*float stU = st.A / 8192f;
					float stV = st.B / 5_000f;
					float stV_0To1 = (stV * 0.5f) + 0.5f;
					obj.AddVertex(v.Pos, new Vector3(stU, stV_0To1, dThing));*/

					float ttU = tt.Shade / 32_768f; // Can be negative/positive
					float ttU_0To1 = (ttU * 0.5f) + 0.5f;
					float ttV = (short)tt.B / 32_768f;
					float ttV_0To1 = (ttV * 0.5f) + 0.5f;
					float ttVUnsigned = tt.B / 65_535f;
					obj.AddVertex(v.Pos, new Vector3(ttU_0To1, ttV_0To1, ttVUnsigned));

					// DEBUG

					sbDebug.AppendLine("\tUnk2 " + st);
					sbDebug.AppendLine("\tUnk3 " + tt);
				}
			}

			switch (data)
			{
				case NiTriStripsData strips:
				{
					// Need to de-strip the triangles
					int vertIdx = 1;
					foreach (ushort numVertsInThisStrip in strips.StripLengths)
					{
						// Output each face as a tri. just 3 verts.
						// example:
						// "f 49 50 51 52" -> "f 49 50 51" and "f 50 51 52"
						// "f 95 96 97 98 99" -> "f 95 96 97" and "f 96 97 98" and "f 97 98 99"

						int numOutputTris = numVertsInThisStrip - 2;
						for (int tri = 0; tri < numOutputTris; tri++)
						{
							obj.AddFace(vertIdx + tri, vertIdx + tri + 1, vertIdx + tri + 2);
						}
						vertIdx += numVertsInThisStrip;
					}
					break;
				}
				case NiTriShapeData shape: // Only used in t03gfji.nif (12 times out of 16_644). Perhaps only useful when tris aren't connected, so strips can't be used
				{
					if (shape.Triangles is null)
					{
						throw new Exception();
					}
					foreach (NiTriShapeData.Tri tri in shape.Triangles)
					{
						obj.AddFace(tri.VertID0, tri.VertID1, tri.VertID2);
					}
					break;
				}
				default:
				{
					throw new Exception();
				}
			}
		}
		public void Output(GLTFNode node, NIFFile nif)
		{
			NiTriBasedGeomData data = TriData.Resolve(nif);

			node.Translation = data.BoundingSphere.Center;

			GLTFMesh mesh = node.CreateMesh();
		}
	}

	private readonly Mesh[] _meshes;

	internal NiPS2GeometryStreamer(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		//Console.WriteLine("NiPS2GeometryStreamer found in " + ((FileStream)r.Stream).Name);

		_meshes = new Mesh[r.ReadUInt32()]; // Matches the number of NiTriStripsData/NiTriShapeData in the .nif

		for (int i = 0; i < _meshes.Length; i++)
		{
			_meshes[i] = new Mesh(r);
		}

		//Console.WriteLine();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiPS2GeometryStreamer));
	}

	public void TestOBJ(NIFFile nif, string dir, bool overwrite)
	{
		if (Directory.Exists(dir))
		{
			if (!overwrite)
			{
				return;
			}
			Directory.Delete(dir, true);
		}
		Directory.CreateDirectory(dir);

		Console.WriteLine("Saving to " + dir);

		Mesh.SecondThing.Test();
		Mesh.ThirdThing.Test();

		var obj = new OBJBuilder();
		var sbDebug = new StringBuilder();

		for (int i = 0; i < _meshes.Length; i++)
		{
			obj.Clear();
			sbDebug.Clear();

			_meshes[i].Output(obj, sbDebug, nif, i);

			obj.Write(Path.Combine(dir, i + ".obj"));
			File.WriteAllText(Path.Combine(dir, i + ".txt"), sbDebug.ToString());
		}
	}
	public void TestGLTF(NIFFile nif, string name)
	{
		// TODO: Rotation per mesh. For example, t06gsk mesh 255 is part of the rocket in skorost. it should be between two tracks but it's rotated the wrong way atm.
		// There also seems to be a transform, for example the skorost flags are offset from the base itself, not rotated. There's no way to rotate from their origin to where they are without offsetting the positions
		// With GLTF, we should be able to specify the object origin and its rotation. But where is the rotation defined?

		using (var gltf = new GLTFWriter())
		{
			GLTFScene scene = gltf.CreateScene();
			scene.Name = name;

			for (int i = 0; i < _meshes.Length; i++)
			{
				GLTFNode node = scene.CreateNode();
				node.Name = $"[{i}]";
				_meshes[i].Output(node, nif);
			}

			using (FileStream s = File.Create(@"C:\Users\Kermalis\Downloads\Test.glb"))
			{
				gltf.WriteGLB(s);
			}
		}
	}
}