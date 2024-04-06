using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiSwitchNode : NiNode
{
	public readonly ushort UnkUshort1;
	public readonly int UnkInt1;

	protected NiSwitchNode(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		UnkUshort1 = r.ReadUInt16();
		UnkInt1 = r.ReadInt32();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(UnkUshort1), UnkUshort1);
		sb.AppendLine(nameof(UnkInt1), UnkInt1);
	}
}