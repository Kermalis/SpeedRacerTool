using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal readonly struct TBC
{
	public readonly float Tension;
	public readonly float Bias;
	public readonly float Continuity;

	public TBC(EndianBinaryReader r)
	{
		Tension = r.ReadSingle();
		// Just checking what values these are if I find any of these. I haven't seen one yet so I'd like to debug it
		SRAssert.Equal(Tension, 0);

		Bias = r.ReadSingle();
		SRAssert.Equal(Bias, 0);

		Continuity = r.ReadSingle();
		SRAssert.Equal(Continuity, 0);
	}

	public void DebugStr(NIFStringBuilder sb, string name)
	{
		sb.NewObject(name, nameof(TBC));

		sb.AppendLine(nameof(Tension), Tension);
		sb.AppendLine(nameof(Bias), Bias);
		sb.AppendLine(nameof(Continuity), Continuity);

		sb.EndObject();
	}
}