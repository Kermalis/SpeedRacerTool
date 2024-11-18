using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using Kermalis.SpeedRacerTool.XDS;
using Kermalis.SpeedRacerTool.XDS.Chunks;
using System;
using System.IO;
using System.Text;

namespace Kermalis.SpeedRacerTool;

internal sealed class Program
{
	private enum ProgramAction : byte
	{
		PatchISO_PS2,
		TestNIF,
		TestXDS,
		TestEveryNIF,
		TestEveryXDS,
	}

	// Change these to your paths cuz I'm too lazy for console args:

	/// <summary>Filepath for the program log</summary>
	private const string LOG_PATH = @"D:\3D Objects\SpeedRacerTool Output\Log.txt";
	/// <summary>Open your PS2 iso with 7-zip. Open "DATA/PS2.ZIP" and extract the "ps2_ps2" folder inside. Then place the path where you extracted that here</summary>
	private const string RIPPED_PS2_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\";
	/// <summary>Open your WII iso with 7-zip and extract the "wii_wiics" folder inside. Then place the path where you extracted that here</summary>
	private const string RIPPED_WII_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\";

	#region Modded PS2 ISO Paths

	/// <summary>This is the modded "PS2.ZIP" that will be placed into the modded PS2 ISO</summary>
	private const string PS2_MODDED_ZIP_FILE = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Mod Test\ISO Contents\DATA\PS2.ZIP";
	/// <summary>This is the filepath of the original PS2 ISO that will be used when creating a modded ISO</summary>
	private const string PS2_INPUT_ISO_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Games\Speed Racer - Original.iso";
	/// <summary>This is the filepath where the modded PS2 ISO will be created</summary>
	private const string PS2_OUTPUT_ISO_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Games\Speed Racer Modded.iso";

	#endregion

	/// <summary>The path where the ripped OBJ files will be placed</summary>
	private const string OUT_OBJ_PATH = @"D:\3D Objects\SpeedRacerTool Output\";
	/// <summary>Test OBJ filepath</summary>
	private const string OUT_TESTOBJ_FILE = @"D:\3D Objects\SpeedRacerTool Output\Test.obj";



	private static readonly StringBuilder _sb = new();

