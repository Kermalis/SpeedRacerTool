using Kermalis.EndianBinaryIO;
using System.Text;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Generic node object for grouping.</summary>
internal class NiNode : NiAVObject
{
	public readonly ChunkRef<NiAVObject>[] Children;
	public readonly ChunkRef<NiDynamicEffect>[] Effects;

	internal NiNode(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		Children = new ChunkRef<NiAVObject>[r.ReadUInt32()];
		ChunkRef<NiAVObject>.ReadArray(r, Children);

		Effects = new ChunkRef<NiDynamicEffect>[r.ReadUInt32()];
		ChunkRef<NiDynamicEffect>.ReadArray(r, Effects);
	}

	internal override string DebugStr(NIFFile nif)
	{
		var sb = new StringBuilder();
		sb.Append(string.Format("Name=\"{0}\", Children=NiAVObject[{1}], Effects=NiDynamicEffect[{2}]",
			Name.Resolve(nif), Children.Length, Effects.Length));

		if (Children.Length > 0)
		{
			sb.AppendLine();
			sb.AppendLine("[");

			foreach (ChunkRef<NiAVObject> c in Children)
			{
				// Yes it can have null...
				NiAVObject? cc = c.Resolve(nif);
				if (cc is not null)
				{
					sb.AppendLine(string.Format("\t{0}@0x{1:X} \"{2}\"", cc.GetType().Name, cc.NIFOffset, cc.Name.Resolve(nif)));
				}
				else
				{
					sb.AppendLine("\tnull");
				}
			}

			sb.Append(']');
		}

		return DebugStr(GetType().Name, sb.ToString());
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		foreach (ChunkRef<NiAVObject> r in Children)
		{
			r.Resolve(nif).SetParentAndChildren(nif, this);
		}
		foreach (ChunkRef<NiDynamicEffect> r in Effects)
		{
			r.Resolve(nif).SetParentAndChildren(nif, this);
		}
	}
}