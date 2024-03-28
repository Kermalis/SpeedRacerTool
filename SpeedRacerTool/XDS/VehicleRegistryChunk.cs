using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class VehicleRegistryChunk : XDSChunk
{
	public sealed class Entry
	{
		public string CarID;
		public string NameWithNickname;
		public string Name;
		public string Employer;
		public string CarName;
		public string UIFilename;
		public string CarFilename;
		public string VehicleSettingsXML;
		public byte Unk1;
		public byte Unk2;
		public float[] Unk3;

		internal Entry(EndianBinaryReader r, XDSFile xds)
		{
			CarID = r.ReadString_Count_TrimNullTerminators(0x20);
			NameWithNickname = r.ReadString_Count_TrimNullTerminators(0x20);
			Name = r.ReadString_Count_TrimNullTerminators(0x20);
			Employer = r.ReadString_Count_TrimNullTerminators(0x20);
			CarName = r.ReadString_Count_TrimNullTerminators(0x20);
			UIFilename = r.ReadString_Count_TrimNullTerminators(0x20);
			CarFilename = r.ReadString_Count_TrimNullTerminators(0x20);
			VehicleSettingsXML = r.ReadString_Count_TrimNullTerminators(0x20);

			Unk1 = r.ReadByte();
			Unk2 = r.ReadByte();
			XDSFile.AssertValue(r.ReadUInt16(), 0x0000);

			Unk3 = new float[8];
			xds.ReadFileSingles(r, Unk3);

			XDSFile.AssertValue(r.ReadUInt16(), 0x0000);
		}

		public override string ToString()
		{
			return NameWithNickname;
		}
	}

	public MagicValue Magic;
	public string Timestamp;

	public OneAyyArray<Entry> Entries;

	internal VehicleRegistryChunk(EndianBinaryReader r, XDSFile xds)
	{
		XDSFile.AssertValue(xds.Unk24, 0x06);
		XDSFile.AssertValue(xds.NumMabStreamNodes, 0x0001);

		uint numDrivers = xds.ReadFileUInt32(r);
		Magic = new MagicValue(r);

		Timestamp = r.ReadString_Count_TrimNullTerminators(0x20);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<Entry>(r);
		XDSFile.AssertValue((ulong)Entries.Values.Length, numDrivers);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END

		XDSFile.ReadChunkEnd(r);
	}

	// vehicle_registry.xds - driver data
	//  0x00-0x0F = Header
	//   fileType = 0x9056EE72
	//  0x10-0x25 = MabStream header
	//   len = 0x1CEC in PS2, 0x172E in WII
	//   Unk24 = 0x06
	//   NumNodes = 0x0001

	//  0x28 (uint) = amount of drivers (Uses file endianness). 25 in PS2, 20 in WII
	//  0x2C (uint_LE) = [magic1] 0x00422008 in PS2, 0x003ACD68 in WII

	//  0x30-0x4F = timestamp ascii, 00 padded
	//  0x50 (LE)0x0009
	//  <
	//   0x52 [OneAyyArray](25 in PS2, 20 in WII) // each driver entry is exactly 0x126 bytes
	//   {
	//    0x20 ascii chars - car id
	//    0x20 ascii chars - name with nickname
	//    0x20 ascii chars - name only
	//    0x20 ascii chars - employer
	//    0x20 ascii chars - car name
	//    0x20 ascii chars - ui filename
	//    0x20 ascii chars - car filename
	//    0x20 ascii chars - vehicle settings xml (cut off for chimchim)
	//    u8 - ???
	//    u8 - ???
	//    0x0000
	//    8 floats using file endianness
	//    0x0000
	//   }
	//   (LE)0x001C
	//  >
	//  (LE)0x0000
}