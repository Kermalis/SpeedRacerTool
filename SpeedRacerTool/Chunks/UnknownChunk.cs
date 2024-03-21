using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks;

internal sealed class UnknownChunk : Chunk
{
	public readonly string Type;
	public readonly byte[] Data;

	internal UnknownChunk(EndianBinaryReader r, int offset, string type, uint size)
		: base(offset)
	{
		Type = type;
		Data = new byte[size];
		r.ReadBytes(Data);
	}

	public override string ToString()
	{
		return DebugStr(Type, string.Format("byte[{0}]",
			Data.Length));
	}
}
