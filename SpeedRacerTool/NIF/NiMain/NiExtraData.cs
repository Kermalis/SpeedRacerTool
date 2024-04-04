using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiExtraData : NiObject
{
	public readonly StringIndex Name;

	protected NiExtraData(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Name = new StringIndex(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Name), Name.Resolve(nif));
	}
}