using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.Chunks.SpeedRacer;

// fwd_short.trk
internal sealed class SRTrackGeometrySaveExtraData : Chunk
{
	public const string NAME = "SRTrackGeometrySaveExtraData";

	public readonly float[] Floats;
	public readonly byte Byte; // Always [1,3] in fwd_short.trk. Haven't checked others

	internal SRTrackGeometrySaveExtraData(EndianBinaryReader r, int offset, uint size)
		: base(offset)
	{
		if (size != 21)
		{
			throw new Exception();
		}

		Floats = new float[5];
		r.ReadSingles(Floats);
		Byte = r.ReadByte();
	}

	internal override string DebugStr(NIF nif)
	{
		return DebugStr(NAME, string.Join(", ", Floats) + " | 0x" + Byte.ToString("X2"));
	}
}
