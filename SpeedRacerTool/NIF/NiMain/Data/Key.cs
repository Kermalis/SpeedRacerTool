using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal sealed class Key<T>
	where T : struct
{
	public float Time { get; private set; }
	public T Value { get; private set; }

	public T? Forward { get; private set; }
	public T? Backward { get; private set; }

	public TBC? TBC { get; private set; }

	private Key()
	{
		//
	}

	public static Key<float> CreateFloat(EndianBinaryReader r, KeyType type)
	{
		var k = new Key<float>();

		k.Time = r.ReadSingle();
		k.Value = r.ReadSingle();

		if (type == KeyType.QUADRATIC_KEY)
		{
			k.Forward = r.ReadSingle();
			k.Backward = r.ReadSingle();
		}
		else if (type == KeyType.TBC_KEY)
		{
			k.TBC = new TBC(r);
		}

		return k;
	}

	public void DebugStr(NIFStringBuilder sb, int index)
	{
		sb.NewObject(index);

		sb.AppendLine(nameof(Time), Time);
		sb.AppendLine(nameof(Value), Value.ToString());

		if (Forward is not null)
		{
			sb.AppendLine(nameof(Forward), Forward.Value.ToString());
		}
		if (Backward is not null)
		{
			sb.AppendLine(nameof(Backward), Backward.Value.ToString());
		}
		if (TBC is not null)
		{
			TBC.Value.DebugStr(sb, nameof(TBC));
		}

		sb.EndObject();
	}
}