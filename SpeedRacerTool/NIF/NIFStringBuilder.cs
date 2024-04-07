using Kermalis.SpeedRacerTool.NIF.NiMain;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

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

	public void WriteChunkPtr(string name, NiObject o)
	{
		AppendName(name);
		o.DebugID(this);
	}

	public void WriteTODO(string name)
	{
		_sb.Append(_curIndentChars);
		_sb.Append("TODO ");
		_sb.AppendLine(name);
	}

	public void AppendLine(TexCoord val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');
		_sb.Append(val.U.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.V.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f)");
	}
	public void AppendLine(string name, TexCoord val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}

	public void AppendLine(Matrix2x2 val, bool indent = true)
	{
		if (indent)
		{
			_sb.Append(_curIndentChars);
		}
		_sb.Append('(');

		_sb.Append(val.A.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.B.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.AppendLine("f,");

		_sb.Append(val.C.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append("f, ");
		_sb.Append(val.D.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));

		_sb.AppendLine("f)");
	}
	public void AppendLine(string name, Matrix2x2 val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
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
	public void AppendLine(string name, Matrix3x3 val)
	{
		AppendName(name);
		AppendLine(val, indent: false);
	}
}