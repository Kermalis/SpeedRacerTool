using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiFloatExtraData : NiExtraData
{
	public readonly float Data;

	public NiFloatExtraData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Data = r.ReadSingle();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Data), Data);
	}
}