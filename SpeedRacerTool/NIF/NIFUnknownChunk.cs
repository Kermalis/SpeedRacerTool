using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;

namespace Kermalis.SpeedRacerTool.NIF;

// Temporary chunk until all chunks are added
internal sealed class NIFUnknownChunk : NiObject
{
	public readonly string Type;
	public readonly byte[] Data;

	internal NIFUnknownChunk(EndianBinaryReader r, int index, int offset, string type, uint size)
		: base(index, offset)
	{
		Type = type;
		Data = new byte[size];
		r.ReadBytes(Data);
	}

	public override string ToString()
	{
		return string.Format("{0} => byte[{1}]",
			Type, Data.Length);
	}
}