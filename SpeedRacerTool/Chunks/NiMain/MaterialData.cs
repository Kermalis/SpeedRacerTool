using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

internal sealed class MaterialData
{
	public readonly StringIndex[] MaterialNames;
	/// <summary>Extra data associated with the material. A value of -1 means the material is the default implementation.</summary>
	public readonly int[] MaterialExtraDatas; // TODO: Is this a ChunkRef<T>?
	public readonly int ActiveMaterial;
	public readonly bool MaterialNeedsUpdate;

	internal MaterialData(EndianBinaryReader r)
	{
		int numMats = r.ReadInt32();

		MaterialNames = new StringIndex[numMats];
		StringIndex.ReadArray(r, MaterialNames);

		MaterialExtraDatas = new int[numMats];
		r.ReadInt32s(MaterialExtraDatas);

		ActiveMaterial = r.ReadInt32();
		MaterialNeedsUpdate = r.ReadBoolean();
	}
}