using System.Numerics;
using System.Text.Json;

namespace Kermalis.SRGLTF;

internal static class GLTFUtils
{
	public static void WriteVector3(this Utf8JsonWriter w, string propertyName, Vector3 value)
	{
		w.WriteStartArray(propertyName);
		w.WriteNumberValue(value.X);
		w.WriteNumberValue(value.Y);
		w.WriteNumberValue(value.Z);
		w.WriteEndArray();
	}
}