using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

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
		if (t is null)
		{
			sb.Append_ArrayElement(index);
			sb.AppendLine_Null();
		}
		else
		{
			sb.NewObject(index, nameof(ShaderTexDesc));

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