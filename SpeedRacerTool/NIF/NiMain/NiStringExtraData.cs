using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiStringExtraData : NiExtraData
{
	public readonly StringIndex Data;

	internal NiStringExtraData(EndianBinaryReader r, int index, int offset, uint size)
		: base(r, index, offset)
	{
		if (size != 8)
		{
			throw new Exception();
		}

		Data = new StringIndex(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);
		sb.AppendLine(nameof(Data), Data.Resolve(nif));
	}
}