using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.XDS;

internal sealed class SpeechStringsChunk : XDSChunk
{
	public sealed class Entry
	{
		public OneBeeString TextID;
		public OneBeeString Text;

		internal Entry(EndianBinaryReader r)
		{
			XDSFile.AssertValue(r.ReadUInt16(), 0x0009);

			TextID = new OneBeeString(r);
			Text = new OneBeeString(r);

			XDSFile.AssertValue(r.ReadUInt16(), 0x001C);
		}

		public override string ToString()
		{
			return $"[{TextID}] = {Text}";
		}
	}

	public MagicValue[] Magics;
	public Entry[] Entries;

	internal SpeechStringsChunk(EndianBinaryReader r, XDSFile xds)
	{
		XDSFile.AssertValue(xds.Unk24, 0x06);

		Magics = new MagicValue[xds.Unk26 * 2];
		MagicValue.ReadArray(r, Magics);

		Entries = new Entry[xds.Unk26];

		for (int i = 0; i < Entries.Length; i++)
		{
			Entries[i] = new Entry(r);
		}

		XDSFile.AssertValue(r.ReadUInt16(), 0x0000);
	}

	// track_registry.xds - track data
	//  0x00-0x0F = Header
	//   fileType = 0x51C55993
	//  0x10-0x25 = MabStream header
	//   len = 0xD2 in PS2, 0xB0 in WII
	//   Unk24 = 0x06
	//   Unk26 = amount of tracks. 6 in PS2, 5 in WII. This corresponds with the amount of nodes below.
	//  0x28 = 2 [magic1] values per track. So 12 in PS2, 10 in WII. This corresponds with the amount of [OneBeeString] below.
	//  [numTracks array elements]
	//  {
	//   (LE)0x0009
	//   <
	//    [OneBeeString] // track name
	//    [OneBeeString] // track ID
	//    (LE)0x001C
	//   >
	//  }
	//  (LE)0x0000

	// speech_strings_american.xds - USA language character text
	//  0x00-0x0F = Header
	//   fileType = 0x91DB494E
	//  0x10-0x25 = MabStream header
	//   len = 0x20620 PS2, 0x20EC6 WII
	//   Unk24 = 0x06
	//   Unk26 = 0x073C (1,852). There are 1,852 nodes below.
	//  0x28-0x3A07 = 0xE78 (3,704) [magic1] values, 2 per the above value (like track_registry.xds). There are also 3,704 [OneBeeString] below. Interestingly, the 2nd value of each pair is the smaller value, unlike track_registry.
	//  [1,852 array elements]
	//  {
	//   (LE)0x0009
	//   <
	//    [OneBeeString] // text ID
	//    [OneBeeString] // text
	//    (LE)0x001C
	//   >
	//  }
	//  (LE)0x0000
}