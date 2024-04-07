using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

partial class PS2TrackChunk
{
	partial class Piece
	{
		public sealed class ArrData2
		{
			public uint UnkUint1;
			/// <summary>Each entry in the array has an increased value. [0, 1]</summary>
			public float Unk4;
			public string UnkStr1;
			public string UnkStr2;
			public float UnkFloat1;
			public uint UnkUint2;
			public float[] Data;

			internal ArrData2(EndianBinaryReader r, XDSFile xds)
			{
				UnkUint1 = r.ReadUInt32();
				Unk4 = xds.ReadFileSingle(r);
				UnkStr1 = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkStr2 = r.ReadString_Count_TrimNullTerminators(0x20);
				UnkFloat1 = xds.ReadFileSingle(r);
				UnkUint2 = r.ReadUInt32();

				Data = new float[6];
				xds.ReadFileSingles(r, Data);

				SRAssert.Equal(r.ReadUInt16(), 0x0000);
			}

			internal void DebugStr(XDSStringBuilder sb, int index)
			{
				sb.NewObject(index);

				sb.AppendLine(nameof(UnkUint1), UnkUint1);
				sb.AppendLine(nameof(Unk4), Unk4);
				sb.AppendLine(nameof(UnkStr1), UnkStr1);
				sb.AppendLine(nameof(UnkStr2), UnkStr2);
				sb.AppendLine(nameof(UnkFloat1), UnkFloat1);
				sb.AppendLine(nameof(UnkUint2), UnkUint2);

				sb.NewArray(nameof(Data), Data.Length);
				for (int i = 0; i < Data.Length; i++)
				{
					sb.Append_ArrayElement(i);
					sb.AppendLine(Data[i], indent: false);
				}
				sb.EndArray();

				sb.EndObject();
			}
		}
	}
}