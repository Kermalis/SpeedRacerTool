using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRGbRangeLODData : NIFChunk
{
	public readonly float[] Data;

	internal SRGbRangeLODData(EndianBinaryReader r, int offset, uint size)
		: base(offset)
	{
		if (size != 28)
		{
			throw new Exception();
		}

		Data = new float[7];
		r.ReadSingles(Data);
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(SRGbRangeLODData), string.Join(", ", Data));
	}
}