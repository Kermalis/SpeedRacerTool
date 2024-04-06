using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiCamera : NiAVObject
{
	public readonly ushort UnkUshort1;
	public readonly float FrustumLeft;
	public readonly float FrustumRight;
	public readonly float FrustumTop;
	public readonly float FrustumBottom;
	public readonly float FrustumNear;
	public readonly float FrustumFar;
	public readonly bool IsOrtho;
	public readonly float ViewportLeft;
	public readonly float ViewportRight;
	public readonly float ViewportTop;
	public readonly float ViewportBottom;
	public readonly float LODAdjust;
	public readonly NullableChunkRef<NiObject> UnkLink;
	// These two are unknown. Changing value crashes viewer according to nifskope
	public readonly uint UnkUint1;
	public readonly uint UnkUint2;

	public NiCamera(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		UnkUshort1 = r.ReadUInt16();
		FrustumLeft = r.ReadSingle();
		FrustumRight = r.ReadSingle();
		FrustumTop = r.ReadSingle();
		FrustumBottom = r.ReadSingle();
		FrustumNear = r.ReadSingle();
		FrustumFar = r.ReadSingle();
		IsOrtho = r.ReadSafeBoolean();
		ViewportLeft = r.ReadSingle();
		ViewportRight = r.ReadSingle();
		ViewportTop = r.ReadSingle();
		ViewportBottom = r.ReadSingle();
		LODAdjust = r.ReadSingle();
		UnkLink = new NullableChunkRef<NiObject>(r);
		SRAssert.Equal(UnkLink.ChunkIndex, -1);
		UnkUint1 = r.ReadUInt32();
		UnkUint2 = r.ReadUInt32();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(UnkUshort1), UnkUshort1);
		sb.AppendLine(nameof(FrustumLeft), FrustumLeft);
		sb.AppendLine(nameof(FrustumRight), FrustumRight);
		sb.AppendLine(nameof(FrustumTop), FrustumTop);
		sb.AppendLine(nameof(FrustumBottom), FrustumBottom);
		sb.AppendLine(nameof(FrustumNear), FrustumNear);
		sb.AppendLine(nameof(FrustumFar), FrustumFar);
		sb.AppendLine_Boolean(nameof(IsOrtho), IsOrtho);
		sb.AppendLine(nameof(ViewportLeft), ViewportLeft);
		sb.AppendLine(nameof(ViewportRight), ViewportRight);
		sb.AppendLine(nameof(ViewportTop), ViewportTop);
		sb.AppendLine(nameof(ViewportBottom), ViewportBottom);
		sb.AppendLine(nameof(LODAdjust), LODAdjust);
		sb.AppendLine(nameof(UnkUint1), UnkUint1);
		sb.AppendLine(nameof(UnkUint2), UnkUint2);

		sb.WriteChunk(nameof(UnkLink), nif, UnkLink.Resolve(nif));
	}
}