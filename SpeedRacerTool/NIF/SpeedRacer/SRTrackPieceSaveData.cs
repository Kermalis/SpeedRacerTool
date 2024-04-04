using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

internal sealed class SRTrackPieceSaveData : NIFChunk
{
	public readonly float[] Data;

	internal SRTrackPieceSaveData(EndianBinaryReader r, int offset, uint size)
		: base(offset)
	{
		if (size != 8)
		{
			throw new Exception();
		}

		Data = new float[2];
		r.ReadSingles(Data);
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(SRTrackPieceSaveData), string.Format("{0} | {1}", Data[0], Data[1]));
	}
}