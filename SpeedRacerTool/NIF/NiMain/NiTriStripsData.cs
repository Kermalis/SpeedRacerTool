using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Holds mesh data using strips of triangles.</summary>
internal sealed class NiTriStripsData : NiTriBasedGeomData
{
	public const string NAME = "NiTriStripsData";

	public readonly ushort[] StripLengths;
	public readonly ushort[][] Points;

	internal NiTriStripsData(EndianBinaryReader r, int offset, uint userVersion)
		: base(r, offset)
	{
		ushort numStrips = r.ReadUInt16();

		StripLengths = new ushort[numStrips];
		r.ReadUInt16s(StripLengths);

		// Only PS2 version has this...
		if (userVersion != 0)
		{
			bool hasPoints = r.ReadBoolean();
			if (hasPoints)
			{
				Points = new ushort[numStrips][];
				for (int i = 0; i < Points.Length; i++)
				{
					Points[i] = new ushort[StripLengths[i]];
					r.ReadUInt16s(Points[i]);
				}
			}
			else
			{
				Points = [];
			}
		}
		else
		{
			Points = [];
		}
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(NAME, string.Format("NumVerts={0} | NumTris={1} | NumStrips={2}",
			NumVerts, NumTris, StripLengths.Length));
	}
}