using Kermalis.EndianBinaryIO;
using Kermalis.SRGLTF.Accessors;
using Kermalis.SRGLTF.Buffers;
using Kermalis.SRGLTF.BufferViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Kermalis.SRGLTF;

// If GLTF gets too complicated, use SharpGLTF.Toolkit
// https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html
public sealed class GLTFWriter : IDisposable
{
	private GLTFBinBuffer? _binBuffer;

	private List<GLTFAccessor>? _accessors;
	private List<GLTFBuffer>? _buffers;
	private List<GLTFBufferView>? _bufferViews;
	private List<GLTFMesh>? _meshes;
	private List<GLTFNode>? _nodes;
	private List<GLTFScene>? _scenes;

	public GLTFScene CreateScene()
	{
		_scenes ??= new List<GLTFScene>(1);
		var s = new GLTFScene(this);
		_scenes.Add(s);
		return s;
	}
	internal GLTFNode CreateNode(GLTFScene s)
	{
		_nodes ??= new List<GLTFNode>(1);
		var n = new GLTFNode(_nodes.Count, s);
		_nodes.Add(n);
		return n;
	}
	internal GLTFMesh CreateMesh(GLTFNode n)
	{
		_meshes ??= new List<GLTFMesh>(1);
		var m = new GLTFMesh(_meshes.Count, n);
		_meshes.Add(m);
		return m;
	}
	internal GLTFBinBuffer GetBinBuffer()
	{
		if (_binBuffer is null)
		{
			if (_buffers is null)
			{
				_buffers = new List<GLTFBuffer>(1);
			}
			else
			{
				foreach (GLTFBuffer b in _buffers)
				{
					b.Index++;
				}
			}
			_binBuffer = new GLTFBinBuffer();
			_buffers.Insert(0, _binBuffer);
		}
		return _binBuffer;
	}

	public void WriteGLB(Stream s)
	{
		const int HEADER_LENGTH_OFS = 0x8;

		var w = new EndianBinaryWriter(s, ascii: true);

		w.WriteChars("glTF");
		w.WriteUInt32(2); // Version 2.0
		w.WriteUInt32(0); // Length, come back to it later

		// First chunk must be JSON
		WriteJSONChunk(w);
		// Second chunk must be BIN unless the data is empty
		if (_binBuffer is not null)
		{
			WriteBINChunk(w, _binBuffer);
		}

		// Now that we know the total length, go update it in the header
		long totalLength = s.Position;
		s.Position = HEADER_LENGTH_OFS;
		w.WriteUInt32((uint)totalLength);
	}

	private void WriteJSONChunk(EndianBinaryWriter w)
	{
		const int JSON_LENGTH_OFS = 0xC;
		const int JSON_DATA_OFS = 0x14;

		w.WriteUInt32(0); // Length, come back to it later
		w.WriteChars("JSON");
		WriteJSONData(w.Stream);

		long jsonEndOfs = w.Stream.Position;
		// Check for JSON padding requirements
		int jsonLength = (int)(jsonEndOfs - JSON_DATA_OFS);
		int jsonPaddingCount = GetNecessaryPadding(jsonLength);
		if (jsonPaddingCount > 0)
		{
			jsonEndOfs += jsonPaddingCount;
			// Trailing 0x20s for padding
			Span<byte> pad = stackalloc byte[jsonPaddingCount];
			pad.Fill(0x20);
			w.WriteBytes(pad);
		}

		// Update JSON chunk length
		w.Stream.Position = JSON_LENGTH_OFS;
		w.WriteInt32(jsonLength + jsonPaddingCount);

		// Go to the end again
		w.Stream.Position = jsonEndOfs;
	}
	private void WriteJSONData(Stream stream)
	{
		var opt = new JsonWriterOptions
		{
			Indented = false,
		};
		var w = new Utf8JsonWriter(stream, options: opt);

		w.WriteStartObject();

		// Asset property
		w.WriteStartObject("asset");
		w.WriteString("version", "2.0"); // The version is supposed to be a string, not a number
		w.WriteEndObject();

		if (_accessors is not null)
		{
			w.WriteStartArray("accessors");
			foreach (GLTFAccessor a in _accessors)
			{
				a.Write(w);
			}
			w.WriteEndArray();
		}
		if (_buffers is not null)
		{
			w.WriteStartArray("buffers");
			foreach (GLTFBuffer b in _buffers)
			{
				b.Write(w);
			}
			w.WriteEndArray();
		}
		if (_bufferViews is not null)
		{
			w.WriteStartArray("bufferViews");
			foreach (GLTFBufferView bv in _bufferViews)
			{
				bv.Write(w);
			}
			w.WriteEndArray();
		}
		if (_meshes is not null)
		{
			w.WriteStartArray("meshes");
			foreach (GLTFMesh m in _meshes)
			{
				m.Write(w);
			}
			w.WriteEndArray();
		}
		if (_nodes is not null)
		{
			w.WriteStartArray("nodes");
			foreach (GLTFNode n in _nodes)
			{
				n.Write(w);
			}
			w.WriteEndArray();
		}
		if (_scenes is not null)
		{
			w.WriteStartArray("scenes");
			foreach (GLTFScene s in _scenes)
			{
				s.Write(w);
			}
			w.WriteEndArray();

			// Hardcode the first scene to be the visible one
			w.WriteNumber("scene", 0);
		}

		w.WriteEndObject();

		w.Flush();
	}

	private static void WriteBINChunk(EndianBinaryWriter w, GLTFBinBuffer bin)
	{
		int binLength = bin.BinLength;
		if (binLength < 1)
		{
			throw new Exception();
		}

		// Check for BIN padding requirements
		int binPaddingCount = GetNecessaryPadding(binLength);

		w.WriteInt32(binLength + binPaddingCount);
		w.WriteChars("BIN ");
		bin.BinWriter.Stream.CopyTo(w.Stream);
		if (binPaddingCount > 0)
		{
			w.WriteZeroes(binPaddingCount); // Trailing 0x00s for padding
		}
	}

	public static int GetNecessaryPadding(int len)
	{
		int val = len % 4;
		return val == 0 ? 0 : 4 - val;
	}

	public void Dispose()
	{
		_binBuffer?.Dispose();
	}
}