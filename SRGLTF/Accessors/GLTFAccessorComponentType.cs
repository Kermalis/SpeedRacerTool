namespace Kermalis.SRGLTF.Accessors;

internal enum GLTFAccessorComponentType : ushort
{
	SignedByte = 5_120,
	UnsignedByte = 5_121,
	SignedShort = 5_122,
	UnsignedShort = 5_123,
	// SignedInt not supported https://registry.khronos.org/glTF/specs/2.0/glTF-2.0.html#accessor-data-types
	UnsignedInt = 5_125,
	Float = 5_126,
}