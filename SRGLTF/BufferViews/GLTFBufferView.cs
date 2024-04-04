using Kermalis.SRGLTF.Buffers;
using System.Text.Json;

namespace Kermalis.SRGLTF.BufferViews;

internal abstract class GLTFBufferView
{
	public readonly int Index;
	public readonly GLTFBuffer Buf;

	protected GLTFBufferView(int index, GLTFBuffer buf)
	{
		Index = index;
		Buf = buf;
	}

	public abstract void Write(Utf8JsonWriter w);
}