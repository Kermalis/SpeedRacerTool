namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSStringBuilder : DebugStringBuilder
{
	public void AppendLine(OneBeeString str)
	{
		_sb.Append(_curIndentChars);
		_sb.AppendLine(str.ToString());
	}

	public void NewNode()
	{
		AppendLine('<');
		Indent(+1);
	}
	public void EndNode()
	{
		Indent(-1);
		AppendLine('>');
	}
}