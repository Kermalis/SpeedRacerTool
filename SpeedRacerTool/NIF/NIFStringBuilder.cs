using Kermalis.SpeedRacerTool.NIF.NiMain;

namespace Kermalis.SpeedRacerTool.NIF;

internal sealed class NIFStringBuilder : DebugStringBuilder
{
	public void WriteChunk(int index, NIFFile nif, NiObject? o)
	{
		Append_ArrayElement(index);
		if (o is null)
		{
			_sb.AppendLine("null");
		}
		else
		{
			o.DebugStr_Wrap(nif, this);
		}
	}
	public void WriteChunk(string name, NIFFile nif, NiObject? o)
	{
		AppendName(name);
		if (o is null)
		{
			_sb.AppendLine("null");
		}
		else
		{
			o.DebugStr_Wrap(nif, this);
		}
	}

	public void WriteTODO(string name)
	{
		_sb.Append(_curIndentChars);
		_sb.Append("TODO ");
		_sb.AppendLine(name);
	}
}