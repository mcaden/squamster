float4 uvMap_fp_hlsl(
      float2 uv  : TEXCOORD0
      ) : COLOR
{
  float4 color = float4(uv.x, uv.y, 0, 1);
  return color;
}