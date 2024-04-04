using Kermalis.SpeedRacerTool.NIF.NiMain;
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

	public void AppendName(string name)
	{
		_sb.Append(_curIndentChars);
		_sb.Append(name);
		_sb.Append(" = ");
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
	public void AppendLine(string name, float val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}
	public void AppendLine(string name, Matrix3x3 val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}
	public void AppendLine(string name, Vector3 val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}
	public void AppendLine(string name, string? val)
	{
		AppendName(name);
		if (val is null)
		{
			_sb.AppendLine("null");
		}
		else
		{
			AppendLine_Quotes(val, indent: false);
		}
	}
	public void AppendLine_Boolean(string name, bool val)
	{
		AppendName(name);
		AppendLine(val.ToString(), indent: false);
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
		_sb.Append(val.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
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
	public void AppendLine(Matrix3x3 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');

		_sb.Append(val.A.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.B.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.C.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f,");

		_sb.Append(val.D.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.E.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.F.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f,");

		_sb.Append(val.G.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.H.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.I.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));

		_sb.AppendLine("f)");
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

	public void EmptyArray()
	{
		AppendLine("Arr(0) = []");
	}
	public void NewArray(string name, int len)
	{
		AppendName(name);

		AppendLine($"Arr({len}) =", indent: false);
		AppendLine('[');
		Indent(+1);
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