using Kermalis.EndianBinaryIO;
using System;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal sealed class QTransform
{
	// This behavior can be observed in "ps2_ps2\ui\models\321go.nif" for example
	private const int NULL_HEX = unchecked((int)0xFF7FFFFF);
	private static readonly float _nullVal = BitConverter.Int32BitsToSingle(NULL_HEX);

	public readonly Vector3 Translation;
	public readonly Quaternion Rotation;
	public readonly float Scale;

	public bool IsTranslationNull => Translation == new Vector3(_nullVal, _nullVal, _nullVal);
	public bool IsRotationNull => Rotation == new Quaternion(_nullVal, _nullVal, _nullVal, _nullVal);
	public bool IsScaleNull => Scale == _nullVal;

	public QTransform(EndianBinaryReader r)
	{
		Translation = r.ReadVector3();
		Rotation = r.ReadQuaternion();
		Scale = r.ReadSingle();
	}

	public void DebugStr(NIFStringBuilder sb, string name)
	{
		sb.NewObject(name, nameof(QTransform));

		if (IsTranslationNull)
		{
			sb.AppendLine_Null(nameof(Translation));
		}
		else
		{
			sb.AppendLine(nameof(Translation), Translation);
		}

		if (IsRotationNull)
		{
			sb.AppendLine_Null(nameof(Rotation));
		}
		else
		{
			sb.AppendLine(nameof(Rotation), Rotation);
		}

		if (IsScaleNull)
		{
			sb.AppendLine_Null(nameof(Scale));
		}
		else
		{
			sb.AppendLine(nameof(Scale), Scale);
		}

		sb.EndObject();
	}
}