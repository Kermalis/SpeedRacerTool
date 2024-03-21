using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.Chunks.SpeedRacer;

// fwd_short.trk
internal sealed class SRGbRangeLODData : Chunk
{
	public const string NAME = "SRGbRangeLODData";

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

	internal override string DebugStr(NIF nif)
	{
		return DebugStr(NAME, string.Join(", ", Data));
	}
}