	private static void Main()
	{
		using (StreamWriter log = File.CreateText(LOG_PATH))
		{
			Console.SetOut(log);

			bool doCatch = false;
			ProgramAction a = ProgramAction.TestNIF;

			if (doCatch)
			{
				try
				{
					Do(a);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
			else
			{
				Do(a);
			}
		}
	}
	private static void Do(ProgramAction a)
	{
		switch (a)
		{
			case ProgramAction.PatchISO_PS2:
			{
				PatchISO_PS2(PS2_INPUT_ISO_PATH, PS2_OUTPUT_ISO_PATH, PS2_MODDED_ZIP_FILE);
				break;
			}
			case ProgramAction.TestNIF:
			{
				TestNIF(false, RIPPED_PS2_PATH);
				//TestNIF(false, RIPPED_WII_PATH);
				break;
			}
			case ProgramAction.TestXDS:
			{
				TestXDS(RIPPED_PS2_PATH);
				//TestXDS(RIPPED_WII_PATH);
				break;
			}
			case ProgramAction.TestEveryNIF:
			{
				TestEveryNIF(RIPPED_PS2_PATH, false);
				// TODO: Fix wii
				//TestEveryNIF(RIPPED_WII_PATH);
				break;
			}
			case ProgramAction.TestEveryXDS:
			{
				TestEveryXDS(RIPPED_PS2_PATH);
				TestEveryXDS(RIPPED_WII_PATH);
				break;
			}
		}
	}

	private static void TestXDS(string dir)
	{
		// track 1 heightfield physics (PS2)
		//const string PATH = @"tracks\t01\physics\t01_phx_ground_hf.xds";

		// track 5 heightfield physics (PS2)
		const string PATH = @"tracks\t05\physics\t05_phx_ground_hf.xds";

		string fullPath = Path.Combine(dir, PATH);
		Console.WriteLine("Opening {0}", fullPath);

		using (FileStream s = File.OpenRead(fullPath))
		{
			var xds = new XDSFile(s, true);

			var c = xds.Chunks[0] as PhysicsPropsChunk;
			c?.TestHeightfieldDump(OUT_TESTOBJ_FILE);

			;
		}
	}
	private static void TestEveryXDS(string dir)
	{
		foreach (string path in Directory.EnumerateFiles(dir, "*.xds", SearchOption.AllDirectories))
		{
			Console.WriteLine("Opening {0}", path);

			using (FileStream s = File.OpenRead(path))
			{
				var xds = new XDSFile(s, false);

				;
			}
		}

		;
	}
	private static void PatchISO_PS2(string originalISOFile, string newISOFile, string inZip)
	{
		// In the USA release I have, PS2.ZIP exists from 0x493E0000 to 0x4D05BA38 inclusive (0x3C7BA39 length)
		const uint PS2_ZIP_OFFSET = 0x493E0000;
		const uint PS2_ZIP_LENGTH = 0x03C7BA39;

		File.Copy(originalISOFile, newISOFile, true);

		FileStream inzip = File.OpenRead(inZip);

		if (inzip.Length > PS2_ZIP_LENGTH)
		{
			throw new Exception();
		}

		FileStream outFile = File.Open(newISOFile, FileMode.Open, FileAccess.Write, FileShare.None);
		outFile.Position = PS2_ZIP_OFFSET;
		inzip.CopyTo(outFile);

		// Shouldn't get any underflow
		int numPaddingNeeded = (int)(PS2_ZIP_LENGTH - inzip.Length);
		inzip.Dispose();
		if (numPaddingNeeded != 0)
		{
			var w = new EndianBinaryWriter(outFile);
			w.WriteZeroes(numPaddingNeeded);
		}
		outFile.Dispose();

		Console.WriteLine("PS2.ZIP injected with {0} bytes of padding", numPaddingNeeded);
	}
	private static void TestNIF(bool requireFullHierarchy, string dir)
	{
		// Uncomment something to rip it
		// I just included random things here... you can of course rip any .nif file if you just write the path here

		#region Thunderhead

		// Its texture is "[WII RIP]\tracks\t01\t01ttrk1_colr.dds"

		#region PS2 & WII

		// track_tunnelcap:
		//const string PATH = @"tracks\t01\models\t01gtunn.nif";

		// track_endcap:
		//const string PATH = @"tracks\t01\models\t01gjcap.nif";

		// props
		const string PATH = @"tracks\t01\models\t01ggs.nif";

		#endregion

		#region PS2 Only

		// grading (PS2)
		//const string PATH = @"tracks\t01\grading.nif";

		// forward short trk data (PS2)
		//const string PATH = @"tracks\t01\geo\fwd_short.trk";

		// forward long trk geo (PS2)
		//const string PATH = @"tracks\t01\geo\fwd_long.nif";

		#endregion

		#endregion

		//

		#region Onuris

		#region PS2 & WII

		// props
		//const string PATH = @"tracks\t02\models\t02gocc.nif";

		#endregion

		#endregion

		//

		#region Fuji

		// Its texture is "[WII RIP]\tracks\t03\t03ttrk1_colr.dds"

		#region PS2 & WII

		// props [need to add NiPSysData and other particle chunks]
		//const string PATH = @"tracks\t03\models\t03gfji.nif";

		#endregion

		#region PS2 Only

		// forward long trk geo (PS2)
		//const string PATH = @"tracks\t03\geo\fwd_long.nif";

		#endregion

		#endregion

		//

		#region Aurora

		// Its texture is "[WII RIP]\tracks\t04\t04ttrk1_colr.dds"

		#region PS2 & WII

		// props
		//const string PATH = @"tracks\t04\models\t04gaur.nif";

		#endregion

		#region PS2 Only

		// forward long trk geo (PS2)
		//const string PATH = @"tracks\t04\geo\fwd_long.nif";

		#endregion

		#endregion

		//

		#region Grand Prix

		// Its texture is "[WII RIP]\tracks\t05\t05ttrk1_colr.dds"

		#region PS2 & WII

		// props
		//const string PATH = @"tracks\t05\models\t05ggp.nif";

		#endregion

		#region PS2 Only

		// forward long trk geo (PS2)
		//const string PATH = @"tracks\t05\geo\fwd_long.nif";

		#endregion

		#endregion

		//

		#region Skorost (PS2 Only)

		// props (PS2)
		//const string PATH = @"tracks\t06\models\t06gsk.nif";

		// forward long trk geo (PS2)
		//const string PATH = @"tracks\t06\geo\fwd_long.nif";

		#endregion

		// PS2 TESTING
		//const string PATH = @"ui\models\321go.nif";
		//const string PATH = @"tracks\t01\models\t01ldir.nif";


		string fullPath = Path.Combine(dir, PATH);
		Console.WriteLine("Opening {0}", fullPath);

		using (FileStream s = File.OpenRead(fullPath))
		{
			var nif = new NIFFile(s, requireFullHierarchy);

			/*if (requireFullHierarchy)
			{
				nif.PrintHierarchy();
			}*/

			string objDir = GetNIFOutputDir(PATH);
			var testC = (NiPS2GeometryStreamer?)Array.Find(nif.BlockDatas, a => a is NiPS2GeometryStreamer);
			testC?.TestOBJ(nif, objDir, true);


			// TODO: -convcolonly for godot (https://www.youtube.com/watch?v=Mq-_FffB2eE)
			//testC?.TestGLTF(nif, Path.GetFileName(PATH));
		}
	}
	private static void TestEveryNIF(string dir, bool requireFullHierarchy)
	{
		foreach (string path in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
		{
			if (!path.EndsWith(".nif") && !path.EndsWith(".trk"))
			{
				continue;
			}

			Console.WriteLine("Opening {0}", path);

			using (FileStream s = File.OpenRead(path))
			{
				var nif = new NIFFile(s, requireFullHierarchy);

				if (requireFullHierarchy)
				{
					nif.PrintHierarchy();
				}

				//string objDir = GetNIFOutputDir(path);
				//var testC = (NiPS2GeometryStreamer?)Array.Find(nif.BlockDatas, a => a is NiPS2GeometryStreamer);
				//testC?.TestOBJ(nif, objDir, true);
			}

			Console.WriteLine();
		}

		;
	}

	private static string GetNIFOutputDir(string relativeNIFPath)
	{
		string a = relativeNIFPath.Remove(relativeNIFPath.Length - 4, 4); // remove ".nif" or ".trk"
		return Path.Combine(OUT_OBJ_PATH, a);
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