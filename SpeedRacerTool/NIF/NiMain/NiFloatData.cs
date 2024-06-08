using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiFloatData : NiObject
{
	public readonly KeyGroup<float> Data;

	public NiFloatData(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Data = KeyGroup<float>.CreateFloat(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		Data.DebugStr(sb, nameof(Data));
	}
}