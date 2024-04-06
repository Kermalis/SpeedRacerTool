using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal abstract class XDSChunk
{
	public readonly int Offset;
	/// <summary>It's still hard to figure out how this works...</summary>
	public readonly ushort OpCode;
	/// <summary>The amount of nodes in the MabStream (not nodes within nodes)</summary>
	public readonly ushort NumNodes;

	protected XDSChunk(int offset, ushort opcode, ushort numNodes)
	{
		Offset = offset;
		OpCode = opcode;
		NumNodes = numNodes;
	}

	internal static XDSChunk ReadChunk(EndianBinaryReader r, XDSFile xds, int offset, ushort opcode, ushort numNodes)
	{
		switch (xds.FileType)
		{
			case 0x1208BC7C: return new SplinesChunk(r, xds, offset, opcode, numNodes);
			case 0x1D0D4974: return new InterestingEventGeneratorsChunk(r, xds, offset, opcode, numNodes);
			case 0x51C55993: return new TrackRegistryChunk(r, offset, opcode, numNodes);
			case 0x5F3A7F1E: return new ActionMapChunk(r, xds, offset, opcode, numNodes);
			case 0x9056EE72: return new VehicleRegistryChunk(r, xds, offset, opcode, numNodes);
			case 0x91DB494E: return new SpeechStringsChunk(r, offset, opcode, numNodes);
			case 0xAA55B8C0: return new ReplayListChunk(r, xds, offset, opcode, numNodes);
			case 0xAB90DE70: return new PhysicsPropsChunk(r, xds, offset, opcode, numNodes);
			case 0xE73FBE05: return new PS2TrackChunk(r, xds, offset, opcode, numNodes);
				//case 0xF6EB4F8D: return new WIITrackChunk(r, xds, offset, opcode, numNodes);
		}
		return new XDSUnsupportedChunk(r, offset, opcode, numNodes);
	}

	internal void DebugStr(XDSStringBuilder sb, int i)
	{
		sb.Append_ArrayElement(i);
		sb.AppendLine_NoQuotes(string.Format("ChunkType={0}@0x{1:X}, OpCode=0x{2:X4}, NumNodes=0x{3:X4}", GetType().Name, Offset, OpCode, NumNodes), indent: false);
		sb.NewObject();

		DebugStr(sb);

		sb.EndObject();
	}
	protected virtual void DebugStr(XDSStringBuilder sb)
	{
		sb.AppendLine_NoQuotes(GetType().ToString());
	}
}