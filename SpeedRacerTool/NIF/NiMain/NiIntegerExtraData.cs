using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiIntegerExtraData : NiExtraData
{
	public readonly uint Data;

	public NiIntegerExtraData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Data = r.ReadUInt32();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);
		sb.AppendLine(nameof(Data), Data);
	}
}