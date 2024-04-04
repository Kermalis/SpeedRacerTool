namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal enum BillboardMode : ushort
{
	ALWAYS_FACE_CAMERA = 0,
	/// <summary>The billboard will only rotate around the up axis.</summary>
	ROTATE_ABOUT_UP = 1,
	RIGID_FACE_CAMERA = 2,
	ALWAYS_FACE_CENTER = 3,
	RIGID_FACE_CENTER = 4,
	ROTATE_ABOUT_UP2 = 9,
}

/// <summary>Used by NiGeometryData to control the volatility of the mesh.
/// Consistency Type is masked to only the upper 4 bits(0xF000). Dirty mask is the lower 12 (0x0FFF) but only used at runtime.</summary>
internal enum ConsistencyType : ushort
{
	CT_MUTABLE = 0x0000,
	CT_STATIC = 0x4000,
	CT_VOLATILE = 0x8000,
}

internal enum CoordGenType : uint
{
	CG_WORLD_PARALLEL = 0,
	CG_WORLD_PERSPECTIVE = 1,
	CG_SPHERE_MAP = 2,
	CG_SPECULAR_CUBE_MAP = 3,
	CG_DIFFUSE_CUBE_MAP = 4,
}

/// <summary>The type of information that's stored in a texture used by a NiTextureEffect.</summary>
internal enum EffectType : uint
{
	EFFECT_PROJECTED_LIGHT = 0,
	EFFECT_PROJECTED_SHADOW = 1,
	EFFECT_ENVIRONMENT_MAP = 2,
	EFFECT_FOG_MAP = 3,
}

internal enum TexClampMode : uint
{
	CLAMP_S_CLAMP_T = 0,
	CLAMP_S_WRAP_T = 1,
	WRAP_S_CLAMP_T = 2,
	WRAP_S_WRAP_T = 3,
}

internal enum TexFilterMode : uint
{
	FILTER_NEAREST = 0,
	FILTER_BILERP = 1,
	FILTER_TRILERP = 2,
	FILTER_NEAREST_MIPNEAREST = 3,
	FILTER_NEAREST_MIPLERP = 4,
	FILTER_BILERP_MIPNEAREST = 5,
}