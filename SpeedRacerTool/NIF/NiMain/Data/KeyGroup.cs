using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal sealed class KeyGroup<T>
	where T : struct
{
	public readonly KeyType? Interpolation;
	public readonly Key<T>[] Keys;

	private KeyGroup(KeyType? interp, Key<T>[] keys)
	{
		Interpolation = interp;
		Keys = keys;
	}

	public static KeyGroup<float> CreateFloat(EndianBinaryReader r)
	{
		KeyType? interp;
		Key<float>[] keys;

		uint numKeys = r.ReadUInt32();

		if (numKeys == 0)
		{
			interp = null;
			keys = [];
		}
		else
		{
			interp = r.ReadEnum<KeyType>();

			keys = new Key<float>[numKeys];
			for (int i = 0; i < keys.Length; i++)
			{
				keys[i] = Key<float>.CreateFloat(r, interp.Value);
			}
		}

		return new KeyGroup<float>(interp, keys);
	}

	public void DebugStr(NIFStringBuilder sb, string name)
	{
		sb.NewObject(name, nameof(KeyGroup<T>));

		if (Interpolation is not null)
		{
			sb.AppendLine(nameof(Interpolation), Interpolation.Value.ToString());
		}

		sb.NewArray(nameof(Keys), Keys.Length);
		for (int i = 0; i < Keys.Length; i++)
		{
			Keys[i].DebugStr(sb, i);
		}
		sb.EndArray();

		sb.EndObject();
	}
}