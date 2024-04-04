using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;

namespace Kermalis.SpeedRacerTool.NIF.SpeedRacer;

// Probably doesn't inherit NiExtraData since the first Float would be a StringIndex, but doesn't seem to be one
internal sealed class SRTrackGeometrySaveExtraData : NiObject
{
	public readonly float[] Floats;
	public readonly byte Byte; // Always [1,3] in fwd_short.trk. Haven't checked others

	internal SRTrackGeometrySaveExtraData(EndianBinaryReader r, int index, int offset, uint size)
		: base(index, offset)
	{
		if (size != 21)
		{
			throw new Exception();
		}

		Floats = new float[5];
		r.ReadSingles(Floats);
		Byte = r.ReadByte();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.NewArray(nameof(Floats), Floats.Length);
		for (int i = 0; i < Floats.Length; i++)
		{
			sb.AppendLine_ArrayElement(i);
			sb.AppendLine(Floats[i], indent: false);
		}
		sb.EndArray();

		sb.AppendLine(nameof(Byte), Byte);
	}
}