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
		/// <summary>ChimChim's value is cut off lol</summary>
		public string VehicleSettingsXML;
		public byte Unk1;
		public byte Unk2;
		/// <summary>The first 4 values seem to be car stats</summary>
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
			SRAssert.Equal(r.ReadUInt16(), 0x0000);

			Unk3 = new float[7];
			xds.ReadFileSingles(r, Unk3);

			SRAssert.Equal(xds.ReadFileSingle(r), 1f);

			SRAssert.Equal(r.ReadUInt16(), 0x0000);
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine(nameof(CarID), CarID);
			sb.AppendLine(nameof(NameWithNickname), NameWithNickname);
			sb.AppendLine(nameof(Name), Name);
			sb.AppendLine(nameof(Employer), Employer);
			sb.AppendLine(nameof(CarName), CarName);
			sb.AppendLine(nameof(UIFilename), UIFilename);
			sb.AppendLine(nameof(CarFilename), CarFilename);
			sb.AppendLine(nameof(VehicleSettingsXML), VehicleSettingsXML);

			sb.AppendLine(nameof(Unk1), Unk1);
			sb.AppendLine(nameof(Unk2), Unk2);

			sb.NewArray(Unk3.Length);
			for (int i = 0; i < Unk3.Length; i++)
			{
				sb.Append_ArrayElement(i);
				sb.AppendLine(Unk3[i], indent: false);
			}
			sb.EndArray();

			sb.EndObject();
		}

		public override string ToString()
		{
			return NameWithNickname;
		}
	}

	public Magic_OneAyyArray Magic_Entries;
	public string Timestamp;

	// Node data
	public OneAyyArray<Entry> Entries;

	internal VehicleRegistryChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0106);
		SRAssert.Equal(NumNodes, 0x0001);

		Magic_Entries = new Magic_OneAyyArray(r, xds);

		Timestamp = r.ReadString_Count_TrimNullTerminators(0x20);

		// NODE START
		XDSFile.ReadNodeStart(r);

		Entries = new OneAyyArray<Entry>(r);
		Entries.AssertMatch(Magic_Entries);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i] = new Entry(r, xds);
		}

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine(nameof(Timestamp), Timestamp);

		sb.NewNode();

		sb.NewArray(Entries.Values.Length);
		for (int i = 0; i < Entries.Values.Length; i++)
		{
			Entries.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndNode();
	}
}