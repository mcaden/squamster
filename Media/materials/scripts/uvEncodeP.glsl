/* ========================================================================== */
/*                                                                            */
/*   Filename.c                                                               */
/*   (c) 2001 Author                                                          */
/*                                                                            */
/*   Description                                                              */
/*                                                                            */
/* ========================================================================== */




void main()
{
  gl_FragColor = vec4( gl_TexCoord[0].x, gl_TexCoord[0].y, 0, 1 );
}
