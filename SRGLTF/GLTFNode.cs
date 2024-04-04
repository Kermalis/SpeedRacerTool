using System;
using System.Numerics;
using System.Text.Json;

namespace Kermalis.SRGLTF;

public sealed class GLTFNode
{
	internal readonly int Index;
	internal readonly GLTFScene Parent;

	public string? Name;
	public Vector3? Translation;
	public GLTFMesh? Mesh;

	internal GLTFNode(int index, GLTFScene parent)
	{
		Index = index;
		Parent = parent;
	}

	public GLTFMesh CreateMesh()
	{
		if (Mesh is not null)
		{
			throw new Exception();
		}

		Mesh = Parent.Parent.CreateMesh(this);
		return Mesh;
	}

	internal void Write(Utf8JsonWriter w)
	{
		w.WriteStartObject();

		if (Name is not null)
		{
			w.WriteString("name", Name);
		}
		if (Translation is not null)
		{
			w.WriteVector3("translation", Translation.Value);
		}

		w.WriteEndObject();
	}
}