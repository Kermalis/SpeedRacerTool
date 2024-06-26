﻿using Kermalis.EndianBinaryIO;
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

	private const string LOG_PATH = @"Log.txt";
	private const string ISO_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Games\";
	private const string MODDED_PS2_ZIP_FILE = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Mod Test\ISO Contents\DATA\PS2.ZIP";

	private const string RIPPED_PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\";
	private const string RIPPED_PS2_PATH = RIPPED_PATH + @"Original PS2.ZIP ps2_ps2\";
	private const string RIPPED_WII_PATH = RIPPED_PATH + @"Original WII rip\";

	private const string OUT_OBJ_PATH = @"C:\Users\Kermalis\Downloads\Output\";

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
				string originalISOFile = ISO_PATH + @"Speed Racer - Original.iso";
				string newISOFile = ISO_PATH + @"Speed Racer Modded.iso";
				PatchISO_PS2(originalISOFile, newISOFile, MODDED_PS2_ZIP_FILE);
				break;
			}
			case ProgramAction.TestNIF:
			{
				TestNIF(false);
				break;
			}
			case ProgramAction.TestXDS:
			{
				TestXDS();
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

	private static void TestXDS()
	{
		// track 1 heightfield physics (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\physics\t01_phx_ground_hf.xds";

		// track 5 heightfield physics (PS2)
		const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t05\physics\t05_phx_ground_hf.xds";

		Console.WriteLine("Opening {0}", PATH);

		using (FileStream s = File.OpenRead(PATH))
		{
			var xds = new XDSFile(s, true);

			var c = xds.Chunks[0] as PhysicsPropsChunk;
			c?.TestHeightfieldDump(@"C:\Users\Kermalis\Downloads\Test.obj");

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
	private static void TestNIF(bool requireFullHierarchy)
	{
		// Thunderhead grading (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\grading.nif";

		// Thunderhead track_tunnelcap (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01gtunn.nif";

		// Thunderhead track_endcap (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01gjcap.nif";

		// Thunderhead forward short trk data (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\geo\fwd_short.trk";

		// Thunderhead forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\t01ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\geo\fwd_long.nif";

		// Thunderhead props (PS2)
		const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01ggs.nif";
		// Onuris props (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t02\models\t02gocc.nif";
		// Fuji props (PS2) [need to add NiPSysData and other particle chunks]
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t03\models\t03gfji.nif";
		// Aurora props (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t04\models\t04gaur.nif";
		// GrandPrix props (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t05\models\t05ggp.nif";
		// Skorost props (PS2)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t06\models\t06gsk.nif";




		// Fuji forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t03\t03ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t03\geo\fwd_long.nif";

		// Aurora forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t04\t04ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t04\geo\fwd_long.nif";

		// GrandPrix forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t05\t05ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t05\geo\fwd_long.nif";

		// Skorost forward long trk geo (PS2)
		// Its texture is @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t05\t05ttrk1_colr.dds"
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t06\geo\fwd_long.nif";



		// PS2 TESTING
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\ui\models\321go.nif";
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original PS2.ZIP ps2_ps2\tracks\t01\models\t01ldir.nif";


		// Thunderhead track_tunnelcap (WII)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\models\t01gtunn.nif";

		// Thunderhead track_endcap (WII)
		//const string PATH = @"C:\Users\Kermalis\Documents\Emulation\PS2\Hacking\Speed Racer PS2 and WII rip\Original WII rip\tracks\t01\models\t01gjcap.nif";

		Console.WriteLine("Opening {0}", PATH);

		using (FileStream s = File.OpenRead(PATH))
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

	private static string GetNIFOutputDir(string nifPath)
	{
		string a = Path.GetRelativePath(RIPPED_PATH, nifPath);
		a = a.Remove(a.Length - 4, 4); // remove ".nif" or ".trk"
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