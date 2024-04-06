using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Holds mesh data using strips of triangles.</summary>
internal sealed class NiTriStripsData : NiTriBasedGeomData
{
	public readonly ushort[] StripLengths;
	public readonly ushort[][]? Points;

	internal NiTriStripsData(EndianBinaryReader r, int index, int offset, uint userVersion)
		: base(r, index, offset)
	{
		ushort numStrips = r.ReadUInt16();

		StripLengths = new ushort[numStrips];
		r.ReadUInt16s(StripLengths);

		// Only PS2 version has this..?
		if (userVersion != 0)
		{
			if (r.ReadSafeBoolean())
			{
				Points = new ushort[numStrips][];
				for (int i = 0; i < Points.Length; i++)
				{
					Points[i] = new ushort[StripLengths[i]];
					r.ReadUInt16s(Points[i]);
				}
			}
		}
	}

	/*internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(NiTriStripsData), string.Format("NumVerts={0} | NumTris={1} | NumStrips={2}",
			NumVerts, NumTris, StripLengths.Length));
	}*/

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiTriStripsData));
	}
}