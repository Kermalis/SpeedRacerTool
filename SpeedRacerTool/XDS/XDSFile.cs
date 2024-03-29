using Kermalis.EndianBinaryIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSFile
{
	public readonly Endianness Endianness;
	public uint FileType;
	public readonly List<XDSChunk> Chunks;

	public XDSFile(Stream s, bool throwIfNotSupported)
	{
		var r = new EndianBinaryReader(s, ascii: true);

		// Read header

		Span<char> header = stackalloc char[8];
		r.ReadChars(header);
		if (!header.SequenceEqual("XDS!0303"))
		{
			throw new InvalidDataException("Header not found");
		}

		char endianChar = r.ReadChar();
		switch (endianChar)
		{
			case 'b': Endianness = Endianness.BigEndian; break;
			case 'l': Endianness = Endianness.LittleEndian; break;
			default: throw new InvalidDataException("Invalid endianness: " + endianChar);
		}

		AssertValue(r.ReadByte(), 4); // Unk9
		AssertValue(r.ReadUInt16(), 0x0001); // UnkA

		FileType = r.ReadUInt32();

		AssertValue(r.ReadUInt16(), 0x0002); // Unk10
		AssertValue(r.ReadUInt16(), 0x000A); // Unk12
		AssertValue(r.ReadByte(), 0x09); // Unk14

		Span<char> mabStream = stackalloc char[9];
		r.ReadChars(mabStream);
		if (!mabStream.SequenceEqual("MabStream"))
		{
			throw new Exception();
		}

		AssertValue(r.ReadUInt16(), 0x0100); // Unk1E

		uint streamLength = r.ReadUInt32();
		AssertValue(streamLength, (ulong)(r.Stream.Length - r.Stream.Position) - 2);

		// Read chunks
		Chunks = new List<XDSChunk>(1);

		while (true)
		{
			int offset = (int)r.Stream.Position;

			ushort opcode = r.ReadUInt16();
			if (r.Stream.Position == r.Stream.Length)
			{
				AssertValue(opcode, 0x0000);
				break;
			}
			else
			{
				AssertValueNot(opcode, 0x0000);
			}

			AssertValue(opcode >> 8, 0x01); // OpCodes must be 0x1XX

			ushort numNodes = r.ReadUInt16();
			AssertValueNot(numNodes, 0x0000);

			var c = XDSChunk.ReadChunk(r, this, offset, opcode, numNodes);

			if (throwIfNotSupported && c is XDSUnsupportedChunk)
			{
				throw new Exception($"XDS type not yet supported: 0x{FileType:X8}");
			}

			Chunks.Add(c);
		}

		// Debug

		var sb = new XDSStringBuilder();

		sb.AppendLine(string.Format("FileType=0x{0:X8} | ({1} chunks) =", FileType, Chunks.Count));
		sb.AppendLine('[');
		sb.Indent(+1);
		for (int i = 0; i < Chunks.Count; i++)
		{
			Chunks[i].DebugStr(sb, i);
		}
		sb.Indent(-1);
		sb.AppendLine(']');

		Console.WriteLine(sb.ToString());

		// editor_template.xds is similar to the t01.xds in the PS2. It's interesting
		// t01.xds is the endgoal to open.
		//  Interestingly, PS2 tracks have a different filetype/Unk24 (0xE73FBE05 / 0x0131) than the WII tracks (0xF6EB4F8D / 0x012F)
	}

	internal static void AssertValue(ulong value, ulong expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void AssertValue(float value, float expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void AssertValue(Vector3 value, Vector3 expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void AssertValueNot(float value, float expected)
	{
		if (value == expected)
		{
			throw new Exception();
		}
	}
	internal static void AssertValueNot(Vector3 value, Vector3 expected)
	{
		if (value == expected)
		{
			throw new Exception();
		}
	}
	internal uint ReadFileUInt32(EndianBinaryReader r)
	{
		r.Endianness = Endianness;
		uint val = r.ReadUInt32();
		r.Endianness = Endianness.LittleEndian;
		return val;
	}
	internal float ReadFileSingle(EndianBinaryReader r)
	{
		r.Endianness = Endianness;
		float val = r.ReadSingle();
		r.Endianness = Endianness.LittleEndian;
		return val;
	}
	internal Vector2 ReadFileVector2(EndianBinaryReader r)
	{
		r.Endianness = Endianness;
		Vector2 val = r.ReadVector2();
		r.Endianness = Endianness.LittleEndian;
		return val;
	}
	internal Vector3 ReadFileVector3(EndianBinaryReader r)
	{
		r.Endianness = Endianness;
		Vector3 val = r.ReadVector3();
		r.Endianness = Endianness.LittleEndian;
		return val;
	}
	internal void ReadFileSingles(EndianBinaryReader r, Span<float> dest)
	{
		r.Endianness = Endianness;
		r.ReadSingles(dest);
		r.Endianness = Endianness.LittleEndian;
	}
	internal void ReadFileUInt16s(EndianBinaryReader r, Span<ushort> dest)
	{
		r.Endianness = Endianness;
		r.ReadUInt16s(dest);
		r.Endianness = Endianness.LittleEndian;
	}

	internal static void ReadNodeStart(EndianBinaryReader r)
	{
		AssertValue(r.ReadUInt16(), 0x0009);
	}
	internal static void ReadNodeEnd(EndianBinaryReader r)
	{
		AssertValue(r.ReadUInt16(), 0x001C);
	}
}