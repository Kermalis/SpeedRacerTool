using Kermalis.EndianBinaryIO;
using System;
using System.Diagnostics;

namespace Kermalis.SpeedRacerTool.XDS;

[DebuggerDisplay("{" + nameof(Values) + "}")]
internal struct OneAyyArray<T>
{
	public T[] Values;

	/// <summary>Does not read the elements</summary>
	internal OneAyyArray(EndianBinaryReader r)
	{
		XDSFile.AssertValue(r.ReadUInt16(), 0x001A);
		XDSFile.AssertValue(r.ReadUInt16(), 0x0002);

		ushort num = r.ReadUInt16();
		Values = num == 0 ? [] : new T[num];
	}

	/// <summary>Does not write the elements</summary>
	internal readonly void Write(EndianBinaryWriter w)
	{
		if (Values.Length > ushort.MaxValue)
		{
			throw new Exception();
		}

		w.WriteUInt16(0x001A);
		w.WriteUInt16(0x0002);
		w.WriteUInt16((ushort)Values.Length);
	}

	public override readonly string ToString()
	{
		return Values.ToString()!;
	}
}