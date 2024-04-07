using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class VehicleRegistryChunk
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
			sb.NewObject(index);

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

			sb.NewArray(nameof(Unk3), Unk3.Length);
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
}