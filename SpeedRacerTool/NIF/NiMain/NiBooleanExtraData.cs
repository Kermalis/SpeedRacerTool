using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBooleanExtraData : NiExtraData
{
	public readonly bool Data;

	public NiBooleanExtraData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Data = r.ReadSafeBoolean();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);
		sb.AppendLine_Boolean(nameof(Data), Data);
	}
}