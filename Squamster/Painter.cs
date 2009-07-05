/*************************************************************************

This file is part of Squamster - An Ogre /mesh viewer/painter for windows.
 
 * Copyright ©  2008 - Katalyst Studios 

    Squamster is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Squamster is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Squamster.  If not, see <http://www.gnu.org/licenses/>.


**************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mogre;

namespace Squamster
{
    class Painter
    {
#if DEBUG
        public Rectangle2D mMiniScreen;
        public SceneNode mMiniScreenNode;
#endif        
        public List<TexturePtr> customTextures = new List<TexturePtr>();
        public List<System.Drawing.Bitmap> brushes = new List<System.Drawing.Bitmap>();
        System.Drawing.Bitmap currentTransformedBrush;
        int mCurrentBrush = -1;
        float mBrushScale = 1;
        float mBrushOpacity = 1;
        public int activeTexture = -1;
        public TexturePtr uvEncodedTexture;
        StringVector materials = new StringVector();
        public PaintModes paintMode = PaintModes.TOOLS;
        public Tools currentTool = Tools.BRUSH;

        public enum Tools
        { 
            BRUSH,
            BLUR,
            SHARPEN,
            DODGE,
            BURN
        }

        public static ColourValue penColor = new ColourValue(1, 0, 0, 1);

        public Painter()
        {
            uvEncodedTexture = TextureManager.Singleton.CreateManual("RttTex",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                OgreForm.mWindow.Width, OgreForm.mWindow.Height, 0, PixelFormat.PF_FLOAT16_RGBA, (int)TextureUsage.TU_RENDERTARGET);




            RenderTexture renderTexture = uvEncodedTexture.GetBuffer().GetRenderTarget();
            renderTexture.AddViewport(OgreForm.ExCamera.getOgreCamera);
            renderTexture.GetViewport(0).SetClearEveryFrame(true);
            renderTexture.GetViewport(0).BackgroundColour = ColourValue.White;
            renderTexture.GetViewport(0).OverlaysEnabled = false;

#if DEBUG

            mMiniScreen = new Rectangle2D(true);

            mMiniScreen.SetCorners(0.5f, -0.5f, 1.0f, -1.0f);
            mMiniScreen.BoundingBox = new AxisAlignedBox(-100000.0f * Mogre.Vector3.UNIT_SCALE, 100000.0f * Mogre.Vector3.UNIT_SCALE);

            mMiniScreenNode = OgreForm.mSceneMgr.RootSceneNode.CreateChildSceneNode("MiniScreenNodeX");
            mMiniScreenNode.AttachObject(mMiniScreen);



            //Materials for the rtt Screen
            MaterialPtr rttMaterial = MaterialManager.Singleton.Create("RttMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            rttMaterial.CreateTechnique().CreatePass();
            rttMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
            rttMaterial.GetTechnique(0).GetPass(0).CreateTextureUnitState("RttTex").NumMipmaps = 0;
            mMiniScreen.SetMaterial("RttMat");

#endif

            //Materials for the uv encoding
            MaterialPtr uvEncodingMaterial = MaterialManager.Singleton.Create("uvEncodingMatX", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            uvEncodingMaterial.CreateTechnique().CreatePass();
            uvEncodingMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
            uvEncodingMaterial.GetTechnique(0).GetPass(0).SetVertexProgram("uvEncode_vs_glsl");
            uvEncodingMaterial.GetTechnique(0).GetPass(0).SetFragmentProgram("uvEncode_ps_glsl");

            uvEncodingMaterial.CreateTechnique().CreatePass();
            uvEncodingMaterial.GetTechnique(1).GetPass(0).LightingEnabled = false;
            uvEncodingMaterial.GetTechnique(1).GetPass(0).SetFragmentProgram("uvEncode_ps_hlsl");
            renderTexture.PreRenderTargetUpdate += new RenderTargetListener.PreRenderTargetUpdateHandler(preRenderTargetUpdateX);
            renderTexture.PostRenderTargetUpdate += new RenderTargetListener.PostRenderTargetUpdateHandler(postRenderTargetUpdateX);
        }

        public int currentBrush
        {
            get
            {
                return mCurrentBrush;
            }
            set
            {
                mCurrentBrush = value;
                currentTransformedBrush = new Bitmap(brushes[currentBrush], new Size((int)(brushes[currentBrush].Width * mBrushScale), (int)(brushes[currentBrush].Height * mBrushScale)));
                LogManager.Singleton.LogMessage("Current brush set as: " + mCurrentBrush.ToString());
            }
        }

        public void addBrush(System.Drawing.Bitmap brushImage)
        {
            brushes.Add(brushImage);
        }

        public float BrushScale
        {
            get
            {
                return mBrushScale;
            }

            set
            {
                mBrushScale = value;
                currentTransformedBrush = new Bitmap(brushes[currentBrush], new Size((int)(brushes[currentBrush].Width * mBrushScale), (int)(brushes[currentBrush].Height * mBrushScale)));
            }
        }

        public Point getBrushSize()
        {
            Point scaledBrushSize = new Point( 0, 0 );
            if (currentBrush >= 0)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Getting brush size...");
                scaledBrushSize.X = (int)(brushes[mCurrentBrush].Width * mBrushScale);
                scaledBrushSize.Y = (int)(brushes[mCurrentBrush].Height * mBrushScale);
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Brush size retrieved as: " + scaledBrushSize.ToString());
            }

            return scaledBrushSize;
        }

        public float BrushOpacity
        {
            get
            {
                return mBrushOpacity;
            }

            set
            {
                mBrushOpacity = value;
            }
        }

        public void dispose()
        {
            for (int i = 0; i < customTextures.Count; i++)
            {
                customTextures[i] = null;
            }
            uvEncodedTexture = null;
        }

        public PointF draw(int mouseX, int mouseY, ColourValue penColor)
        {
            PointF previewPoint = new PointF(-1, -1);
            switch (paintMode)
            { 
                case PaintModes.TOOLS:
                    useTool(mouseX, mouseY, penColor);
                    break;
                case PaintModes.FILTER:
                    break;
            }

            return previewPoint;
        }

        private PointF useTool(int mouseX, int mouseY, ColourValue penColor)
        {
            PointF previewPoint = new PointF(-1, -1);
            if (activeTexture >= 0 && customTextures[activeTexture] != null)
            {
                float uvX = 0;
                float uvY = 0;

                if (mouseX < 0)
                    mouseX = 0;
                else if (mouseX > OgreForm.mWindow.Width)
                    mouseX = (int)OgreForm.mWindow.Width;

                if (mouseY < 0)
                    mouseY = 0;
                else if (mouseY > OgreForm.mWindow.Height)
                    mouseY = (int)OgreForm.mWindow.Height;

                Point tl = new Point(mouseX - (currentTransformedBrush.Width / 2), mouseY - (currentTransformedBrush.Height / 2));
                if (tl.X < 0)
                    tl.X = 0;
                else if (tl.X >= uvEncodedTexture.Width)
                    tl.X = (int)uvEncodedTexture.Width - 1;

                if (tl.Y < 0)
                    tl.Y = 0;
                else if (tl.Y >= uvEncodedTexture.Height)
                    tl.Y = (int)uvEncodedTexture.Height - 1;

                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Top-left = " + tl.ToString());

                Point br = new Point(tl.X + currentTransformedBrush.Width, tl.Y + currentTransformedBrush.Height);
                if (br.X >= uvEncodedTexture.Width)
                    br.X = (int)uvEncodedTexture.Width - 1;
                if (br.Y >= uvEncodedTexture.Height)
                    br.Y = (int)uvEncodedTexture.Height - 1;

                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Bottom-Right = " + br.ToString());

                int width = br.X - tl.X;
                int height = br.Y - tl.Y;

                List<Point> points = new List<Point>();
                List<Point> brushPoints = new List<Point>();

                unsafe
                {
                    HardwarePixelBufferSharedPtr buffer = uvEncodedTexture.GetBuffer();
                    buffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                    ColourValue uvColorValue = new ColourValue();
                    PixelBox pbox = buffer.CurrentLock;

                    HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                    meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                    PixelBox texEdit = meshTexBuffer.CurrentLock;

                    Box writebox = new Box((uint)tl.X, (uint)tl.Y, (uint)br.X, (uint)br.Y);

                    for (uint i = (uint)tl.X; i < br.X; i++)
                    {
                        for (uint j = (uint)tl.Y; j < br.Y; j++)
                        {
                            writebox = new Box(i, j, i, j);
                            PixelUtil.UnpackColour(&uvColorValue, uvEncodedTexture.Format, pbox.GetSubVolume(writebox).data.ToPointer());

                            if (!(uvColorValue.b > 0))
                            {
                                float red = uvColorValue.r;
                                float green = uvColorValue.g;

                                red = red % 1;
                                if (red < 0)
                                {
                                    red = red + 1;
                                }
                                green = green % 1;
                                if (green < 0)
                                {
                                    green = green + 1;
                                }

                                previewPoint.X = red;
                                previewPoint.Y = green;

                                uvX = red * (float)customTextures[activeTexture].Width - 1;
                                uvY = green * (float)customTextures[activeTexture].Height - 1;

                                if (!points.Contains(new Point((int)(uvX + .5), (int)(uvY + .5))))
                                {
                                    points.Add(new Point((int)(uvX + .5), (int)(uvY + .5)));
                                    brushPoints.Add(new Point((int)(i - tl.X), (int)(j - tl.Y)));
                                }
                            }
                        }
                    }

                    switch (currentTool)
                    {
                        case Tools.BRUSH:
                            usePaintBrush(penColor, points, brushPoints, texEdit, writebox);
                            break;
                        case Tools.BLUR:
                            ConvMatrix blurMatrix = new ConvMatrix();
                            blurMatrix.SetAll(1);
                            blurMatrix.Factor = 9;
                            useConvolutionTool(points, brushPoints, ref texEdit, ref writebox, blurMatrix);
                            break;
                        case Tools.SHARPEN:
                            ConvMatrix sharpenMatrix = new ConvMatrix();
                            sharpenMatrix.SetAll(0);
                            sharpenMatrix.MidLeft = -2;
                            sharpenMatrix.MidRight = -2;
                            sharpenMatrix.TopMid = -2;
                            sharpenMatrix.BottomMid = -2;
                            sharpenMatrix.Pixel = 11;
                            sharpenMatrix.Factor = 3;
                            useConvolutionTool(points, brushPoints, ref texEdit, ref writebox, sharpenMatrix);
                            break;
                        case Tools.DODGE:
                            useDodgeTool(penColor, points, brushPoints, texEdit, writebox);
                            break;
                        case Tools.BURN:
                            useBurnTool(penColor, points, brushPoints, texEdit, writebox);
                            break;
                        default:
                            LogManager.Singleton.LogMessage("Error - Unknown tool");
                            break;
                    }

                    meshTexBuffer.Unlock();
                    buffer.Unlock();
                }
            }
            return previewPoint;
        }

        unsafe private void useDodgeTool(ColourValue penColor, List<Point> points, List<Point> brushPoints, PixelBox texEdit, Box writebox)
        {
            for (int i = 0; i < points.Count; i++)
            {
                writebox = new Box((uint)points[i].X, (uint)points[i].Y, (uint)points[i].X, (uint)points[i].Y);

                ColourValue baseColor = new ColourValue();

                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                ColourValue drawColor = new ColourValue();
                drawColor.SetAsARGB((uint)currentTransformedBrush.GetPixel((int)(brushPoints[i].X), (int)(brushPoints[i].Y)).ToArgb());

                float alpha = (1 - drawColor.r) * mBrushOpacity;


                //component + 0.25 * sin(component * PI)
                drawColor.r = alpha * (baseColor.r + 0.25f * (float)System.Math.Sin(baseColor.r * System.Math.PI)) + baseColor.r * (1 - alpha);
                drawColor.g = alpha * (baseColor.g + 0.25f * (float)System.Math.Sin(baseColor.g * System.Math.PI)) + baseColor.g * (1 - alpha);
                drawColor.b = alpha * (baseColor.b + 0.25f * (float)System.Math.Sin(baseColor.b * System.Math.PI)) + baseColor.b * (1 - alpha);


                PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
            }
        }

        unsafe private void useBurnTool(ColourValue penColor, List<Point> points, List<Point> brushPoints, PixelBox texEdit, Box writebox)
        {
            for (int i = 0; i < points.Count; i++)
            {
                writebox = new Box((uint)points[i].X, (uint)points[i].Y, (uint)points[i].X, (uint)points[i].Y);

                ColourValue baseColor = new ColourValue();

                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                ColourValue drawColor = new ColourValue();
                drawColor.SetAsARGB((uint)currentTransformedBrush.GetPixel((int)(brushPoints[i].X), (int)(brushPoints[i].Y)).ToArgb());

                float alpha = (1 - drawColor.r) * mBrushOpacity;


                //0.25 * component
                drawColor.r = alpha * (baseColor.r * .25f) + baseColor.r * (1 - alpha);
                drawColor.g = alpha * (baseColor.g * .25f) + baseColor.g * (1 - alpha);
                drawColor.b = alpha * (baseColor.b * .25f) + baseColor.b * (1 - alpha);


                PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
            }
        }

        unsafe private void usePaintBrush(ColourValue penColor, List<Point> points, List<Point> brushPoints, PixelBox texEdit, Box writebox)
        {
            for (int i = 0; i < points.Count; i++)
            {
                writebox = new Box((uint)points[i].X, (uint)points[i].Y, (uint)points[i].X, (uint)points[i].Y);

                ColourValue baseColor = new ColourValue();

                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                ColourValue drawColor = new ColourValue();
                drawColor.SetAsARGB((uint)currentTransformedBrush.GetPixel((int)(brushPoints[i].X), (int)(brushPoints[i].Y)).ToArgb());

                float alpha = (1 - drawColor.r) * mBrushOpacity;

                drawColor.r = alpha * penColor.r + baseColor.r * (1 - alpha);
                drawColor.g = alpha * penColor.g + baseColor.g * (1 - alpha);
                drawColor.b = alpha * penColor.b + baseColor.b * (1 - alpha);


                PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
            }
        }

        unsafe private void useConvolutionTool(List<Point> points, List<Point> brushPoints, ref PixelBox texEdit, ref Box writebox, ConvMatrix toolMatrix)
        {
            for (int i = 0; i < points.Count; i++)
            {
                List<ColourValue> pixels = new List<ColourValue>();
                ColourValue prevColor = new ColourValue();
                writebox = new Box((uint)points[i].X, (uint)points[i].Y, (uint)points[i].X, (uint)points[i].Y);
                PixelUtil.UnpackColour(&prevColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                if (points[i].X > 0 && points[i].X < customTextures[activeTexture].Width - 1 && points[i].Y > 0 && points[i].Y < customTextures[activeTexture].Height - 1)
                {
                    ColourValue baseColor = new ColourValue();
                    Box subCoords;
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            subCoords = new Box((uint)(points[i].X + x), (uint)(points[i].Y + y), (uint)(points[i].X + x), (uint)(points[i].Y + y));
                            PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(subCoords).data.ToPointer());
                            pixels.Add(baseColor);
                        }
                    }
                    Bitmap bm = new Bitmap(3, 3, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    System.Drawing.Imaging.BitmapData bmd = bm.LockBits(new System.Drawing.Rectangle(0, 0, 3, 3), System.Drawing.Imaging.ImageLockMode.WriteOnly, bm.PixelFormat);
                    int PixelSize = 4;

                    for (int y = 0; y < bmd.Height; y++)
                    {
                        byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                        for (int x = 0; x < bmd.Width; x++)
                        {
                            Mogre.ColourValue color = pixels[x + (y * 3)];
                            row[x * PixelSize] = (byte)(color.b * 255);
                            row[x * PixelSize + 1] = (byte)(color.g * 255);
                            row[x * PixelSize + 2] = (byte)(color.r * 255);
                            row[x * PixelSize + 3] = (byte)(color.a * 255);
                        }
                    }
                    bm.UnlockBits(bmd);

                    Conv3x3(bm, toolMatrix);

                    ColourValue blurColor = new ColourValue();
                    ColourValue drawColor = new ColourValue();
                    blurColor.SetAsARGB((uint)bm.GetPixel(1, 1).ToArgb());
                    drawColor.SetAsARGB((uint)currentTransformedBrush.GetPixel((int)(brushPoints[i].X), (int)(brushPoints[i].Y)).ToArgb());

                    float alpha = (1 - drawColor.r) * mBrushOpacity;

                    drawColor.r = alpha * blurColor.r + prevColor.r * (1 - alpha);
                    drawColor.g = alpha * blurColor.g + prevColor.g * (1 - alpha);
                    drawColor.b = alpha * blurColor.b + prevColor.b * (1 - alpha);
                    drawColor.a = 1;
                    PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                }
            }
        }

        public void setActiveTexture(string texture)
        {
            if ( texture.StartsWith("drawTex_"))
            {
                LogManager.Singleton.LogMessage("Fetching draw-texture '" + texture + "' since it already exists...");
                bool textureSelected = false;
                for (int i = 0; i < customTextures.Count && !textureSelected; i++)
                {
                    if (customTextures[i].Name == texture)
                    {
                        activeTexture = i;
                        textureSelected = true;
                    }
                }
                LogManager.Singleton.LogMessage("Now drawing on texture: " + customTextures[activeTexture].Name);
            }
            else
            { 
                string drawTexName = "drawTex_" + texture;
                LogManager.Singleton.LogMessage("Creating a new draw-texture for '" + texture + "' since it doesn't already exist...");
                TexturePtr originalTexture = TextureManager.Singleton.GetByName(texture);
                customTextures.Add(TextureManager.Singleton.CreateManual(drawTexName,
                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                    originalTexture.Width, originalTexture.Height, 0, originalTexture.Format, (int)TextureUsage.TU_DYNAMIC));
                activeTexture = customTextures.Count - 1;
                originalTexture.CopyToTexture(customTextures[activeTexture]);
                for (uint i = 0; i < OgreForm.mCurrentEntity.NumSubEntities; i++)
                {
                    Material mat = OgreForm.mCurrentEntity.GetSubEntity(i).GetMaterial();
                    Material.TechniqueIterator techniqueItr = mat.GetTechniqueIterator();
                    while (techniqueItr.MoveNext())
                    {
                        Technique.PassIterator passItr = techniqueItr.Current.GetPassIterator();
                        while (passItr.MoveNext())
                        {
                            Pass.TextureUnitStateIterator texItr = passItr.Current.GetTextureUnitStateIterator();
                            while (texItr.MoveNext())
                            {
                                if (texItr.Current.TextureName == texture)
                                {
                                    texItr.Current.SetTextureName(customTextures[activeTexture].Name);
                                }
                            }
                        }
                    }
                }
                LogManager.Singleton.LogMessage("Now drawing on texture: " + customTextures[activeTexture].Name);
            }

        }

        public void applyFilter(Filters filter, Object arguments)
        {
            switch (filter)
            { 
                case Filters.INVERT:
                    invertFilter(customTextures[activeTexture]);
                    break;
                case Filters.GRAYSCALE:
                    grayscaleFilter(customTextures[activeTexture]);
                    break;
                case Filters.BRIGHTNESS:
                    brightnessFilter(customTextures[activeTexture], (float)arguments);
                    break;
                case Filters.CONTRAST:
                    contrastFilter(customTextures[activeTexture], (float)arguments);
                    break;
                case Filters.BLUR:
                    blurFilter(customTextures[activeTexture]);
                    break;
                case Filters.GAUSSIAN_BLUR:
                    gaussianBlurFilter(customTextures[activeTexture]);
                    break;
                case Filters.SHARPEN:
                    sharpenFilter(customTextures[activeTexture]);
                    break;
                default:
                    break;
            }
        }

        public enum Filters
        { 
            INVERT,
            CONTRAST,
            BRIGHTNESS,
            GRAYSCALE,
            BLUR,
            GAUSSIAN_BLUR,
            SHARPEN
        }

        public enum PaintModes
        {
            TOOLS,
            FILTER
        }

        private void invertFilter( Mogre.TexturePtr texture )
        {
            unsafe
            {
                uint texWidth = customTextures[activeTexture].Width;
                uint texHeight = customTextures[activeTexture].Height;
                HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox texEdit = meshTexBuffer.CurrentLock;
                for (uint x = 0; x < texWidth; x++)
                {
                    for (uint y = 0; y < texHeight; y++)
                    {
                        Box writebox = new Box(x, y, x, y);

                        ColourValue baseColor = new ColourValue();

                        PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                        ColourValue drawColor = new ColourValue();
                        drawColor.a = baseColor.a;
                        drawColor.r = 1 - baseColor.r;
                        drawColor.g = 1 - baseColor.g;
                        drawColor.b = 1 - baseColor.b;

                        PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                    }
                }
                meshTexBuffer.Unlock();
            }
        }

        private void grayscaleFilter(Mogre.TexturePtr texture)
        {
            unsafe
            {
                uint texWidth = customTextures[activeTexture].Width;
                uint texHeight = customTextures[activeTexture].Height;
                HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox texEdit = meshTexBuffer.CurrentLock;
                for (uint x = 0; x < texWidth; x++)
                {
                    for (uint y = 0; y < texHeight; y++)
                    {
                        Box writebox = new Box(x, y, x, y);

                        ColourValue baseColor = new ColourValue();

                        PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                        ColourValue drawColor = new ColourValue();
                        drawColor.a = baseColor.a;
                        drawColor.r = drawColor.g = drawColor.b = (.288f * baseColor.r + .587f * baseColor.g + .114f * baseColor.b);

                        PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                    }
                }
                meshTexBuffer.Unlock();
            }
        }

        private void blurFilter(Mogre.TexturePtr texture)
        {
            unsafe
            {
                customTextures[activeTexture].GetBuffer().Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                PixelBox mPboxdst = customTextures[activeTexture].GetBuffer().CurrentLock;
                Mogre.Image mImage = new Mogre.Image();
                LogManager.Singleton.LogMessage("Loading texture data into image");
                mImage.LoadDynamicImage((byte*)mPboxdst.data.ToPointer(), customTextures[activeTexture].Width, customTextures[activeTexture].Height, customTextures[activeTexture].Format);
                customTextures[activeTexture].GetBuffer().Unlock();
                LogManager.Singleton.LogMessage("Creating preview image");
                System.Drawing.Bitmap texImage = MogreImageToBitmap(mImage);
                ConvMatrix m = new ConvMatrix();
                m.SetAll(1);
                m.Factor = 9;

                Conv3x3(texImage, m);

                copyBitmapToTexture(customTextures[activeTexture], texImage);
            }
        }

        private void gaussianBlurFilter(Mogre.TexturePtr texture)
        {
            unsafe
            {
                customTextures[activeTexture].GetBuffer().Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                PixelBox mPboxdst = customTextures[activeTexture].GetBuffer().CurrentLock;
                Mogre.Image mImage = new Mogre.Image();
                LogManager.Singleton.LogMessage("Loading texture data into image");
                mImage.LoadDynamicImage((byte*)mPboxdst.data.ToPointer(), customTextures[activeTexture].Width, customTextures[activeTexture].Height, customTextures[activeTexture].Format);
                customTextures[activeTexture].GetBuffer().Unlock();
                LogManager.Singleton.LogMessage("Creating preview image");
                System.Drawing.Bitmap texImage = MogreImageToBitmap(mImage);
                ConvMatrix m = new ConvMatrix();
                m.SetAll(1);
                m.Pixel = 4;
                m.BottomMid = 2;
                m.TopMid = 2;
                m.MidLeft = 2;
                m.MidRight = 2;
                m.Factor = 16;

                Conv3x3(texImage, m);

                copyBitmapToTexture(customTextures[activeTexture], texImage);
            }
        }

        private void sharpenFilter(Mogre.TexturePtr texture)
        {
            unsafe
            {
                customTextures[activeTexture].GetBuffer().Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                PixelBox mPboxdst = customTextures[activeTexture].GetBuffer().CurrentLock;
                Mogre.Image mImage = new Mogre.Image();
                LogManager.Singleton.LogMessage("Loading texture data into image");
                mImage.LoadDynamicImage((byte*)mPboxdst.data.ToPointer(), customTextures[activeTexture].Width, customTextures[activeTexture].Height, customTextures[activeTexture].Format);
                customTextures[activeTexture].GetBuffer().Unlock();
                LogManager.Singleton.LogMessage("Creating preview image");
                System.Drawing.Bitmap texImage = MogreImageToBitmap(mImage);
                ConvMatrix m = new ConvMatrix();
                m.SetAll(0);
                m.Pixel = 11;
                m.BottomMid = -2;
                m.TopMid = -2;
                m.MidLeft = -2;
                m.MidRight = -2;
                m.Factor = 3;

                Conv3x3(texImage, m);

                copyBitmapToTexture(customTextures[activeTexture], texImage);
            }
        }

        unsafe private void copyBitmapToTexture(TexturePtr texture, System.Drawing.Bitmap texImage)
        {
            texture.GetBuffer().Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
            PixelBox mPboxdst = texture.GetBuffer().CurrentLock;
            System.Drawing.Imaging.BitmapData bmd = texImage.LockBits(new System.Drawing.Rectangle(0, 0, (int)texImage.Width, (int)texImage.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, texImage.PixelFormat);
            int PixelSize = 4;

            for (uint y = 0; y < bmd.Height; y++)
            {
                byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                for (uint x = 0; x < bmd.Width; x++)
                {
                    Box writebox = new Box(x, y, x, y);
                    Mogre.ColourValue color = new ColourValue();
                    color.b = (float)(Convert.ToInt32(row[x * PixelSize])) / 255;
                    color.g = (float)(Convert.ToInt32(row[x * PixelSize + 1])) / 255;
                    color.r = (float)(Convert.ToInt32(row[x * PixelSize + 2])) / 255;
                    color.a = 1;

                    PixelUtil.PackColour(color, texture.Format, mPboxdst.GetSubVolume(writebox).data.ToPointer());
                }
            }
            texImage.UnlockBits(bmd);
            texture.GetBuffer().Unlock();
        }

        public static Bitmap MogreImageToBitmap(Mogre.Image img)
        {
            unsafe
            {
                Bitmap bm = new Bitmap((int)img.Width, (int)img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.Drawing.Imaging.BitmapData bmd = bm.LockBits(new System.Drawing.Rectangle(0, 0, (int)img.Width, (int)img.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bm.PixelFormat);
                int PixelSize = 4;

                for (int y = 0; y < bmd.Height; y++)
                {
                    byte* row = (byte*)bmd.Scan0 + (y * bmd.Stride);
                    for (int x = 0; x < bmd.Width; x++)
                    {
                        Mogre.ColourValue color = img.GetColourAt(x, y, 0);
                        row[x * PixelSize] = (byte)(color.b * 255);
                        row[x * PixelSize + 1] = (byte)(color.g * 255);
                        row[x * PixelSize + 2] = (byte)(color.r * 255);
                        row[x * PixelSize + 3] = (byte)(color.a * 255);
                    }
                }
                bm.UnlockBits(bmd);
                return bm;
            }
        }

        public class ConvMatrix
        {
            public int TopLeft = 0, TopMid = 0, TopRight = 0;
            public int MidLeft = 0, Pixel = 1, MidRight = 0;
            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
            public int Factor = 1;
            public int Offset = 0;
            public void SetAll(int nVal)
            {
                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight =
                          BottomLeft = BottomMid = BottomRight = nVal;
            }
        }

        public static bool Conv3x3(Bitmap b, ConvMatrix m)
        {
            // Avoid divide by zero errors

            if (0 == m.Factor)
                return false; Bitmap

            // GDI+ still lies to us - the return format is BGR, NOT RGB. 

            bSrc = (Bitmap)b.Clone();
            System.Drawing.Imaging.BitmapData bmData = b.LockBits(new System.Drawing.Rectangle(0, 0, b.Width, b.Height),
                                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            System.Drawing.Imaging.BitmapData bmSrc = bSrc.LockBits(new System.Drawing.Rectangle(0, 0, bSrc.Width, bSrc.Height),
                               System.Drawing.Imaging.ImageLockMode.ReadWrite,
                               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            int stride2 = stride * 2;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;
                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) +
                            (pSrc[5] * m.TopMid) +
                            (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) +
                            (pSrc[5 + stride] * m.Pixel) +
                            (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) +
                            (pSrc[5 + stride2] * m.BottomMid) +
                            (pSrc[8 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) +
                            (pSrc[4] * m.TopMid) +
                            (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) +
                            (pSrc[4 + stride] * m.Pixel) +
                            (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) +
                            (pSrc[4 + stride2] * m.BottomMid) +
                            (pSrc[7 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) +
                                       (pSrc[3] * m.TopMid) +
                                       (pSrc[6] * m.TopRight) +
                                       (pSrc[0 + stride] * m.MidLeft) +
                                       (pSrc[3 + stride] * m.Pixel) +
                                       (pSrc[6 + stride] * m.MidRight) +
                                       (pSrc[0 + stride2] * m.BottomLeft) +
                                       (pSrc[3 + stride2] * m.BottomMid) +
                                       (pSrc[6 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return true;
        }

        private void OLD_blurFilter(Mogre.TexturePtr texture)
        {
            unsafe
            {
                uint texWidth = customTextures[activeTexture].Width;
                uint texHeight = customTextures[activeTexture].Height;
                HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox texEdit = meshTexBuffer.CurrentLock;
                for (uint x = 0; x < texWidth; x++)
                {
                    for (uint y = 0; y < texHeight; y++)
                    {
                        float sumR = 0;
                        float sumG = 0;
                        float sumB = 0;
                        int numPixels = 0;
                        ColourValue baseColor = new ColourValue();
                        Box writebox;
                        if (x > 0)
                        {
                            if (y > 0)
                            {
                                numPixels++;
                                writebox = new Box(x - 1, y - 1, x - 1, y - 1);
                                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                                sumR += baseColor.r;
                                sumG += baseColor.g;
                                sumB += baseColor.b;
                            }
                            numPixels++;
                            writebox = new Box(x - 1, y, x - 1, y);
                            PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                            sumR += baseColor.r;
                            sumG += baseColor.g;
                            sumB += baseColor.b;


                            if (y < texHeight - 1)
                            {
                                numPixels++;
                                writebox = new Box(x - 1, y + 1, x - 1, y + 1);
                                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                                sumR += baseColor.r;
                                sumG += baseColor.g;
                                sumB += baseColor.b;

                            }
                        }
                        if (y > 0)
                        {
                            numPixels++;
                            writebox = new Box(x, y - 1, x, y - 1);
                            PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                            sumR += baseColor.r;
                            sumG += baseColor.g;
                            sumB += baseColor.b;
                            
                            if (x < texWidth - 1)
                            {
                                numPixels++;
                                writebox = new Box(x + 1, y - 1, x + 1, y - 1);
                                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                                sumR += baseColor.r;
                                sumG += baseColor.g;
                                sumB += baseColor.b;
                            }
                        }

                        if (y < texHeight - 1)
                        {
                            numPixels++;
                            writebox = new Box(x, y + 1, x, y + 1);
                            PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                            sumR += baseColor.r;
                            sumG += baseColor.g;
                            sumB += baseColor.b;

                            if (x < texWidth - 1)
                            {
                                numPixels++;
                                writebox = new Box(x + 1, y + 1, x + 1, y + 1);
                                PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                                sumR += baseColor.r;
                                sumG += baseColor.g;
                                sumB += baseColor.b;
                            }
                        }
                        

                        if (x < texWidth - 1)
                        {
                            numPixels++;
                            writebox = new Box(x + 1, y, x + 1, y);
                            PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                            sumR += baseColor.r;
                            sumG += baseColor.g;
                            sumB += baseColor.b;
                        }

                        numPixels++;
                        writebox = new Box(x, y, x, y);
                        PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                        sumR += baseColor.r;
                        sumG += baseColor.g;
                        sumB += baseColor.b;

                        baseColor.r = sumR / numPixels;
                        baseColor.g = sumG / numPixels;
                        baseColor.b = sumB / numPixels;

                        PixelUtil.PackColour(baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                    }
                }
                meshTexBuffer.Unlock();
            }
        }

        private void brightnessFilter(Mogre.TexturePtr texture, float value)
        {
            unsafe
            {
                uint texWidth = customTextures[activeTexture].Width;
                uint texHeight = customTextures[activeTexture].Height;
                HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox texEdit = meshTexBuffer.CurrentLock;
                for (uint x = 0; x < texWidth; x++)
                {
                    for (uint y = 0; y < texHeight; y++)
                    {
                        Box writebox = new Box(x, y, x, y);

                        ColourValue baseColor = new ColourValue();

                        PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                        ColourValue drawColor = new ColourValue();
                        drawColor.a = baseColor.a;
                        drawColor.r = baseColor.r + value;
                        if (drawColor.r > 1)
                        {
                            drawColor.r = 1;
                        }
                        else if (drawColor.r < 0)
                        {
                            drawColor.r = 0;
                        }
                        drawColor.b = baseColor.b + value;
                        if (drawColor.b > 1)
                        {
                            drawColor.b = 1;
                        }
                        else if (drawColor.b < 0)
                        {
                            drawColor.b = 0;
                        }
                        drawColor.g = baseColor.g + value;
                        if (drawColor.g > 1)
                        {
                            drawColor.g = 1;
                        }
                        else if (drawColor.g < 0)
                        {
                            drawColor.g = 0;
                        }

                        PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                    }
                }
                meshTexBuffer.Unlock();
            }
        }

        private void contrastFilter(Mogre.TexturePtr texture, float value)
        {
            value++;
            unsafe
            {
                uint texWidth = customTextures[activeTexture].Width;
                uint texHeight = customTextures[activeTexture].Height;
                HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox texEdit = meshTexBuffer.CurrentLock;
                for (uint x = 0; x < texWidth; x++)
                {
                    for (uint y = 0; y < texHeight; y++)
                    {
                        Box writebox = new Box(x, y, x, y);

                        ColourValue baseColor = new ColourValue();

                        PixelUtil.UnpackColour(&baseColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                        ColourValue drawColor = new ColourValue();
                        drawColor.a = baseColor.a;
                        drawColor.r = baseColor.r - 0.5f;
                        drawColor.r *= value;
                        drawColor.r += 0.5f;
                        if (drawColor.r < 0) drawColor.r = 0;
                        if (drawColor.r > 255) drawColor.r = 255;

                        drawColor.g = baseColor.g - 0.5f;
                        drawColor.g *= value;
                        drawColor.g += 0.5f;
                        if (drawColor.g < 0) drawColor.g = 0;
                        if (drawColor.g > 255) drawColor.g = 255;

                        drawColor.b = baseColor.b - 0.5f;
                        drawColor.b *= value;
                        drawColor.b += 0.5f;
                        if (drawColor.b < 0) drawColor.b = 0;
                        if (drawColor.b > 255) drawColor.b = 255;

                        PixelUtil.PackColour(drawColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());
                    }
                }
                meshTexBuffer.Unlock();
            }
        }

        public string createDrawTexture(string name)
        {
            string drawTexName = "drawTex_" + name;
            if (!name.StartsWith("drawTex_") && !TextureManager.Singleton.ResourceExists(drawTexName))
            {
                LogManager.Singleton.LogMessage("Creating a new draw-texture for: " + name);
                TexturePtr originalTexture = TextureManager.Singleton.GetByName(name);
                customTextures.Add(TextureManager.Singleton.CreateManual(drawTexName,
                    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                    originalTexture.Width, originalTexture.Height, 0, originalTexture.Format, (int)TextureUsage.TU_DYNAMIC));
                originalTexture.CopyToTexture(customTextures[customTextures.Count - 1]);

                LogManager.Singleton.LogMessage("Created texture: " + customTextures[customTextures.Count - 1].Name);
            }
            else
            {
                if (name.StartsWith("drawTex_"))
                {
                    drawTexName = name;
                }
                LogManager.Singleton.LogMessage("Skipping draw-texture creation - it already exists.");
            }

            return drawTexName;
        }

        public void saveCurrentTextureAs( string path )
        {
            LogManager.Singleton.LogMessage("Saving File...");
            unsafe
            {
                HardwarePixelBufferSharedPtr readbuffer;
                readbuffer = customTextures[activeTexture].GetBuffer(0, 0);
                readbuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox readrefpb = readbuffer.CurrentLock;
                byte* readrefdata = (byte*)readrefpb.data;

                Mogre.Image img = new Mogre.Image();
                img = img.LoadDynamicImage(readrefdata, customTextures[activeTexture].Width,
                customTextures[activeTexture].Height, customTextures[activeTexture].Format);
                img.Save(path);

                readbuffer.Unlock();
            }
        }

        void preRenderTargetUpdateX(RenderTargetEvent_NativePtr evt)
        {
            if (OgreForm.mCurrentEntity != null)
            {

                for (uint i = 0; i < OgreForm.mCurrentEntity.NumSubEntities; i++)
                {
                    materials.Add(OgreForm.mCurrentEntity.GetSubEntity(i).MaterialName);
                    OgreForm.mCurrentEntity.GetSubEntity(i).MaterialName = "uvEncodingMatX";
                }
            }
#if DEBUG
            mMiniScreenNode.SetVisible(false);
#endif
        }

        void postRenderTargetUpdateX(RenderTargetEvent_NativePtr evt)
        {
            if (OgreForm.mCurrentEntity != null)
            {
                for (uint i = 0; i < OgreForm.mCurrentEntity.NumSubEntities; i++)
                {
                    OgreForm.mCurrentEntity.GetSubEntity(i).MaterialName = materials[(int)i];
                }
                materials.Clear();
            }
#if DEBUG
            mMiniScreenNode.SetVisible(true);
#endif
        }

        public string getCurrentTextureName()
        {
            return customTextures[activeTexture].Name;
        }
    }
}
