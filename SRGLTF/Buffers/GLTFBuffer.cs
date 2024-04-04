using System.Text.Json;

namespace Kermalis.SRGLTF.Buffers;

internal abstract class GLTFBuffer
{
	public int Index;

	public abstract void Write(Utf8JsonWriter w);
}