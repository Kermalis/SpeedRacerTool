using System.Numerics;
using System.Text;

namespace Kermalis.SpeedRacerTool;

internal abstract class DebugStringBuilder
{
	private const string INDENT_CHARS = "  ";

	protected readonly StringBuilder _sb;
	private byte _indentLevel;
	protected string _curIndentChars;

	protected DebugStringBuilder()
	{
		_sb = new StringBuilder();
		_indentLevel = 0;
		_curIndentChars = string.Empty;
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

	public void AppendName(string name)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		_sb.Append(" = ");
	}

	public void AppendLine_ArrayElement(int index)
	{
		_sb.Append(_curIndentChars);
		_sb.Append('[');
		_sb.Append(index);
		_sb.AppendLine("] =");
	}
	public void Append_ArrayElement(int index)
	{
		_sb.Append(_curIndentChars);
		_sb.Append('[');
		_sb.Append(index);
		_sb.Append("] = ");
	}

	/// <summary>Writes <see langword="null"/> with no indentation or quotes</summary>
	public void AppendLine_Null()
	{
		_sb.AppendLine("null");
	}
	public void AppendLine_Null(string name)
	{
		AppendName(name);
		AppendLine_Null();
	}

	public void AppendLine_Boolean(string name, bool val)
	{
		AppendName(name);
		AppendLine_NoQuotes(val.ToString(), indent: false);
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

	public void AppendLine(string name, byte val, bool hex = true)
	{
		AppendName(name);
		if (hex)
		{
			_sb.Append("0x");
			_sb.AppendLine(val.ToString("X2"));
		}
		else
		{
			_sb.AppendLine(val.ToString());
		}
	}

	public void AppendLine(string name, short val)
	{
		AppendName(name);
		_sb.AppendLine(val.ToString());
	}

	public void AppendLine(string name, ushort val, bool hex = true)
	{
		AppendName(name);
		if (hex)
		{
			_sb.Append("0x");
			_sb.AppendLine(val.ToString("X4"));
		}
		else
		{
			_sb.AppendLine(val.ToString());
		}
	}

	public void AppendLine(string name, int val)
	{
		AppendName(name);
		_sb.AppendLine(val.ToString());
	}

	public void AppendLine(string name, uint val, bool hex = true)
	{
		AppendName(name);
		if (hex)
		{
			_sb.Append("0x");
			_sb.AppendLine(val.ToString("X8"));
		}
		else
		{
			_sb.AppendLine(val.ToString());
		}
	}

	public void AppendLine(float val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append(val.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append('f');
		_sb.AppendLine();
	}
	public void AppendLine(string name, float val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}

	public void AppendLine(Vector2 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}

	public void AppendLine(Vector3 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}
	public void AppendLine(string name, Vector3 val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}

	public void AppendLine(Quaternion val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.W.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}
	public void AppendLine(string name, Quaternion val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}

	/// <summary>Write string with quotes</summary>
	public void AppendLine(string val, bool indent = true)
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
	public void AppendLine(string name, string? val)
	{
		AppendName(name);
		if (val is null)
		{
			AppendLine_Null();
		}
		else
		{
			AppendLine(val, indent: false);
		}
	}

	public void AppendLine_NoQuotes(string str, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.AppendLine(str);
	}

	public void NewObject()
	{
		AppendLine('{');
		Indent(+1);
	}
	public void NewObject(string name)
	{
		AppendName(name);

		AppendLine('{');
		Indent(+1);
	}
	public void EndObject()
	{
		Indent(-1);
		AppendLine('}');
	}

	public void EmptyArray()
	{
		AppendLine_NoQuotes("Arr(0) = []");
	}
	public void NewArray(string name, int len)
	{
		AppendName(name);

		AppendLine_NoQuotes($"Arr({len}) =", indent: false);
		AppendLine('[');
		Indent(+1);
	}
	public void NewArray(int len)
	{
		AppendLine_NoQuotes($"Arr({len}) =");
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