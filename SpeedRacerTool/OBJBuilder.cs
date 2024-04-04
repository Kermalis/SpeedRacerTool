using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace Kermalis.SpeedRacerTool;

/// <summary>Two ways to use this:
/// 1 [Mesh] - Name object (optional), add vertices (optionally with colors), add faces.
/// 2 [Points] - Name object (optional), add vertices.</summary>
internal sealed class OBJBuilder
{
	private readonly StringBuilder _sb;

	public OBJBuilder()
	{
		_sb = new StringBuilder();
	}

	public void Clear()
	{
		_sb.Clear();
	}

	public void AddObject(string name)
	{
		_sb.Append("o ");
		_sb.AppendLine(name);
	}

	public void AddVertex(Vector3 pos)
	{
		_sb.Append("v ");
		_sb.Append(pos.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.Append(pos.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.AppendLine(pos.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
	}
	/// <summary>Not a part of the .obj standard. Blender won't load colors unless all 3 components are [0, 1]</summary>
	public void AddVertex(Vector3 pos, Vector3 color)
	{
		if ((color.X is < 0 or > 1)
			|| (color.Y is < 0 or > 1)
			|| (color.Z is < 0 or > 1))
		{
			throw new ArgumentOutOfRangeException(nameof(color), color, null);
		}
		_sb.Append("v ");
		_sb.Append(pos.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.Append(pos.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.Append(pos.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.Append(color.X.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.Append(color.Y.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
		_sb.Append(' ');
		_sb.AppendLine(color.Z.ToString(SRUtils.TOSTRING_NO_SCIENTIFIC));
	}

	public void AddFace(int vertID1, int vertID2, int vertID3)
	{
		_sb.Append("f ");
		_sb.Append(vertID1);
		_sb.Append(' ');
		_sb.Append(vertID2);
		_sb.Append(' ');
		_sb.Append(vertID3);
		_sb.AppendLine();
	}

	public void Write(string path)
	{
		File.WriteAllText(path, _sb.ToString());
	}
}