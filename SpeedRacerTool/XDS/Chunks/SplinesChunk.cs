using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS.Chunks;

internal sealed partial class SplinesChunk : XDSChunk
{
	public string Name;
	// These two uints are related. Their sum is the length of UnkArray1/UnkArray2
	public uint Unk48;
	public uint Unk4C;
	public float Unk50;
	public Magic_OneAyyArray Magic_UnkArray1;
	public Magic_OneAyyArray Magic_UnkArray2;
	public Magic_OneAyyArray Magic_UnkArray3;
	/// <summary>The magic value is present despite the length being 0</summary>
	public Magic_OneAyyArray Magic_EmptyArray;
	/// <summary>Almost always 1</summary>
	public float Unk7C;
	public string UnitOfMeasurement;

	// Node data
	public OneAyyArray<ArrayData1> UnkArray1;
	public OneAyyArray<ArrayData2_3> UnkArray2;
	public OneAyyArray<ArrayData2_3> UnkArray3;

	internal SplinesChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
		: base(offset, opcode, numNodes)
	{
		SRAssert.Equal(OpCode, 0x0111);
		SRAssert.Equal(NumNodes, 0x0001);

		Name = r.ReadString_Count_TrimNullTerminators(0x20);
		Unk48 = xds.ReadFileUInt32(r); // ???
		Unk4C = xds.ReadFileUInt32(r); // ???
		Unk50 = xds.ReadFileSingle(r); // ???
		Magic_UnkArray1 = new Magic_OneAyyArray(r, xds);
		uint unk5C = xds.ReadFileUInt32(r);
		Magic_UnkArray2 = new Magic_OneAyyArray(r, xds);
		Magic_UnkArray3 = new Magic_OneAyyArray(r, xds);
		Magic_EmptyArray = new Magic_OneAyyArray(r, xds);
		SRAssert.Equal(r.ReadUInt32(), 0);
		Unk7C = xds.ReadFileSingle(r);
		UnitOfMeasurement = r.ReadString_Count_TrimNullTerminators(0x10);

		// NODE START
		XDSFile.ReadNodeStart(r);

		UnkArray1 = new OneAyyArray<ArrayData1>(r);
		UnkArray1.AssertMatch(Magic_UnkArray1);
		SRAssert.Equal((uint)UnkArray1.Values.Length, Unk48 + Unk4C);
		SRAssert.Equal((uint)UnkArray1.Values.Length, unk5C); // If these are all equal, don't need to check them again below
		SRAssert.Equal((uint)UnkArray1.Values.Length, Magic_UnkArray2.ArrayLen);
		for (int i = 0; i < UnkArray1.Values.Length; i++)
		{
			UnkArray1.Values[i] = new ArrayData1(r, xds);
		}

		UnkArray2 = new OneAyyArray<ArrayData2_3>(r);
		UnkArray2.AssertMatch(Magic_UnkArray2);
		SRAssert.Equal((uint)UnkArray2.Values.Length, Unk48 + Unk4C);
		SRAssert.Equal((uint)UnkArray2.Values.Length, unk5C);
		for (int i = 0; i < UnkArray2.Values.Length; i++)
		{
			UnkArray2.Values[i] = new ArrayData2_3(r, xds, true);
		}

		UnkArray3 = new OneAyyArray<ArrayData2_3>(r);
		UnkArray3.AssertMatch(Magic_UnkArray3);
		SRAssert.Equal((uint)UnkArray3.Values.Length, Unk48 + unk5C + 1);
		for (int i = 0; i < UnkArray3.Values.Length; i++)
		{
			UnkArray3.Values[i] = new ArrayData2_3(r, xds, false);
		}

		OneAyyArray<object>.ReadEmpty(r);

		XDSFile.ReadNodeEnd(r);
		// NODE END
	}

	protected override void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine(nameof(Name), Name);
		sb.AppendLine(nameof(Unk48), Unk48, hex: false);
		sb.AppendLine(nameof(Unk4C), Unk4C, hex: false);
		sb.AppendLine(nameof(Unk50), Unk50);
		sb.AppendLine(nameof(Unk7C), Unk7C);
		sb.AppendLine(nameof(UnitOfMeasurement), UnitOfMeasurement);

		sb.NewNode();

		sb.NewArray(nameof(UnkArray1), UnkArray1.Values.Length);
		for (int i = 0; i < UnkArray1.Values.Length; i++)
		{
			UnkArray1.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(nameof(UnkArray2), UnkArray2.Values.Length);
		for (int i = 0; i < UnkArray2.Values.Length; i++)
		{
			UnkArray2.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.NewArray(nameof(UnkArray3), UnkArray3.Values.Length);
		for (int i = 0; i < UnkArray3.Values.Length; i++)
		{
			UnkArray3.Values[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EmptyArray();

		sb.EndNode();
	}
}