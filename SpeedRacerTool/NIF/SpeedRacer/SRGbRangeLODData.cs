using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRGbRangeLODData : NiObject
{
	public readonly float[] Data;

	internal SRGbRangeLODData(EndianBinaryReader r, int index, int offset, uint size)
		: base(index, offset)
	{
		if (size != 28)
		{
			throw new Exception();
		}

		Data = new float[7];
		r.ReadSingles(Data);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.NewArray(nameof(Data), Data.Length);
		for (int i = 0; i < Data.Length; i++)
		{
			sb.AppendLine_ArrayElement(i);
			sb.AppendLine(Data[i], indent: false);
		}
		sb.EndArray();
	}
}