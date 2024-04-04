using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiSwitchNode : NiNode
{
	public readonly ushort UnkUshort1;
	public readonly int UnkInt1;

	protected NiSwitchNode(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		UnkUshort1 = r.ReadUInt16();
		UnkInt1 = r.ReadInt32();
	}
}