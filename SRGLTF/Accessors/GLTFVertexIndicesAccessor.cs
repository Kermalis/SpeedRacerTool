using Kermalis.SRGLTF.BufferViews;
using System.Text.Json;

namespace Kermalis.SRGLTF.Accessors;

// Hardcoding it to use ushorts for now
internal sealed class GLTFVertexIndicesAccessor : GLTFAccessor
{
	public int Count;
	public ushort Min;
	public ushort Max;

	public GLTFVertexIndicesAccessor(int index, GLTFBufferView bv)
		: base(index, bv)
	{
		//
	}

	public override void Write(Utf8JsonWriter w)
	{
		w.WriteStartObject();

		w.WriteNumber("bufferView", BufView.Index);
		w.WriteNumber("byteOffset", 0);
		WriteComponentType(w, GLTFAccessorComponentType.UnsignedShort);
		w.WriteNumber("count", Count);
		WriteMinMax(w, Min, Max);
		w.WriteString("type", "SCALAR");

		w.WriteEndObject();
	}
}