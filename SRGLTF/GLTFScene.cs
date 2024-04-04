using System.Collections.Generic;
using System.Text.Json;

namespace Kermalis.SRGLTF;

public sealed class GLTFScene
{
	internal readonly GLTFWriter Parent;

	public string? Name;
	private List<GLTFNode>? _nodes;

	internal GLTFScene(GLTFWriter parent)
	{
		Parent = parent;
	}

	public GLTFNode CreateNode()
	{
		GLTFNode n = Parent.CreateNode(this);
		_nodes ??= new List<GLTFNode>(1);
		_nodes.Add(n);
		return n;
	}

	internal void Write(Utf8JsonWriter w)
	{
		w.WriteStartObject();

		if (Name is not null)
		{
			w.WriteString("name", Name);
		}

		if (_nodes is not null)
		{
			w.WriteStartArray("nodes");
			foreach (GLTFNode n in _nodes)
			{
				w.WriteNumberValue(n.Index);
			}
			w.WriteEndArray();
		}

		w.WriteEndObject();
	}
}