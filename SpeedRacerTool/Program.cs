using Kermalis.SpeedRacerTool.Chunks;
using Kermalis.SpeedRacerTool.Chunks.NiMain;
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
			var nif = new NIF(s);

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