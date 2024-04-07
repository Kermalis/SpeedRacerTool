using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal readonly struct NiGeometryDataFlags
{
	private readonly ushort _data;

	public ushort NumUVSets => (ushort)(_data & 0b0000_0000_0011_1111);
	public ushort UnkMat => (ushort)((_data & 0b0000_1111_1100_0000) >> 6);
	public NiNBTMethod NBTMethod => (NiNBTMethod)((_data & 0b0011_0000_0000_0000) >> 12);
	private ushort Unused => (ushort)((_data & 0b1100_0000_0000_0000) >> 14);

	public NiGeometryDataFlags(EndianBinaryReader r)
	{
		_data = r.ReadUInt16();

		SRAssert.Equal(UnkMat, 0);
		SRAssert.Equal(Unused, 0);
	}

	public void DebugStr(NIFStringBuilder sb, string name)
	{
		sb.NewObject(name, nameof(NiGeometryDataFlags));

		sb.AppendLine(nameof(NumUVSets), NumUVSets);
		sb.AppendLine(nameof(UnkMat), UnkMat);
		sb.AppendLine(nameof(NBTMethod), NBTMethod.ToString());

		sb.EndObject();
	}
}