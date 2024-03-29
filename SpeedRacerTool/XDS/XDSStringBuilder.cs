using System.Numerics;
using System.Text;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class XDSStringBuilder
{
	private const string INDENT_CHARS = "  ";

	private readonly StringBuilder _sb;
	private byte _indentLevel;
	private string _curIndentChars;

	public XDSStringBuilder()
	{
		_sb = new StringBuilder();
		_indentLevel = 0;
		_curIndentChars = string.Empty;
	}

	public void AppendLine_ArrayElement(int i)
	{
		_sb.Append(_curIndentChars);
		_sb.AppendLine($"[{i}] =");
	}
	public void Append_ArrayElement(int i)
	{
		_sb.Append(_curIndentChars);
		_sb.Append($"[{i}] = ");
	}
	public void AppendLine(string name, byte val, bool hex = true)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		if (hex)
		{
			_sb.Append(" = 0x");
			_sb.AppendLine(val.ToString("X2"));
		}
		else
		{
			_sb.Append(" = ");
			_sb.AppendLine(val.ToString());
		}
	}
	public void AppendLine(string name, uint val, bool hex = true)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		if (hex)
		{
			_sb.Append(" = 0x");
			_sb.AppendLine(val.ToString("X8"));
		}
		else
		{
			_sb.Append(" = ");
			_sb.AppendLine(val.ToString());
		}
	}
	public void AppendLine(string name, float val)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		_sb.Append(" = ");
		AppendLine(val, indent: false);
	}
	public void AppendLine(string name, Vector3 val)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		_sb.Append(" = ");
		AppendLine(val, indent: false);
	}

	public void Indent(int change)
	{
		_curIndentChars = string.Empty;
		_indentLevel = (byte)(_indentLevel + change);
		for (int i = 0; i < _indentLevel; i++)
		{
			_curIndentChars += INDENT_CHARS;
		}
	}

	public void AppendLine(string str, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.AppendLine(str);
	}
	public void AppendLine(char c, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append(c);
		_sb.AppendLine();
	}
	public void AppendLine(float val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append(val.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.Append('f');
		_sb.AppendLine();
	}
	public void AppendLine(Vector2 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.X.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Y.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}
	public void AppendLine(Vector3 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.X.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Y.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Z.ToString(Program.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}
	public void AppendLine(OneBeeString str)
	{
		_sb.Append(_curIndentChars);
		_sb.AppendLine(str.ToString());
	}
	public void AppendLine_Quotes(string val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('"');
		_sb.Append(val);
		_sb.Append('"');
		_sb.AppendLine();
	}

	public void NewObject()
	{
		AppendLine('{');
		Indent(+1);
	}
	public void EndObject()
	{
		Indent(-1);
		AppendLine('}');
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

	public void EmptyArray()
	{
		AppendLine("Arr(0) = []");
	}
	public void NewArray(int len)
	{
		AppendLine($"Arr({len}) =");
		AppendLine('[');
		Indent(+1);
	}
	public void EndArray()
	{
		Indent(-1);
		AppendLine(']');
	}

	public override string ToString()
	{
		return _sb.ToString();
	}
}