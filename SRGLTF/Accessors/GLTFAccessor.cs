using Kermalis.SRGLTF.BufferViews;
using System.Numerics;
using System.Text.Json;

namespace Kermalis.SRGLTF.Accessors;

internal abstract class GLTFAccessor
{
	public readonly int Index;
	public readonly GLTFBufferView BufView;

	protected GLTFAccessor(int index, GLTFBufferView bv)
	{
		Index = index;
		BufView = bv;
	}

	public abstract void Write(Utf8JsonWriter w);

	protected static void WriteComponentType(Utf8JsonWriter w, GLTFAccessorComponentType ct)
	{
		w.WriteNumber("componentType", (int)ct);
	}
	protected static void WriteMinMax(Utf8JsonWriter w, int min, int max)
	{
		w.WriteStartArray("max");
		w.WriteNumberValue(max);
		w.WriteEndArray();
		w.WriteStartArray("min");
		w.WriteNumberValue(min);
		w.WriteEndArray();
	}
	protected static void WriteMinMax(Utf8JsonWriter w, Vector3 min, Vector3 max)
	{
		w.WriteVector3("max", max);
		w.WriteVector3("min", min);
	}
}