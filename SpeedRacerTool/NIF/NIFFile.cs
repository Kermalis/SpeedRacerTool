using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;
using System.IO;

namespace Kermalis.SpeedRacerTool.NIF;

internal sealed class NIFFile
{
	public readonly string VersionStr;
	public readonly uint Version;
	public readonly uint UserVersion;
	public readonly string[] BlockTypes;
	public readonly ushort[] BlockTypeIndices; // Get block type by & 0x7FFF. What's the last bit?
	public readonly uint[] BlockSizes;
	public readonly string[] Strings;
	public readonly uint[] Groups;
	public readonly NiObject[] BlockDatas;
	public readonly ChunkPtr<NiObject>[] Roots;

	public NIFFile(Stream s)
	{
		var r = new EndianBinaryReader(s, ascii: true);

		ReadHeaderString(r, out VersionStr, out Version);

		bool littleEndian = r.ReadSafeBoolean();
		// These two are always little endian:
		UserVersion = r.ReadUInt32();
		uint numBlocks = r.ReadUInt32();

		if (!littleEndian)
		{
			r.Endianness = Endianness.BigEndian;
		}

		BlockTypes = new string[r.ReadUInt16()];
		for (int i = 0; i < BlockTypes.Length; i++)
		{
			BlockTypes[i] = r.ReadString_Count(r.ReadInt32());
		}

		BlockTypeIndices = new ushort[numBlocks];
		r.ReadUInt16s(BlockTypeIndices);

		BlockSizes = new uint[numBlocks];
		r.ReadUInt32s(BlockSizes);

		uint numStrings = r.ReadUInt32();
		_ = r.ReadUInt32(); // maxStringLen, used to create a buffer to read

		Strings = new string[numStrings];
		for (int i = 0; i < Strings.Length; i++)
		{
			Strings[i] = r.ReadString_Count(r.ReadInt32());
		}

		Groups = new uint[r.ReadUInt32()];
		r.ReadUInt32s(Groups);

		BlockDatas = new NiObject[numBlocks];
		for (int i = 0; i < BlockDatas.Length; i++)
		{
			BlockDatas[i] = NiObject.ReadChunk(r, i, this);
		}

		Roots = new ChunkPtr<NiObject>[r.ReadUInt32()];
		r.ReadArray(Roots);

		HandleHierarchy();
	}

	private void HandleHierarchy()
	{
		foreach (ChunkPtr<NiObject> root in Roots)
		{
			root.Resolve(this).SetIsRoot();
		}
		foreach (ChunkPtr<NiObject> root in Roots)
		{
			root.Resolve(this).SetParentAndChildren(this, null);
		}

		// Sanity check
		foreach (NiObject o in BlockDatas)
		{
			if (o.Depth < 0)
			{
				throw new Exception();
			}
		}
	}

	private static void ReadHeaderString(EndianBinaryReader r,
		out string versionStr, out uint ver)
	{
		versionStr = string.Empty;

		int c = 0;

		while (true)
		{
			if (c++ >= 80)
			{
				throw new Exception("Header too long");
			}

			char ch = r.ReadChar();
			if (ch == '\n')
			{
				break;
			}
			versionStr += ch;
		}

		ver = r.ReadUInt32();
	}

	public void PrintHierarchy()
	{
		var sb = new NIFStringBuilder();
		sb.NewArray("NIF Roots", Roots.Length);
		for (int i = 0; i < Roots.Length; i++)
		{
			sb.WriteChunk(i, this, Roots[i].Resolve(this));
		}
		sb.EndArray();
		Console.WriteLine(sb.ToString());
	}
}