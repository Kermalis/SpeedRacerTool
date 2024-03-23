using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.Chunks.NiMain;
using Kermalis.SpeedRacerTool.Chunks.SpeedRacer;
using System;

namespace Kermalis.SpeedRacerTool.Chunks;

internal abstract class Chunk
{
	public int Offset;

	protected Chunk(int offset)
	{
		Offset = offset;
	}

	public static Chunk ReadChunk(EndianBinaryReader r, string chunkType, uint chunkSize, uint userVersion)
	{
		int ofs = (int)r.Stream.Position;

		// Referring to "nif.xml" in NifSkope to add these
		// Ptr is "tUpLink" type. It usually refers to things prior in the hierarchy

		Chunk c;

		switch (chunkType)
		{
			case SRTrackPieceSaveData.NAME:
				c = new SRTrackPieceSaveData(r, ofs, chunkSize); break;
			case SRTrackGeometrySaveExtraData.NAME:
				c = new SRTrackGeometrySaveExtraData(r, ofs, chunkSize); break;
			case SRGbRangeLODData.NAME:
				c = new SRGbRangeLODData(r, ofs, chunkSize); break;
			case NiPS2GeometryStreamer.NAME:
				c = new NiPS2GeometryStreamer(r, ofs); break;

			case NiNode.NAME:
				c = new NiNode(r, ofs); break;
			case NiStringExtraData.NAME:
				c = new NiStringExtraData(r, ofs, chunkSize); break;
			case NiZBufferProperty.NAME:
				c = new NiZBufferProperty(r, ofs); break;
			case NiTriStrips.NAME:
				c = new NiTriStrips(r, ofs); break;
			case NiTriStripsData.NAME:
				c = new NiTriStripsData(r, ofs, userVersion); break;

			default:
				c = new UnknownChunk(r, ofs, chunkType, chunkSize); break;
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