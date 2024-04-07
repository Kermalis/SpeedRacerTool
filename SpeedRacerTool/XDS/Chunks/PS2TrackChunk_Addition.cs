using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	public sealed partial class Addition
	{
		/// <summary>Values like "track_scenery_32", "track_scenery_451", "track_startline" (editor_template only)</summary>
		public string UnkStr1;
		/// <summary>Values like "track_piece_4", "track_piece_12"</summary>
		public string UnkStr2;
		public uint UnkUint1;
		/// <summary>Seems related to the placement relative to a lap's length</summary>
		public float UnkFloat1;
		/// <summary>"SPEEDUP", "STREETLIGHT", "ADVERTISING", "OBSTACLE", or "STARTLINE" (editor_template only)</summary>
		public string Type;
		/// <summary>"" (SPEEDUP), "level_light" (STREETLIGHT), "level_advertising" (ADVERTISING), "level_obstacle" (OBSTACLE), or "level_startline" (STARTLINE)</summary>
		public string UnkStr4;
		/// <summary>Values like "midinnerright", "midouterleft", "midouterright", "lowerleft", "centre"</summary>
		public string Position;
		/// <summary>Values like -0f, -1f</summary>
		public float UnkFloat2;
		/// <summary>Values like 1f, 0f</summary>
		public float UnkFloat3;
		public Magic_OneAyyArray Magic;

		// Node data
		public OneAyyArray<ArrData1> Array; // Length is always 0 or 25

		internal Addition(EndianBinaryReader r, XDSFile xds)
		{
			UnkStr1 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr2 = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkUint1 = r.ReadUInt32();
			UnkFloat1 = xds.ReadFileSingle(r);
			Type = r.ReadString_Count_TrimNullTerminators(0x20);
			UnkStr4 = r.ReadString_Count_TrimNullTerminators(0x20);
			Position = r.ReadString_Count_TrimNullTerminators(0x2C);
			SRAssert.Equal(r.ReadUInt32(), 0x80000000);
			UnkFloat2 = xds.ReadFileSingle(r);
			SRAssert.Equal(r.ReadUInt32(), 0x80000000);
			UnkFloat3 = xds.ReadFileSingle(r);
			Magic = new Magic_OneAyyArray(r, xds); // Still has value even when 0

			// NODE START
			XDSFile.ReadNodeStart(r);

			Array = new OneAyyArray<ArrData1>(r);
			Array.AssertMatch(Magic);

			switch (Type)
			{
				case "ADVERTISING":
				case "OBSTACLE":
				case "STARTLINE":
				case "STREETLIGHT":
				{
					SRAssert.Equal(Array.Values.Length, 0);
					break;
				}
				case "SPEEDUP":
				{
					SRAssert.Equal(Array.Values.Length, 25);
					break;
				}
				default: throw new Exception();
			}

			for (int i = 0; i < Array.Values.Length; i++)
			{
				Array.Values[i] = new ArrData1(r, xds);
			}

			XDSFile.ReadNodeEnd(r);
			// NODE END
		}

		internal void DebugStr(XDSStringBuilder sb, int index)
		{
			sb.NewObject(index);

			sb.AppendLine(nameof(UnkStr1), UnkStr1);
			sb.AppendLine(nameof(UnkStr2), UnkStr2);
			sb.AppendLine(nameof(UnkUint1), UnkUint1);
			sb.AppendLine(nameof(UnkFloat1), UnkFloat1);
			sb.AppendLine(nameof(Type), Type);
			sb.AppendLine(nameof(UnkStr4), UnkStr4);
			sb.AppendLine(nameof(Position), Position);
			sb.AppendLine(nameof(UnkFloat2), UnkFloat2);
			sb.AppendLine(nameof(UnkFloat3), UnkFloat3);

			sb.NewNode();

			sb.NewArray(nameof(Array), Array.Values.Length);
			if (sb.IsVerbose)
			{
				for (int i = 0; i < Array.Values.Length; i++)
				{
					Array.Values[i].DebugStr(sb, i);
				}
			}
			sb.EndArray();

			sb.EndNode();

			sb.EndObject();
		}
	}
}