using System.Text.Json;

namespace Kermalis.SRGLTF;

public sealed class GLTFMesh
{
	internal readonly int Index;
	internal readonly GLTFNode Parent;

	public string? Name;

	internal GLTFMesh(int index, GLTFNode parent)
	{
		Index = index;
		Parent = parent;
	}

	internal void Write(Utf8JsonWriter w)
	{
		w.WriteStartObject();

		if (Name is not null)
		{
			w.WriteString("name", Name);
		}

		w.WriteEndObject();
	}
}