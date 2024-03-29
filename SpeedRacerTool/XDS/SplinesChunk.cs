using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class SplinesChunk : XDSChunk
{
	public struct Array1Data
	{
		public Vector2 Pos;

		internal Array1Data(EndianBinaryReader r, XDSFile xds)
		{
			Pos = xds.ReadFileVector2(r);
			XDSFile.AssertValue(r.ReadUInt32(), 0);
			XDSFile.AssertValue(r.ReadUInt16(), 0);
		}

		public override readonly string ToString()
		{
			return string.Format("({0}f, {1}f)",
				Pos.X.ToString(Program.TOSTRING_NO_SCIENTIFIC), Pos.Y.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		}
	}
	public struct Array2_3Data
	{
		public float Val;

		internal Array2_3Data(EndianBinaryReader r, XDSFile xds, bool mustBeOneIDK)
		{
			Val = xds.ReadFileSingle(r);
			if (mustBeOneIDK)
			{
				XDSFile.AssertValue(Val, 1f);
			}
			XDSFile.AssertValue(r.ReadUInt16(), 0);
		}

		public override readonly string ToString()
		{
			return Val.ToString(Program.TOSTRING_NO_SCIENTIFIC) + 'f';
		}
	}

	public string Name;
	// These two uints are related. Their sum is the length of UnkArray1/UnkArray2
	public uint Unk48;
	public uint Unk4C;
	public float Unk50;
	public MagicValue Magic_UnkArray1;
	public MagicValue Magic_UnkArray2;
	public MagicValue Magic_UnkArray3;
	public MagicValue Magic_EmptyArray;
	/// <summary>Almost always 1</summary>
	public float Unk7C;
	public string UnitOfMeasurement;

	// Node data
	public OneAyyArray<Array1Data> UnkArray1;
	public OneAyyArray<Array2_3Data> UnkArray2;
	public OneAyyArray<Array2_3Data> UnkArray3;

	internal SplinesChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		XDSFile.AssertValue(OpCode, 0x0111);
		XDSFile.AssertValue(NumNodes, 0x0001);

		Name = r.ReadString_Count_TrimNullTerminators(0x20);
		Unk48 = xds.ReadFileUInt32(r); // ???
		Unk4C = xds.ReadFileUInt32(r); // ???
		Unk50 = xds.ReadFileSingle(r); // ???
		uint unk54 = xds.ReadFileUInt32(r);
		Magic_UnkArray1 = new MagicValue(r);
		uint unk5C = xds.ReadFileUInt32(r);
		uint unk60 = xds.ReadFileUInt32(r);
		Magic_UnkArray2 = new MagicValue(r);
		uint numUnkArray3 = xds.ReadFileUInt32(r);
		Magic_UnkArray3 = new MagicValue(r);
		XDSFile.AssertValue(r.ReadUInt32(), 0);
		Magic_EmptyArray = new MagicValue(r);
		XDSFile.AssertValue(r.ReadUInt32(), 0);
		Unk7C = xds.ReadFileSingle(r);
		UnitOfMeasurement = r.ReadString_Count_TrimNullTerminators(0x10);

		// NODE START
		XDSFile.ReadNodeStart(r);

		UnkArray1 = new OneAyyArray<Array1Data>(r);
		XDSFile.AssertValue((ulong)UnkArray1.Values.Length, Unk48 + Unk4C);
		XDSFile.AssertValue((ulong)UnkArray1.Values.Length, unk54);
		XDSFile.AssertValue((ulong)UnkArray1.Values.Length, unk5C); // If these are all equal, don't need to check them again below
		XDSFile.AssertValue((ulong)UnkArray1.Values.Length, unk60);
		for (int i = 0; i < UnkArray1.Values.Length; i++)
		{
			UnkArray1.Values[i] = new Array1Data(r, xds);
		}

		UnkArray2 = new OneAyyArray<Array2_3Data>(r);
		XDSFile.AssertValue((ulong)UnkArray2.Values.Length, Unk48 + Unk4C);
		XDSFile.AssertValue((ulong)UnkArray2.Values.Length, unk54);
		for (int i = 0; i < UnkArray2.Values.Length; i++)
		{
			UnkArray2.Values[i] = new Array2_3Data(r, xds, true);
		}

		UnkArray3 = new OneAyyArray<Array2_3Data>(r);
		XDSFile.AssertValue((ulong)UnkArray3.Values.Length, Unk48 + unk54 + 1);
		XDSFile.AssertValue((ulong)UnkArray3.Values.Length, numUnkArray3);
		for (int i = 0; i < UnkArray3.Values.Length; i++)
		{
			UnkArray3.Values[i] = new Array2_3Data(r, xds, false);
		}

		var emptyArr = new OneAyyArray<object>(r);
		XDSFile.AssertValue((ulong)emptyArr.Values.Length, 0);

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine_Quotes(Name);
		sb.AppendLine(nameof(Unk48), Unk48, hex: false);
		sb.AppendLine(nameof(Unk4C), Unk4C, hex: false);
		sb.AppendLine(nameof(Unk50), Unk50);
		sb.AppendLine(nameof(Unk7C), Unk7C);
		sb.AppendLine_Quotes(UnitOfMeasurement);

		sb.NewNode();

		sb.NewArray(UnkArray1.Values.Length);
		for (int i = 0; i < UnkArray1.Values.Length; i++)
		{
			sb.Append_ArrayElement(i);
			sb.AppendLine(UnkArray1.Values[i].Pos, indent: false);
		}
		sb.EndArray();

		sb.NewArray(UnkArray2.Values.Length);
		for (int i = 0; i < UnkArray2.Values.Length; i++)
		{
			sb.Append_ArrayElement(i);
			sb.AppendLine(UnkArray2.Values[i].Val, indent: false);
		}
		sb.EndArray();

		sb.NewArray(UnkArray3.Values.Length);
		for (int i = 0; i < UnkArray3.Values.Length; i++)
		{
			sb.Append_ArrayElement(i);
			sb.AppendLine(UnkArray3.Values[i].Val, indent: false);
		}
		sb.EndArray();

		sb.EmptyArray();

		sb.EndNode();
	}
}