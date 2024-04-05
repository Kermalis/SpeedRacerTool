using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal readonly struct StringIndex
{
	public readonly int Index;

	internal StringIndex(EndianBinaryReader r)
	{
		Index = r.ReadInt32();
		SRAssert.GreaterEqual(Index, -1);
	}

	public string? Resolve(NIFFile nif)
	{
		return Index == -1 ? null : nif.Strings[Index];
	}

	public override string ToString()
	{
		return string.Format("string ({0})", Index);
	}
}