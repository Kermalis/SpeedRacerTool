using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal sealed class TexDesc
{
	public sealed class Transform
	{
		public readonly TexCoord Translation;
		public readonly TexCoord Tiling;
		/// <summary>2D Rotation of texture image around third W axis after U and V.</summary>
		public readonly float WRotation;
		public readonly uint UnkUint1;
		public readonly TexCoord CenterOffset;

		public Transform(EndianBinaryReader r)
		{
			Translation = new TexCoord(r);
			Tiling = new TexCoord(r);
			WRotation = r.ReadSingle();
			UnkUint1 = r.ReadUInt32();
			CenterOffset = new TexCoord(r);
		}

		public static void DebugStr(NIFStringBuilder sb, string name, Transform? t)
		{
			if (t is null)
			{
				sb.AppendName(name);
				sb.AppendLine_Null();
			}
			else
			{
				sb.NewObject(name, nameof(TexDesc) + '.' + nameof(Transform));

				t.DebugStr(sb);

				sb.EndObject();
			}
		}
		private void DebugStr(NIFStringBuilder sb)
		{
			sb.AppendLine(nameof(Translation), Translation);
			sb.AppendLine(nameof(Tiling), Tiling);
			sb.AppendLine(nameof(WRotation), WRotation);
			sb.AppendLine(nameof(UnkUint1), UnkUint1);
			sb.AppendLine(nameof(CenterOffset), CenterOffset);
		}
	}

	public readonly ChunkRef<NiSourceTexture> Source;
	/// <summary>Texture mode flags; clamp and filter mode stored in upper byte with 0xYZ00 = clamp mode Y, filter mode Z.</summary>
	public readonly ushort Flags;
	// These two may be "PS2 L" and "PS2 K" from NifSkope, despite it saying it's for another version
	public readonly short UnkShort1;
	public readonly short UnkShort2;
	public readonly Transform? Tran;

	public TexDesc(EndianBinaryReader r)
	{
		Source = new ChunkRef<NiSourceTexture>(r);
		Flags = r.ReadUInt16();
		UnkShort1 = r.ReadInt16();
		UnkShort2 = r.ReadInt16();

		if (r.ReadSafeBoolean())
		{
			Tran = new Transform(r);
		}
	}

	public static void DebugStr(NIFFile nif, NIFStringBuilder sb, string name, TexDesc? t)
	{
		if (t is null)
		{
			sb.AppendName(name);
			sb.AppendLine_Null();
		}
		else
		{
			sb.NewObject(name, nameof(TexDesc));

			t.DebugStr(nif, sb);

			sb.EndObject();
		}
	}
	private void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(Flags), Flags);
		sb.AppendLine(nameof(UnkShort1), UnkShort1);
		sb.AppendLine(nameof(UnkShort2), UnkShort2);

		Transform.DebugStr(sb, nameof(Tran), Tran);

		sb.WriteChunk(nameof(Source), nif, Source.Resolve(nif));
	}
}