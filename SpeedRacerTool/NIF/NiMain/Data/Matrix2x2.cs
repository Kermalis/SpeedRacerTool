using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

internal struct Matrix2x2
{
	public float A;
	public float B;
	public float C;
	public float D;

	public Matrix2x2(EndianBinaryReader r)
	{
		A = r.ReadSingle();
		B = r.ReadSingle();
		C = r.ReadSingle();
		D = r.ReadSingle();
	}
}