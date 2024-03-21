using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.Chunks.SpeedRacer;

// fwd_short.trk
internal sealed class SRTrackPieceSaveData : Chunk
{
	public const string NAME = "SRTrackPieceSaveData";

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

	internal override string DebugStr(NIF nif)
	{
		return DebugStr(NAME, string.Format("{0} | {1}", Data[0], Data[1]));
	}
}