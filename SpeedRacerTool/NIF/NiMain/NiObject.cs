using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.SpeedRacer;
using System;
using System.Collections.Generic;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Referring to "nif.xml" in NifSkope to add these
// Speed Racer is version 20.3.0.9 (0x14030009)

/// <summary>Abstract object type.</summary>
internal abstract class NiObject
{
	public readonly int NIFIndex;
	/// <summary>Offset in the .nif file</summary>
	public readonly int NIFOffset;

	public bool IsRoot;
	public HashSet<NiObject>? Parents;

	protected NiObject(int index, int offset)
	{
		NIFIndex = index;
		NIFOffset = offset;
	}

	internal static NiObject ReadChunk(EndianBinaryReader r, int index, NIFFile nif)
	{
		string chunkType = nif.BlockTypes[nif.BlockTypeIndices[index] & 0x7FFF];
		uint chunkSize = nif.BlockSizes[index];
		int ofs = (int)r.Stream.Position;

		NiObject c;

		switch (chunkType)
		{
			case nameof(NiAlphaProperty):
				c = new NiAlphaProperty(r, index, ofs); break;
			case nameof(NiAmbientLight):
				c = new NiAmbientLight(r, index, ofs); break;
			case nameof(NiBillboardNode):
				c = new NiBillboardNode(r, index, ofs); break;
			case nameof(NiBooleanExtraData):
				c = new NiBooleanExtraData(r, index, ofs); break;
			case nameof(NiBoolInterpolator):
				c = new NiBoolInterpolator(r, index, ofs); break;
			case nameof(NiCamera):
				c = new NiCamera(r, index, ofs); break;
			case nameof(NiDirectionalLight):
				c = new NiDirectionalLight(r, index, ofs); break;
			case nameof(NiIntegerExtraData):
				c = new NiIntegerExtraData(r, index, ofs); break;
			case nameof(NiLODNode):
				c = new NiLODNode(r, index, ofs); break;
			case nameof(NiMaterialProperty):
				c = new NiMaterialProperty(r, index, ofs); break;
			case nameof(NiNode):
				c = new NiNode(r, index, ofs); break;
			case nameof(NiParticleSystem):
				c = new NiParticleSystem(r, index, ofs); break;
			case nameof(NiPixelData):
				c = new NiPixelData(r, index, ofs); break;
			case nameof(NiPointLight):
				c = new NiPointLight(r, index, ofs); break;
			case nameof(NiPS2GeometryStreamer):
				c = new NiPS2GeometryStreamer(r, index, ofs); break;
			case nameof(NiShadeProperty):
				c = new NiShadeProperty(r, index, ofs); break;
			case nameof(NiSourceTexture):
				c = new NiSourceTexture(r, index, ofs); break;
			case nameof(NiSpecularProperty):
				c = new NiSpecularProperty(r, index, ofs); break;
			case nameof(NiSpotLight):
				c = new NiSpotLight(r, index, ofs); break;
			case nameof(NiStringExtraData):
				c = new NiStringExtraData(r, index, ofs, chunkSize); break;
			case nameof(NiTextureEffect):
				c = new NiTextureEffect(r, index, ofs); break;
			case nameof(NiTexturingProperty):
				c = new NiTexturingProperty(r, index, ofs); break;
			case nameof(NiTransformController):
				c = new NiTransformController(r, index, ofs); break;
			case nameof(NiTransformInterpolator):
				c = new NiTransformInterpolator(r, index, ofs); break;
			case nameof(NiTriShape):
				c = new NiTriShape(r, index, ofs); break;
			case nameof(NiTriShapeData):
				c = new NiTriShapeData(r, index, ofs); break;
			case nameof(NiTriStrips):
				c = new NiTriStrips(r, index, ofs); break;
			case nameof(NiTriStripsData):
				c = new NiTriStripsData(r, index, ofs, nif.UserVersion); break;
			case nameof(NiVertexColorProperty):
				c = new NiVertexColorProperty(r, index, ofs); break;
			case nameof(NiVisController):
				c = new NiVisController(r, index, ofs); break;
			case nameof(NiZBufferProperty):
				c = new NiZBufferProperty(r, index, ofs); break;

			case nameof(SRGbGeometryGroup):
				c = new SRGbGeometryGroup(r, index, ofs, chunkSize); break;
			case nameof(SRGbRangeLODData):
				c = new SRGbRangeLODData(r, index, ofs, chunkSize); break;
			case nameof(SRTrackGeometrySaveExtraData):
				c = new SRTrackGeometrySaveExtraData(r, index, ofs, chunkSize); break;
			case nameof(SRTrackPieceSaveData):
				c = new SRTrackPieceSaveData(r, index, ofs, chunkSize); break;

			default:
				c = new NIFUnknownChunk(r, index, ofs, chunkType, chunkSize); break;
		}

		if (ofs + chunkSize != r.Stream.Position)
		{
			throw new Exception();
		}

		return c;
	}

	internal void DebugID(NIFStringBuilder sb)
	{
		sb.AppendLine_NoQuotes(string.Format("\"{0} (#{1}) @ 0x{2:X}\"", GetType().Name, NIFIndex, NIFOffset), indent: false);
	}
	internal void DebugStr_Wrap(NIFFile nif, NIFStringBuilder sb)
	{
		DebugID(sb);
		sb.NewObject();

		DebugStr(nif, sb);

		sb.EndObject();
	}
	protected virtual void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(GetType().Name, ToString());
		// Uncomment this when we remove NIFUnknownChunk:
		//throw new Exception();
	}

	public virtual void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		// Some files actually use the same NiObject multiple times
		// In that case, it may behave as if the child NiObject was cloned...

		// parent is null when handling the root objects from NIFFile
		if (parent is null)
		{
			// Make sure it's handled correctly
			if (IsRoot)
			{
				return;
			}
			throw new Exception();
		}

		Parents ??= new HashSet<NiObject>(1);
		Parents.Add(parent);
	}
}