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
			case SRGbRangeLODData.NAME:
				c = new SRGbRangeLODData(r, ofs, chunkSize); break;
			case SRTrackGeometrySaveExtraData.NAME:
				c = new SRTrackGeometrySaveExtraData(r, ofs, chunkSize); break;
			case SRTrackPieceSaveData.NAME:
				c = new SRTrackPieceSaveData(r, ofs, chunkSize); break;

			case NiNode.NAME:
				c = new NiNode(r, ofs); break;
			case NiPS2GeometryStreamer.NAME:
				c = new NiPS2GeometryStreamer(r, ofs); break;
			case NiStringExtraData.NAME:
				c = new NiStringExtraData(r, ofs, chunkSize); break;
			case NiTriShapeData.NAME:
				c = new NiTriShapeData(r, ofs); break;
			case NiTriStrips.NAME:
				c = new NiTriStrips(r, ofs); break;
			case NiTriStripsData.NAME:
				c = new NiTriStripsData(r, ofs, userVersion); break;
			case NiZBufferProperty.NAME:
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