using Kermalis.EndianBinaryIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPS2GeometryStreamer : NIFChunk
{
	public const string NAME = "NiPS2GeometryStreamer";

	private sealed class Mesh
	{
		private struct Vertex
		{
			public float X;
			public float Y;
			public float Z;
			// Either 0x0 or 0x8000
			public uint D;

			public Vertex(EndianBinaryReader r)
			{
				X = r.ReadSingle();
				Y = r.ReadSingle();
				Z = r.ReadSingle();
				D = r.ReadUInt32();
				if (D is not 0 and not 0x8000)
				{
					;
				}
			}

			public override readonly string ToString()
			{
				return string.Format("(X={0} | Y={1} | Z={2} | D=0x{3:X})",
					X.ToString(Program.TOSTRING_NO_SCIENTIFIC),
					Y.ToString(Program.TOSTRING_NO_SCIENTIFIC),
					Z.ToString(Program.TOSTRING_NO_SCIENTIFIC),
					D);
			}
		}
		public struct SecondThing
		{
			// Seems to be TexCoords

			// So far [0, 8192]. Continuous values
			public short A;
			// So far [-4565, 1784]
			public short B;

			public static HashSet<short> _aVals = new();
			public static HashSet<short> _bVals = new();

			public SecondThing(EndianBinaryReader r)
			{
				A = r.ReadInt16();
				B = r.ReadInt16();

				if (A > 0x2000)
				{
					;
				}
				_aVals.Add(A);
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
				string half = string.Format("({0} | {1})", BitConverter.Int16BitsToHalf(A).ToString(Program.TOSTRING_NO_SCIENTIFIC), BitConverter.Int16BitsToHalf(B).ToString(Program.TOSTRING_NO_SCIENTIFIC));
				string s32 = string.Format("{0}", (int)((ushort)A | (uint)B << 16));
				string u32 = string.Format("0x{0:X8}", (ushort)A | (uint)B << 16);

				return string.Format("(S16={0} ... U16={1} ... H={2} ... S32={3} ... U32={4})", s16, u16, half, s32, u32);
			}
		}
		public struct ThirdThing
		{
			// TODO: Halfs?
			// We're missing UVs and Normals. It has to be these...
			public short A;
			public short B;

			public static HashSet<short> _aVals = new();
			public static HashSet<short> _bVals = new();

			public ThirdThing(EndianBinaryReader r)
			{
				A = r.ReadInt16();
				B = r.ReadInt16();

				_aVals.Add(A);
				_bVals.Add(B);
			}

			public static void Test()
			{
				Console.WriteLine("3Thing A values: [{0}]", string.Join(", ", _aVals.OrderDescending()));
				Console.WriteLine("3Thing B values: [{0}]", string.Join(", ", _bVals.OrderDescending()));
			}

			public override readonly string ToString()
			{
				string s16 = string.Format("({0} | {1})", A, B);
				string u16 = string.Format("(0x{0:X4} | 0x{1:X4})", (ushort)A, (ushort)B);
				string half = string.Format("({0} | {1})", BitConverter.Int16BitsToHalf(A).ToString(Program.TOSTRING_NO_SCIENTIFIC), BitConverter.Int16BitsToHalf(B).ToString(Program.TOSTRING_NO_SCIENTIFIC));
				string s32 = string.Format("{0}", (int)((ushort)A | (uint)B << 16));
				string u32 = string.Format("0x{0:X8}", (ushort)A | (uint)B << 16);

				return string.Format("(S16={0} ... U16={1} ... H={2} ... S32={3} ... U32={4})", s16, u16, half, s32, u32);
			}
		}

		private readonly Vertex[] _vertices;
		// Makes way more sense as s16 at least in the ones I checked...
		private readonly SecondThing[] _unknown2;
		// Looks like two u16 flags
		private readonly ThirdThing[] _unknown3;

		public readonly ChunkRef<NiTriStripsData> TriData;

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

			TriData = new ChunkRef<NiTriStripsData>(r);

			// All 5 in fwd_short.nif
			// 5 in track_endcap.nif
			// 3 in track_tunnelcap.nif
			byte unknown1 = r.ReadByte();

			int numVertices = r.ReadInt32();

			// All 1 in fwd_short.nif
			byte unknown2 = r.ReadByte();
			_ = r.ReadUInt32(); // remainingByteLen. Used to easily skip this mesh

			Console.WriteLine("Unknown mesh vars: Unknown1: {0} | Unknown2: {1}", unknown1, unknown2);

			long portionBegin = r.Stream.Position;

			_vertices = new Vertex[numVertices];
			for (int i = 0; i < _vertices.Length; i++)
			{
				_vertices[i] = new Vertex(r);
			}

			portionBegin = JumpToNextPortion(portionBegin, r.Stream.Position);
			r.Stream.Position = portionBegin;

			// UV? Convert to [0, 1] and see
			_unknown2 = new SecondThing[numVertices];
			for (int i = 0; i < _unknown2.Length; i++)
			{
				_unknown2[i] = new SecondThing(r);
			}

			portionBegin = JumpToNextPortion(portionBegin, r.Stream.Position);
			r.Stream.Position = portionBegin;

			// Normals?
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

		public void Output(StringBuilder sbOBJ, StringBuilder sbDebug, NIFFile nif)
		{
			NiTriStripsData data = TriData.Resolve(nif)!;

			sbOBJ.AppendLine("o " + data.BoundingSphere.Center);
			sbDebug.AppendLine("Center: " + data.BoundingSphere.Center);

			// Output verts
			for (int i = 0; i < _vertices.Length; i++)
			{
				ref Vertex v = ref _vertices[i];
				ref SecondThing st = ref _unknown2[i];
				ref ThirdThing tt = ref _unknown3[i];

				sbOBJ.Append("v ");
				sbOBJ.Append(v.X.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sbOBJ.Append(' ');
				sbOBJ.Append(v.Y.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sbOBJ.Append(' ');
				sbOBJ.Append(v.Z.ToString(Program.TOSTRING_NO_SCIENTIFIC));

				// Vertex colors test
				/*float uu = st.A / 8192f;
				float uv = MathF.Abs((short)st.B / -4096f);
				float dThing = v.D / 32_768f;
				sb.Append(' ');
				sb.Append(uu.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sb.Append(' ');
				sb.Append(uv.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sb.Append(' ');
				sb.Append(dThing);*/

				float stU = st.A / 8192f; // Appears to be Horizontal TexCoord if div 8192
				float ttU = tt.A / 32_768f; // Can be negative/positive
				float ttU_0To1 = ttU * 0.5f + 0.5f;
				float ttV = tt.B / 32_768f; // Can be negative/positive
				float ttV_0To1 = ttV * 0.5f + 0.5f;
				float dThing = v.D / 32_768f;
				sbOBJ.Append(' ');
				sbOBJ.Append(ttU_0To1.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sbOBJ.Append(' ');
				sbOBJ.Append(ttV_0To1.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sbOBJ.Append(' ');
				sbOBJ.Append(0);

				/*float uu = tt.A / 65_535f;
				float uv = tt.B / 65_535f;
				float dThing = v.D;
				sb.Append(' ');
				sb.Append(uu.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sb.Append(' ');
				sb.Append(uv.ToString(Program.TOSTRING_NO_SCIENTIFIC));
				sb.Append(' ');
				sb.Append(0);*/

				sbOBJ.AppendLine();

				// DEBUG

				sbDebug.Append(i);
				sbDebug.AppendLine(":");

				sbDebug.AppendLine("\tVertex " + v);
				sbDebug.AppendLine("\tUnk2 " + st);
				sbDebug.AppendLine("\tUnk3 " + tt);
			}

			// Output tris. Need to de-strip them
			int vertIdx = 1;
			foreach (ushort numVertsInThisStrip in data.StripLengths)
			{
				// Output each face as a tri. just 3 verts.
				// example:
				// "f 49 50 51 52" -> "f 49 50 51" and "f 50 51 52"
				// "f 95 96 97 98 99" -> "f 95 96 97" and "f 96 97 98" and "f 97 98 99"

				int numOutputTris = numVertsInThisStrip - 2;
				for (int tri = 0; tri < numOutputTris; tri++)
				{
					sbOBJ.Append("f ");
					for (int i = 0; i < 3; i++)
					{
						sbOBJ.Append(i + tri + vertIdx);
						sbOBJ.Append(' ');
					}
					sbOBJ.AppendLine();
				}
				vertIdx += numVertsInThisStrip;
			}
		}
	}

	private readonly Mesh[] _meshes;

	internal NiPS2GeometryStreamer(EndianBinaryReader r, int offset)
		: base(offset)
	{
		_meshes = new Mesh[r.ReadUInt32()]; // Matches the number of NiTriStripsData in the .nif

		for (int i = 0; i < _meshes.Length; i++)
		{
			_meshes[i] = new Mesh(r);
		}
	}

	public void TestOBJ(NIFFile nif)
	{
		const string DIR = @"C:\Users\Kermalis\Downloads\Output\";
		if (Directory.Exists(DIR))
		{
			Directory.Delete(DIR, true);
		}
		Directory.CreateDirectory(DIR);

		Mesh.SecondThing.Test();
		Mesh.ThirdThing.Test();

		var sbOBJ = new StringBuilder();
		var sbDebug = new StringBuilder();

		for (int i = 0; i < _meshes.Length; i++)
		{
			sbOBJ.Clear();
			sbDebug.Clear();

			_meshes[i].Output(sbOBJ, sbDebug, nif);

			File.WriteAllText(string.Format("{0}{1}.obj", DIR, i), sbOBJ.ToString());
			File.WriteAllText(string.Format("{0}{1}.txt", DIR, i), sbDebug.ToString());
		}
	}
	public void TestGLTF(NIFFile nif)
	{
		using (FileStream s = File.Create(@"C:\Users\Kermalis\Downloads\Test.gltf"))
		{
			var w = new EndianBinaryWriter(s, ascii: true);

			w.WriteChars("glTF");
			w.WriteUInt32(2); // Version 2.0
			w.WriteUInt32(0); // Length, come back to it later

			// First chunk must be JSON
			w.WriteUInt32(0); // Length, come back to it later
			w.WriteChars("JSON");
			w.WriteUInt32(0); // Binary data trailing 0x20s, TODO

			// Second chunk must be BIN unless the data is empty
			w.WriteUInt32(0); // Len, fix later
			w.WriteChars("BIN ");
			w.WriteUInt32(0); // Binary data trailing 0x00s, TODO

			/*for (int i = 0; i < _meshes.Length; i++)
			{
				sb.Clear();

				_meshes[i].Output(sb, nif);

				File.WriteAllText(string.Format("{0}{1}.obj", DIR, i), sb.ToString());
			}*/
		}
	}
}