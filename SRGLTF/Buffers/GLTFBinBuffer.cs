using Kermalis.EndianBinaryIO;
using System;
using System.IO;
using System.Text.Json;

namespace Kermalis.SRGLTF.Buffers;

// If there's BIN data, we must create a buffer in JSON for it (https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#glb-stored-buffer)
internal sealed class GLTFBinBuffer : GLTFBuffer, IDisposable
{
	public readonly EndianBinaryWriter BinWriter;

	public int BinLength => (int)BinWriter.Stream.Length;

	public GLTFBinBuffer()
	{
		var ms = new MemoryStream(); // Disposed in Dispose() below
		BinWriter = new EndianBinaryWriter(ms, ascii: true);
	}

	public override void Write(Utf8JsonWriter w)
	{
		int binLength = (int)BinWriter.Stream.Length;
		if (binLength < 1)
		{
			throw new Exception();
		}

		w.WriteStartObject();
		w.WriteNumber("byteLength", binLength); // This does not need to match the BIN length with padding
		w.WriteEndObject();
	}

	public void Dispose()
	{
		BinWriter.Stream.Dispose();
	}
}