using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.SpeedRacer;
using System;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Referring to "nif.xml" in NifSkope to add these
// Speed Racer is version 20.3.0.9 (0x14030009)

/// <summary>Abstract object type.</summary>
internal abstract class NiObject
{
	/// <summary>Offset in the .nif file</summary>
	public readonly int NIFOffset;
	public NiObject? Parent;
	public int Depth;

	protected NiObject(int offset)
	{
		NIFOffset = offset;
		Depth = -1;
	}

	internal static NiObject ReadChunk(EndianBinaryReader r, string chunkType, uint chunkSize, uint userVersion)
	{
		int ofs = (int)r.Stream.Position;

		NiObject c;

		switch (chunkType)
		{
			case nameof(NiAmbientLight):
				c = new NiAmbientLight(r, ofs); break;
			case nameof(NiBillboardNode):
				c = new NiBillboardNode(r, ofs); break;
			case nameof(NiBooleanExtraData):
				c = new NiBooleanExtraData(r, ofs); break;
			case nameof(NiCamera):
				c = new NiCamera(r, ofs); break;
			case nameof(NiDirectionalLight):
				c = new NiDirectionalLight(r, ofs); break;
			case nameof(NiIntegerExtraData):
				c = new NiIntegerExtraData(r, ofs); break;
			case nameof(NiLODNode):
				c = new NiLODNode(r, ofs); break;
			case nameof(NiMaterialProperty):
				c = new NiMaterialProperty(r, ofs); break;
			case nameof(NiNode):
				c = new NiNode(r, ofs); break;
			case nameof(NiParticleSystem):
				c = new NiParticleSystem(r, ofs); break;
			case nameof(NiPixelData):
				c = new NiPixelData(r, ofs); break;
			case nameof(NiPointLight):
				c = new NiPointLight(r, ofs); break;
			case nameof(NiPS2GeometryStreamer):
				c = new NiPS2GeometryStreamer(r, ofs); break;
			case nameof(NiShadeProperty):
				c = new NiShadeProperty(r, ofs); break;
			case nameof(NiSourceTexture):
				c = new NiSourceTexture(r, ofs); break;
			case nameof(NiSpotLight):
				c = new NiSpotLight(r, ofs); break;
			case nameof(NiStringExtraData):
				c = new NiStringExtraData(r, ofs, chunkSize); break;
			case nameof(NiTextureEffect):
				c = new NiTextureEffect(r, ofs); break;
			case nameof(NiTriShape):
				c = new NiTriShape(r, ofs); break;
			case nameof(NiTriShapeData):
				c = new NiTriShapeData(r, ofs); break;
			case nameof(NiTriStrips):
				c = new NiTriStrips(r, ofs); break;
			case nameof(NiTriStripsData):
				c = new NiTriStripsData(r, ofs, userVersion); break;
			case nameof(NiVertexColorProperty):
				c = new NiVertexColorProperty(r, ofs); break;
			case nameof(NiZBufferProperty):
				c = new NiZBufferProperty(r, ofs); break;

			case nameof(SRGbGeometryGroup):
				c = new SRGbGeometryGroup(r, ofs, chunkSize); break;
			case nameof(SRGbRangeLODData):
				c = new SRGbRangeLODData(r, ofs, chunkSize); break;
			case nameof(SRTrackGeometrySaveExtraData):
				c = new SRTrackGeometrySaveExtraData(r, ofs, chunkSize); break;
			case nameof(SRTrackPieceSaveData):
				c = new SRTrackPieceSaveData(r, ofs, chunkSize); break;

			default:
				c = new NIFUnknownChunk(r, ofs, chunkType, chunkSize); break;
		}

		if (ofs + chunkSize != r.Stream.Position)
		{
			throw new Exception();
		}

		return c;
	}

	protected string DebugStr(string name, string contents)
	{
		return string.Format("{0}@0x{1:X} = ({2})", name, NIFOffset, contents);
	}

	internal virtual string DebugStr(NIFFile nif)
	{
		return ToString()!;
	}

	public virtual void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		if (Parent is not null)
		{
			throw new Exception();
		}
		Parent = parent;
		Depth = Parent is null ? 0 : Parent.Depth + 1;
	}
}