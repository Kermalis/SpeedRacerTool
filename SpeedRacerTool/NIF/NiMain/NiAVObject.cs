using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Abstract audio-visual base class from which all of Gamebryo's scene graph objects inherit.</summary>
internal abstract class NiAVObject : NiObjectNET
{
	public readonly ushort Flags;
	public readonly Vector3 Translation;
	public readonly Matrix3x3 Rotation;
	public readonly float Scale;
	public readonly ChunkRef<NiProperty>[] Properties;
	public readonly NullableChunkRef<NIFUnknownChunk> CollisionObject; // TODO: NullableChunkRef<NiCollisionObject>

	protected NiAVObject(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadUInt16();
		Translation = r.ReadVector3();
		Rotation = new Matrix3x3(r);
		Scale = r.ReadSingle();

		Properties = new ChunkRef<NiProperty>[r.ReadInt32()];
		r.ReadArray(Properties);

		CollisionObject = new NullableChunkRef<NIFUnknownChunk>(r);

		SRAssert.Equal(CollisionObject.ChunkIndex, -1);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		foreach (ChunkRef<NiProperty> r in Properties)
		{
			r.Resolve(nif).SetParentAndChildren(nif, this);
		}
		CollisionObject.Resolve(nif)?.SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags);
		sb.AppendLine(nameof(Translation), Translation);
		sb.AppendLine(nameof(Rotation), Rotation);
		sb.AppendLine(nameof(Scale), Scale);

		sb.WriteChunk(nameof(CollisionObject), nif, CollisionObject.Resolve(nif));

		sb.NewArray(nameof(Properties), Properties.Length);
		for (int i = 0; i < Properties.Length; i++)
		{
			sb.WriteChunk(i, nif, Properties[i].Resolve(nif));
		}
		sb.EndArray();
	}
}