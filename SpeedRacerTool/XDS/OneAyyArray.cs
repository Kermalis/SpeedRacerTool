using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal struct OneAyyArray<T>
{
	public T[] Values;

	internal OneAyyArray(EndianBinaryReader r)
	{
		XDSFile.AssertValue(r.ReadUInt16(), 0x001A);
		XDSFile.AssertValue(r.ReadUInt16(), 0x0002);

		Values = new T[r.ReadUInt16()];
	}

	public override readonly string ToString()
	{
		return $"{typeof(T)}[{Values.Length}]";
	}
}