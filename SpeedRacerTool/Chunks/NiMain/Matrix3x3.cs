using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

internal struct Matrix3x3
{
	public float A;
	public float B;
	public float C;
	public float D;
	public float E;
	public float F;
	public float G;
	public float H;
	public float I;

	public Matrix3x3(EndianBinaryReader r)
	{
		A = r.ReadSingle();
		B = r.ReadSingle();
		C = r.ReadSingle();
		D = r.ReadSingle();
		E = r.ReadSingle();
		F = r.ReadSingle();
		G = r.ReadSingle();
		H = r.ReadSingle();
		I = r.ReadSingle();
	}
}