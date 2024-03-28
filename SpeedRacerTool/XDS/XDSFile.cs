using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSFile
{
	public readonly Endianness Endianness;
	public uint FileType;
	public byte Unk24;
	public byte Unk25;
	public ushort Unk26;

	public readonly XDSChunk Chunk;

	public XDSFile(Stream s)
	{
		var r = new EndianBinaryReader(s, ascii: true);

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

		byte unk9 = r.ReadByte(); // Always 4
		AssertValue(unk9, 4);
		ushort unkA = r.ReadUInt16(); // Always 0x0001
		AssertValue(unkA, 0x0001);
		FileType = r.ReadUInt32(); // Seems to indicate the type of file. All t01 physics ones are 0xAB90DE70 for example, between PS2 and WII
		ushort unk10 = r.ReadUInt16(); // Always 0x0002
		AssertValue(unk10, 0x0002);
		ushort unk12 = r.ReadUInt16(); // Always 0x000A
		AssertValue(unk12, 0x000A);
		byte unk14 = r.ReadByte(); // Always 0x09
		AssertValue(unk14, 0x09);

		Span<char> mabStream = stackalloc char[9];
		r.ReadChars(mabStream);
		if (!mabStream.SequenceEqual("MabStream"))
		{
			throw new Exception();
		}

		ushort unk1E = r.ReadUInt16(); // Always 0x0100
		AssertValue(unk1E, 0x0100);

		uint fileLength = r.ReadUInt32();
		Unk24 = r.ReadByte();
		byte unk25 = r.ReadByte();
		AssertValue(unk25, 0x01); // Always 0x01

		AssertValue(fileLength, (ulong)(r.Stream.Length - r.Stream.Position));

		Unk26 = r.ReadUInt16();
		AssertValueNot(Unk26, 0x0000);
		Console.WriteLine("FileType=0x{0:X8}, Unk24=0x{1:X2}, Unk26=0x{2:X4}", FileType, Unk24, Unk26);
		Console.WriteLine();

		Chunk = ReadChunk(r);

		// TODO NOTES:
		// All files end with (LE)0x001C (LE)0x0000. Maybe the 0x1C is like an end tag in an xml, and the 0x00 is a "end file" operation
		// Opcodes seem to be 2 bytes, always LE
		// [magic1]= Some uint_LE that they are similar between files. PS2 values are 0x34XXXX or 0x42XXXX. WII values are 0x3AXXXX. Might be an allocator for OneAyyArray below

		// These come after a (LE)0x0009 (which seems to indicate "new node"). It comes shortly after a "magic1" value
		//  [OneAyyArray] = (LE)0x001A (LE)0x0002. seems to be an opcode for "ushort_LE (len)" followed by entries of variable structure and size
		//  [OneBeeString] = (LE)0x001B (LE)0x0002. Seems to be an opcode for "ushort_LE (len)" followed by ascii chars
		//  (LE)0x001C seems to end the node as stated above

		// MabStream chunk:
		//  (LE)0x0002 (LE)0x000A seems to be the opcode
		//  The following byte is the length of the str (always "MabStream")
		//  (LE)0x0100
		//  (uint_LE) = length of the data from Unk26 to the end
		//  (byte) = ??? Unk24. Seems to affect how the following data is read, but not by much. So it might be an opcode or bitflags
		//  (byte) = 0x01
		//  (ushort_LE) = ??? Unk26. Seems related to the amount of nodes in the MabStream (not nodes within nodes)


		// editor_template.xds is similar to the t01.xds in the PS2. It's interesting
		// t01.xds is the endgoal to open.
		//  Interestingly, PS2 tracks have a different filetype/Unk24 (0xE73FBE05 / 0x0131) than the WII tracks (0xF6EB4F8D / 0x012F)

		// replay_list.xds
		//  0x00-0x0F = Header
		//   fileType = 0xAA55B8C0
		//  0x10-0x25 = MabStream header
		//   len = 0xC0
		//   Unk24 = 0x06
		//   Unk26 = 0x0001, which is the amount of nodes below

		//  0x28 (uint) = amount of tracks (Uses file endianness). 5 for both... skorost is missing in PS2 version
		//  0x2C (uint_LE) = [magic1] 0x00346FD0 in PS2, 0x003A71F0 in WII

		//  0x30: (LE)0x0009
		//  <
		//   0x32: [OneAyyArray](5) // each entry is exactly 0x22 bytes
		//   {
		//     0x22 ascii chars
		//   }
		//   (LE)0x001C
		//  >
		//  (LE)0x0000

		// interesting_event_generators.xds
		//  0x00-0x0F = Header
		//   fileType = 0x1D0D4974
		//  0x10-0x25 = MabStream header
		//   len = 0x27A
		//   Unk24 = 0x08
		//   Unk26 = 0x0001

		//  0x28 (uint) = 4 (Uses file endianness, happens to be the number of event entries too)
		//  0x2C (uint_LE) = [magic1] 0x00347800 in PS2, 0x003A7A20 in WII. (Difference of 0x830 from the magic1 in replay_list.xds)

		//  0x30: (LE)0x0009
		//  <
		//   0x32: [OneAyyArray](4) // each event entry is variable length
		//   {
		//    0x40 ascii chars, or maybe 0x3C ascii chars with a (uint) at the end which happens to be 0
		//    (uint_LE)[magic1]
		//    (LE)0x0009
		//    <
		//     [OneBeeString]
		//     (LE)0x001C
		//    >
		//   }
		//   (LE)0x001C
		//  >
		//  (LE)0x0000

		// sr_default_action_map_ps2.xds (yes this PS2 file is also in the WII version and yes the WII version is different)
		//  0x00-0x0F = Header
		//   fileType = 0x5F3A7F1E
		//  0x10-0x25 = MabStream header
		//   len = 0x11A9 in PS2, 0xEAF in WII
		//   Unk24 = 0x0F
		//   Unk26 = 0x0001
		//  0x28 (uint_LE) = [magic1] 0x0034B390 in PS2, 0x003AA5E8 in WII.

		//  0x2C (uint) = num entries (Uses file endianness). 0x2A in PS2, 0x23 in WII
		//  0x30: (uint_LE) = [magic1] 0x0034B3A8 in PS2, 0x003AA600 in WII

		//  0x34: (LE)0x0009
		//  <
		//   0x36: [OneBeeString] = "DEFAULT_ACTIONS"
		//   0x4B: [OneAyyArray](0x2A in PS2, 0x23 in WII) // each entry is variable length
		//   {
		//    (uint_LE)[magic1]

		//    (uint) = 1. Uses file endianness
		//    (uint_LE)[magic1]

		//    (LE)0x0009
		//    <
		//     [OneBeeString]
		//     [OneAyyArray](1)
		//     {
		//      (uint_LE)[magic1]

		//      4 00s (didn't check all of the entries though). Probably a (uint) of file endianness
		//      (uint_LE)[magic1]

		//      (LE)0x0009
		//      <
		//       [OneBeeString] = "MabPS2Controller"
		//       [OneBeeString] = ""
		//       [OneBeeString] // "MAB_PS2_STICK_LX" for example
		//       (LE)0x001C
		//      >
		//     }
		//     (LE)0x001C
		//    >
		//   }
		//  (LE)0x001C
		//  >
		//  (LE)0x0000
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

	private XDSChunk ReadChunk(EndianBinaryReader r)
	{
		switch (FileType)
		{
			case 0x51C55993: return new SpeechStringsChunk(r, this); // track_registry.xds - These two just happen to look the same.
			case 0x9056EE72: return new VehicleRegistryChunk(r, this);
			case 0x91DB494E: return new SpeechStringsChunk(r, this);
			case 0xAB90DE70: return new PhysicsPropsChunk(r, this);
		}

		//throw new Exception($"Type not supported: 0x{FileType:X8}");
		// For testing:
		return null!;
	}
}