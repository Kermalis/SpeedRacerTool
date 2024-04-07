namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSStringBuilder : DebugStringBuilder
{
	public readonly bool IsVerbose;

	public XDSStringBuilder(bool verbose)
	{
		IsVerbose = verbose;
	}

	public void AppendLine(string name, OneBeeString str)
	{
		AppendName(name);
		_sb.AppendLine(str.ToString()); // Will have quotes
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