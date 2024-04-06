using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class ShaderTexDesc
{
	public readonly TexDesc Tex;
	public readonly uint MapIndex;

	private ShaderTexDesc(EndianBinaryReader r)
	{
		Tex = new TexDesc(r);
		MapIndex = r.ReadUInt32();
	}

	public static ShaderTexDesc? Read(EndianBinaryReader r)
	{
		return r.ReadSafeBoolean() ? new ShaderTexDesc(r) : null;
	}

	public static void DebugStr(NIFFile nif, NIFStringBuilder sb, int index, ShaderTexDesc? t)
	{
		sb.Append_ArrayElement(index);
		if (t is null)
		{
			sb.AppendLine_Null();
		}
		else
		{
			sb.AppendLine_NoQuotes(nameof(ShaderTexDesc));
			sb.NewObject();

			t.DebugStr(nif, sb);

			sb.EndObject();
		}
	}
	private void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(MapIndex), MapIndex);

		TexDesc.DebugStr(nif, sb, nameof(Tex), Tex);
	}
}