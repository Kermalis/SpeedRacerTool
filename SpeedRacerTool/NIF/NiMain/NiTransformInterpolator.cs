using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTransformInterpolator : NiKeyBasedInterpolator
{
	public readonly Vector3 Translation;
	public readonly Quaternion Rotation;
	public readonly float Scale;
	public readonly ChunkRef<NIFUnknownChunk> Data; // TODO: NiTransformData

	public NiTransformInterpolator(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Translation = r.ReadVector3();
		Rotation = r.ReadQuaternion();
		Scale = r.ReadSingle();
		Data = new ChunkRef<NIFUnknownChunk>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Translation), Translation);
		sb.AppendLine(nameof(Rotation), Rotation);
		sb.AppendLine(nameof(Scale), Scale);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}