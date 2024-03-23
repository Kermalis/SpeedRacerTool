using Kermalis.EndianBinaryIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSFile
{
	public readonly Endianness Endianness;
	public readonly List<XDSChunk> Chunks;

	public XDSFile(Stream s)
	{
		var r = new EndianBinaryReader(s, ascii: true);

		Span<char> header = stackalloc char[8];
		r.ReadChars(header);
		if (!header.SequenceEqual("XDS!0303"))
		{
			throw new Exception();
		}

		char endianChar = r.ReadChar();
		switch (endianChar)
		{
			case 'b': Endianness = Endianness.BigEndian; break;
			case 'l': Endianness = Endianness.LittleEndian; break;
			default: throw new Exception();
		}

		byte unk9 = r.ReadByte(); // Always 4
		ushort unkA = r.ReadUInt16(); // Always 0x0001
		uint unkC = r.ReadUInt32(); // Seems to indicate the type of file. All t01 physics ones are 0xAB90DE70 for example, between PS2 and WII

		Chunks = [];

		// TODO NOTES:
		// All files end with (LE)0x001C (LE)0x0000. Maybe it's an opcode within MabStream to stop
		// Opcodes seem to be 2 bytes (maybe 4, but doubtful). Always LE. So the above one seems to be "end"

		// MabStream chunk:
		//  (LE)0x0002 (LE)0x000A seems to be the opcode
		//  The following byte is the length of the str (always "MabStream")
		//  (LE)0x0100
		//  (uint_LE) = length of the data (starts counting after the next 2 bytes)
		//  (ushort_LE) = ??? Unk1

		// track_registry.xds - track data
		//  0x00-0x0F = Header
		//   0x10-0x25 = MabStream header
		//    len = 0xD2 in PS2, 0xB0 in WII
		//    Unk1 = 0x0106
		//   0x26 (ushort_LE) = amount of tracks. 6 in PS2, 5 in WII
		//   0x28 = 2 uint_LE per track. So 12 in PS2, 10 in WII
		//   Next: (LE)0x0009
		//    Next: (LE)0x001B (LE)0x0002 to indicate track name, then ushort_LE for name len
		//    Then: (LE)0x001B (LE)0x0002 to indicate track ID, then ushort_LE for id len.
		//    If there's another track after, we have a (LE)0x001C (LE)0x0009, then repeat the above
		// End of file as usual in the first notes I wrote

		// vehicle_registry.xds - character data
		// Note: each string in the entry is 0x20 bytes, even if it that's too short for the string (chim chim is cut off)
		//  0x00-0x0F = Header
		//   0x10-0x25 = MabStream header
		//    len = 0x0x1CEC in PS2, 0x172E in WII
		//    Unk1 = 0x0106
		//   0x26 (ushort_LE) = 0x0001
		//   0x28 (uint) = amount of characters. Uses file endianness. 25 in PS2, 20 in WII
		//   0x2C (uint_LE) = ???. Similar to the pairs of uint_LE in track_registry.xds starting at 0x28
		//   0x30-0x4F = timestamp ascii, 00 padded
		//   0x50 (ushort_LE) = 0x0009
		//   0x52 (ushort_LE) = 0x001A
		//   0x54 (ushort_LE) = 0x0002
		//   0x56 (ushort_LE) = amount of characters. 25 in PS2, 20 in WII
		//   0x58: each entry is exactly 0x126 bytes
		//    0x20 ascii - car id
		//    0x20 ascii - name with nickname
		//    0x20 ascii - name only
		//    0x20 ascii - employer
		//    0x20 ascii - car name
		//    0x20 ascii - ui filename
		//    0x20 ascii - car filename
		//    0x20 ascii - vehicle settings xml (cut off for chimchim)
		//    u8 - ???
		//    u8 - ???
		//    0x0000
		//    8 floats using file endianness
		// End of file as usual in the first notes I wrote
	}
}