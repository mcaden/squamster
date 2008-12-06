/* ========================================================================== */
/*                                                                            */
/*   Filename.c                                                               */
/*   (c) 2001 Author                                                          */
/*                                                                            */
/*   Description                                                              */
/*                                                                            */
/* ========================================================================== */


float4 uvMap_fp(
      float2 uv  : TEXCOORD0
      ) : COLOR
{
  float4 color;
  color.r = uv.x;
  color.g = uv.y;
  color.b = 0;
  color.a = 256;
 
  return color;
}

