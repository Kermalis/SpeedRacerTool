using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRTrackPieceSaveData : NiObject
{
	// TODO
	public readonly float[] Data;

	internal SRTrackPieceSaveData(EndianBinaryReader r, int index, int offset, uint size)
		: base(index, offset)
	{
		if (size != 8)
		{
			throw new Exception();
		}

		Data = new float[2];
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