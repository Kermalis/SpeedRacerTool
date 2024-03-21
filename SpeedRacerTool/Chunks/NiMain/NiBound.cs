using Kermalis.EndianBinaryIO;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

/// <summary>A sphere.</summary>
internal readonly struct NiBound
{
	public readonly Vector3 Center;
	public readonly float Radius;

	public NiBound(EndianBinaryReader r)
	{
		Center = r.ReadVector3();
		Radius = r.ReadSingle();
	}
}