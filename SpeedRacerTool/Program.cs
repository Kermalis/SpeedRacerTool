using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;
using System.IO;
using System.Text;

namespace Kermalis.SpeedRacerTool;

internal sealed class Program
{
	// For doubles but also works for float
	public const string TOSTRING_NO_SCIENTIFIC = "0.###################################################################################################################################################################################################################################################################################################################################################";

	private static readonly StringBuilder _sb = new();

	private static void Main()
	{
		TestISO();
		//TestNIF();
		// TODO: XDS viewer. maybe we can edit the rivals list in vehicle_registry.xds
	}

	private static void TestISO()
	{
		const string IN = @"C:\Users\Kermalis\Documents\Emulation\PS2\Games\Speed Racer - Original.iso";
		const string OUT = @"C:\Users\Kermalis\Documents\Emulation\PS2\Games\Speed Racer Modded.iso";

		const string IN_ZIP = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Mod Test\ISO Contents\DATA\PS2.ZIP";

		// In the USA release I have, PS2.ZIP exists from 0x493E0000 to 0x4D05BA38 inclusive (0x3C7BA39 length)
		const uint PS2ZIP_OFFSET = 0x493E0000;
		const uint PS2ZIP_LENGTH = 0x03C7BA39;

		File.Copy(IN, OUT, true);

		FileStream inzip = File.OpenRead(IN_ZIP);

		if (inzip.Length > PS2ZIP_LENGTH)
		{
			throw new Exception();
		}

		FileStream outFile = File.Open(OUT, FileMode.Open, FileAccess.Write, FileShare.None);
		outFile.Position = PS2ZIP_OFFSET;
		inzip.CopyTo(outFile);

		// Shouldn't get any underflow
		int numPaddingNeeded = (int)(PS2ZIP_LENGTH - inzip.Length);
		inzip.Dispose();
		if (numPaddingNeeded != 0)
		{
			var w = new EndianBinaryWriter(outFile);
			w.WriteZeroes(numPaddingNeeded);
		}
		outFile.Dispose();

		Console.WriteLine("PS2.ZIP injected with {0} bytes of padding", numPaddingNeeded);

		;
	}
	private static void TestNIF()
	{
		// Thunderhead grading (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\grading.nif";

		// Thunderhead track_tunnelcap (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01gtunn.nif";

		// Thunderhead track_endcap (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01gjcap.nif";

		// Thunderhead forward short trk data (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\geo\fwd_short.trk";

		// Thunderhead forward short trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\t01ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\geo\fwd_short.nif";

		// Aurora forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t04\t04ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t04\geo\fwd_long.nif";

		// GrandPrix forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t05\t05ttrk1_colr.dds"
		const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t05\geo\fwd_long.nif";



		// Thunderhead track_tunnelcap (WII)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\models\t01gtunn.nif";

		// Thunderhead track_endcap (WII)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\models\t01gjcap.nif";

		Console.WriteLine("Opening {0}", PATH);

		using (FileStream s = File.OpenRead(PATH))
		{
			var nif = new NIFFile(s);

			foreach (Chunk c in nif.BlockDatas)
			{
				Console.WriteLine(c.DebugStr(nif));
			}

			var testC = (NiPS2GeometryStreamer?)Array.Find(nif.BlockDatas, a => a is NiPS2GeometryStreamer);
			testC?.TestOBJ(nif);
			;
		}
	}

	public static string GetBytesString(ReadOnlySpan<byte> data)
	{
		_sb.Clear();

		for (int i = 0; i < data.Length; i++)
		{
			_sb.Append("0x");
			_sb.Append(data[i].ToString("X2"));
			if (i < data.Length - 1)
			{
				_sb.Append(", ");
			}
		}

		return _sb.ToString();
	}
}