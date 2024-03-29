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
			XDSFile.AssertValue(r.ReadUInt16(), 0x0000);

			Unk3 = new float[7];
			xds.ReadFileSingles(r, Unk3);

			XDSFile.AssertValue(xds.ReadFileSingle(r), 1f);

			XDSFile.AssertValue(r.ReadUInt16(), 0x0000);
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.AppendLine_ArrayElement(index);
			sb.NewObject();

			sb.AppendLine_Quotes(CarID);
			sb.AppendLine_Quotes(NameWithNickname);
			sb.AppendLine_Quotes(Name);
			sb.AppendLine_Quotes(Employer);
			sb.AppendLine_Quotes(CarName);
			sb.AppendLine_Quotes(UIFilename);
			sb.AppendLine_Quotes(CarFilename);
			sb.AppendLine_Quotes(VehicleSettingsXML);

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

	public MagicValue Magic;
	public string Timestamp;

	// Node data
	public OneAyyArray<Entry> Entries;

	internal VehicleRegistryChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x0106);
		XDSFile.AssertValue(NumNodes, 0x0001);

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
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine_Quotes(Timestamp);

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