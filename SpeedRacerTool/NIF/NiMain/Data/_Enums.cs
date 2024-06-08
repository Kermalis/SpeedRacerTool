using System;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

// TODO: Are these const names from some leaked source/debugging, or are they simply from nifskope?

internal enum AlphaFormat : uint
{
	ALPHA_NONE = 0,
	ALPHA_BINARY = 1,
	ALPHA_SMOOTH = 2,
	ALPHA_DEFAULT = 3,
}

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

internal enum ChannelConvention : uint
{
	CC_FIXED = 0,
	/// <summary>Palette</summary>
	CC_INDEX = 3,
	CC_COMPRESSED = 4,
	CC_EMPTY = 5,
}

internal enum ChannelType : uint
{
	CHNL_RED = 0,
	CHNL_GREEN = 1,
	CHNL_BLUE = 2,
	CHNL_ALPHA = 3,
	CHNL_COMPRESSED = 4,
	CHNL_INDEX = 16,
	CHNL_EMPTY = 19,
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

internal enum KeyType : uint
{
	LINEAR_KEY = 1,
	QUADRATIC_KEY = 2,
	TBC_KEY = 3,
	XYZ_ROTATION_KEY = 4,
	CONST_KEY = 5,
}

internal enum MipMapFormat : uint
{
	MIP_FMT_NO = 0,
	MIP_FMT_YES = 1,
	MIP_FMT_DEFAULT = 2,
}

internal enum NiNBTMethod : ushort
{
	NBT_METHOD_NONE = 0,
	NBT_METHOD_NDL = 1,
	NBT_METHOD_MAX = 2,
	NBT_METHOD_ATI = 3,
}

internal enum PixelFormat : uint
{
	PX_FMT_RGB8 = 0,
	PX_FMT_RGBA8 = 1,
	PX_FMT_PAL8 = 2,
	PX_FMT_DXT1 = 4,
	PX_FMT_DXT5 = 5,
	PX_FMT_DXT5_ALT = 6, // Maybe for normals?
}

internal enum PixelLayout : uint
{
	/// <summary>Texture is in 8-bit palette format.</summary>
	PIX_LAY_PALETTISED = 0,
	PIX_LAY_HIGH_COLOR_16 = 1,
	PIX_LAY_TRUE_COLOR_32 = 2,
	PIX_LAY_COMPRESSED = 3,
	PIX_LAY_BUMPMAP = 4,
	/// <summary>Texture is in 4-bit palette format.</summary>
	PIX_LAY_PALETTISED_4 = 5,
	PIX_LAY_DEFAULT = 6,
}

[Flags]
internal enum ShadeFlags : ushort
{
	SHADING_HARD = 0,
	SHADING_SMOOTH = 1 << 0,
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

internal enum TexTransform : uint
{
	TT_TRANSLATE_U = 0,
	TT_TRANSLATE_V = 1,
	TT_ROTATE = 2,
	TT_SCALE_U = 3,
	TT_SCALE_V = 4,
}

internal enum TexType : uint
{
	BASE_MAP = 0,
	DARK_MAP = 1,
	DETAIL_MAP = 2,
	GLOSS_MAP = 3,
	GLOW_MAP = 4,
	BUMP_MAP = 5,
	NORMAL_MAP = 6,
	UNKNOWN2_MAP = 7,
	DECAL_0_MAP = 8,
	DECAL_1_MAP = 9,
	DECAL_2_MAP = 10,
	DECAL_3_MAP = 11,
}

// I don't have NifSkope names here
[Flags]
internal enum ZBufferFlags : ushort
{
	None = 0,
	EnableZTest = 1 << 0,
	/// <summary>If this flag isn't set, it's read-only</summary>
	EnableReadWrite = 1 << 1,
}