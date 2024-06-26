﻿using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBoolInterpolator : NiKeyBasedInterpolator
{
	public readonly byte Value;
	public readonly NullableChunkRef<NIFUnknownChunk> Data; // TODO: Ref<NiBoolData>

	public NiBoolInterpolator(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Value = r.ReadByte(); // I found a bunch of 2s in "ps2_ps2\tracks\t03\models\t03gfji.nif"
		SRAssert.LessEqual(Value, 2);

		Data = new NullableChunkRef<NIFUnknownChunk>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif)?.SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Value), Value);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}