#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler2D TextureSampler : register(s0);

Texture2D lightMask;
sampler2D LightMaskSampler = sampler_state { Texture = <lightMask>; };

float4 MainPS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
  float4 lightColour = tex2D(LightMaskSampler, texCoord);
  float4 tex = tex2D(TextureSampler, texCoord);

  return lightColour * tex;
}

technique SpriteDrawing
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 MainPS();
    }
};