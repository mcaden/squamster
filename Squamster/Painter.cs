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
        public  List<TexturePtr> customTextures = new List<TexturePtr>();
        public  int activeTexture = -1;
        public TexturePtr uvEncodedTexture;
        StringVector materials = new StringVector();

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
                else if (mouseY > OgreForm.mWindow.Width)
                    mouseY = (int)OgreForm.mWindow.Width;

                unsafe
                {
                    HardwarePixelBufferSharedPtr buffer = uvEncodedTexture.GetBuffer();
                    buffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                    ColourValue uvColorValue = new ColourValue();
                    PixelBox pbox = buffer.CurrentLock;

                    Box writebox = new Box((uint)mouseX, (uint)mouseY, (uint)mouseX + 1, (uint)mouseY + 1);
                    PixelUtil.UnpackColour(&uvColorValue, PixelFormat.PF_FLOAT16_RGBA, pbox.GetSubVolume(writebox).data.ToPointer());

                    //LogManager.Singleton.LogMessage("UV color: " + uvColorValue.r.ToString() + "," + uvColorValue.g.ToString() + "," + uvColorValue.b.ToString() + "," + uvColorValue.a.ToString());

                    if (!(uvColorValue.b > 0))
                    {
                        previewPoint.X = uvColorValue.r;
                        previewPoint.Y = uvColorValue.g;
                        uvX = (uvColorValue.r * (customTextures[activeTexture].Width - 1));
                        uvY = (uvColorValue.g * (customTextures[activeTexture].Height - 1));

                        //LogManager.Singleton.LogMessage(" UV Coords: " + uvX.ToString() + ", " + uvY.ToString() + " - On size: " + customTextures[activeTexture].Width.ToString() + ", " + customTextures[activeTexture].Height.ToString());

                        HardwarePixelBufferSharedPtr meshTexBuffer = customTextures[activeTexture].GetBuffer();
                        meshTexBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                        PixelBox texEdit = meshTexBuffer.CurrentLock;

                        writebox = new Box((uint)uvX, (uint)uvY, (uint)uvX + 1, (uint)uvY + 1);
                        PixelUtil.PackColour( penColor, customTextures[activeTexture].Format, texEdit.GetSubVolume(writebox).data.ToPointer());

                        meshTexBuffer.Unlock();                    
                    }
                    buffer.Unlock();
                }
            } 
            return previewPoint;
        }

        public void setActiveTexture(string texture)
        {
            string drawTexName = "drawTex_" + texture;
            if (ResourceGroupManager.Singleton.ResourceExists(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, drawTexName))
            {
                bool textureSelected = false;
                for (int i = 0; i < customTextures.Count && !textureSelected; i++)
                {
                    if (customTextures[i].Name == drawTexName)
                    {
                        activeTexture = i;
                        textureSelected = true;
                    }
                }
                LogManager.Singleton.LogMessage("Now drawing on texture: " + customTextures[activeTexture].Name);
            }
            else
            {
                TexturePtr originalTexture = TextureManager.Singleton.GetByName(texture);
                customTextures.Add(TextureManager.Singleton.CreateManual("drawTex_" + texture,
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

        public void saveCurrentTexture()
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
                img.Save("test.png");

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
    }
}
