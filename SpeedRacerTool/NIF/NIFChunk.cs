using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using Kermalis.SpeedRacerTool.NIF.SpeedRacer;
using System;

namespace Kermalis.SpeedRacerTool.NIF;

internal abstract class NIFChunk
{
	public int Offset;

	protected NIFChunk(int offset)
	{
		Offset = offset;
	}

	internal static NIFChunk ReadChunk(EndianBinaryReader r, string chunkType, uint chunkSize, uint userVersion)
	{
		int ofs = (int)r.Stream.Position;

		// Referring to "nif.xml" in NifSkope to add these
		// Ptr is "tUpLink" type. It usually refers to things prior in the hierarchy

		NIFChunk c;

		switch (chunkType)
		{
			case nameof(SRGbGeometryGroup):
				c = new SRGbGeometryGroup(r, ofs, chunkSize); break;
			case nameof(SRGbRangeLODData):
				c = new SRGbRangeLODData(r, ofs, chunkSize); break;
			case nameof(SRTrackGeometrySaveExtraData):
				c = new SRTrackGeometrySaveExtraData(r, ofs, chunkSize); break;
			case nameof(SRTrackPieceSaveData):
				c = new SRTrackPieceSaveData(r, ofs, chunkSize); break;

			case nameof(NiAmbientLight):
				c = new NiAmbientLight(r, ofs); break;
			case nameof(NiBillboardNode):
				c = new NiBillboardNode(r, ofs); break;
			case nameof(NiCamera):
				c = new NiCamera(r, ofs); break;
			case nameof(NiDirectionalLight):
				c = new NiDirectionalLight(r, ofs); break;
			case nameof(NiLODNode):
				c = new NiLODNode(r, ofs); break;
			case nameof(NiNode):
				c = new NiNode(r, ofs); break;
			case nameof(NiParticleSystem):
				c = new NiParticleSystem(r, ofs); break;
			case nameof(NiPointLight):
				c = new NiPointLight(r, ofs); break;
			case nameof(NiPS2GeometryStreamer):
				c = new NiPS2GeometryStreamer(r, ofs); break;
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
			case nameof(NiZBufferProperty):
				c = new NiZBufferProperty(r, ofs); break;

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
		return string.Format("{0}@0x{1:X} = ({2})", name, Offset, contents);
	}

	internal virtual string DebugStr(NIFFile nif)
	{
		return ToString()!;
	}
}