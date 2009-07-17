/*************************************************************************

This file is part of Squamster - An Ogre /mesh viewer/painter for windows.

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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Mogre;
using MOIS;

namespace Squamster
{
    public partial class OgreForm : Form
    {

#if INSTALL
        const string brushPath = "./media/brushes/";
#else
        const string brushPath = "../../media/brushes/";
#endif

        public static Root mRoot = null;
        public static SceneManager mSceneMgr = null;
        public static Entity mCurrentEntity = null;

        public static RenderWindow mWindow;
        public static ExtendedCamera ExCamera;
        bool mShutdownRequested = false;
        bool busy = false;

        Painter meshPainter;
        MeshLoader meshLoader;
        
        AnimationState mAnimState = null;
        bool playAnim = false;        

        protected MOIS.InputManager inputManager;
        protected MOIS.Keyboard inputKeyboard;
        protected MOIS.Mouse inputMouse;
        bool mouseInPanel1 = false;
        bool actionPerformed = false;

        List<byte[]> undoList = new List<byte[]>();
        List<byte[]> redoList = new List<byte[]>();

        Mogre.Timer frameTimer = new Mogre.Timer();

        public OgreForm()
        {
            InitializeComponent();

            Disposed += new EventHandler(OgreForm_Disposed);
            Resize += new EventHandler(OgreForm_Resize);
        }

        void OgreForm_Resize(object sender, EventArgs e)
        {
            mWindow.WindowMovedOrResized();
        }

        void OgreForm_Disposed(object sender, EventArgs e)
        {
            meshPainter.dispose();
            mRoot.Dispose();
            mRoot = null;
        }

        public void Go()
        {
            Show();
            while (mRoot != null && !mShutdownRequested)
            {
                try
                {
                    System.Threading.Thread.Sleep(0);
                    this.update();
                    Application.DoEvents();
                }
                catch (Exception e)
                {
                    LogManager.Singleton.LogMessage( "!!! EXCEPTION !!!" + e.ToString());
                }
            }
        }

        public void Init()
        {
            
            // Create root object
            mRoot = new Root();
            // Define Resources
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            String secName, typeName, archName;

            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }

            // Setup RenderSystem
            RenderSystem rs = mRoot.GetRenderSystemByName("OpenGL Rendering Subsystem");
            // or use "OpenGL Rendering Subsystem"
            mRoot.RenderSystem = rs;
            rs.SetConfigOption("Full Screen", "No");
            rs.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
            rs.SetConfigOption("VSync", "true");

            // Create Render Window
            mRoot.Initialise(false, "Squamster");
            NameValuePairList misc = new NameValuePairList();
            misc["externalWindowHandle"] = this.splitContainer1.Panel1.Handle.ToString();
            mWindow = mRoot.CreateRenderWindow("Main RenderWindow", 800, 600, false, misc.ReadOnlyInstance);

            //misc = new NameValuePairList();
            //misc["externalWindowHandle"] = this.Handle.ToString();
#if DEBUG
            LogManager.Singleton.SetLogDetail(LoggingLevel.LL_BOREME);
#else
            LogManager.Singleton.SetLogDetail(LoggingLevel.LL_NORMAL);
#endif
            LogManager.Singleton.LogMessage("*** Initializing MOIS ***");
            MOIS.ParamList pl = new MOIS.ParamList();
            //IntPtr windowHnd;
            //mWindow.GetCustomAttribute("WINDOW", out windowHnd);
            pl.Insert("WINDOW", this.Handle.ToString());
            pl.Insert("w32_mouse", "DISCL_FOREGROUND");
            pl.Insert("w32_mouse", "DISCL_NONEXCLUSIVE");
            inputManager = MOIS.InputManager.CreateInputSystem(pl);

            LogManager.Singleton.LogMessage("*** MOIS: InputManager created ***");

            // Create all devices (except joystick, as most people have Keyboard/Mouse) using buffered input.
            inputKeyboard = (MOIS.Keyboard)inputManager.CreateInputObject(MOIS.Type.OISKeyboard, true);
            inputMouse = (MOIS.Mouse)inputManager.CreateInputObject(MOIS.Type.OISMouse, true);

            LogManager.Singleton.LogMessage("*** MOIS: Devices created ***");

            MOIS.MouseState_NativePtr state = inputMouse.MouseState;
            state.width = (int)mWindow.Width;
            state.height = (int)mWindow.Height;

            LogManager.Singleton.LogMessage("*** MOIS: Setup Completed ***");

            // Create a Simple Scene
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC, "sceneMgr");
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
            meshLoader = new MeshLoader();
            Camera cam = mSceneMgr.CreateCamera("Camera");
            cam.NearClipDistance = .01f;

            mWindow.AddViewport(cam);

            ExCamera = new ExtendedCamera("myExtCam", "sceneMgr", "Camera");

            // Init resources
            TextureManager.Singleton.DefaultNumMipmaps = 0;
            

            if (inputKeyboard != null)
            {
                LogManager.Singleton.LogMessage("Setting up keyboard listeners");
                inputKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(KeyPressed);
                inputKeyboard.KeyReleased += new MOIS.KeyListener.KeyReleasedHandler(KeyReleased);
            }
            if (inputMouse != null)
            {
                LogManager.Singleton.LogMessage("Setting up mouse listeners");
                inputMouse.MousePressed += new MOIS.MouseListener.MousePressedHandler(MousePressed);
                inputMouse.MouseReleased += new MOIS.MouseListener.MouseReleasedHandler(MouseReleased);
                inputMouse.MouseMoved += new MOIS.MouseListener.MouseMovedHandler(MouseMotion);
            }

            meshPainter = new Painter();
            colorDialog1.Color = Color.FromArgb((int)Painter.penColor.GetAsARGB());
            colorSelector.BackColor = colorDialog1.Color;

            System.IO.DirectoryInfo brushDir = new System.IO.DirectoryInfo(brushPath);
            System.IO.FileInfo[] brushFiles = brushDir.GetFiles("*.png");
            for (int i = 0; i < brushFiles.Length; i++ )
            {
                System.Drawing.Bitmap newBrush = new Bitmap(brushPath + brushFiles[i].Name);
                brushPreviewList.Images.Add(brushFiles[i].Name, newBrush);
                brushList.Items.Add("Brush " + i.ToString(), brushFiles[i].Name);
                brushList.Items[i].Selected = false;
                meshPainter.addBrush(newBrush);
            }
            brushList.Select();
            brushList.SelectedItems.Clear();
            brushList.Items[0].Selected = true;
            meshPainter.currentBrush = 0;
            
            setViewMode(null, null);

            meshPainter.BrushScale = (float)brushScaleControl.Value;
            meshPainter.BrushOpacity = (float)brushOpacityControl.Value;

            frameTimer.Reset();            
        }

        /// <summary>
        /// Finds all animations available to an entity and places them in the animation dropdown box.
        /// </summary>
        /// <param name="ent">The Ogre Entity to find animation for</param>
        /// <returns>The number of animations found</returns>
        private int populateAnims( Entity ent)
        {
            animBox.Items.Clear();
            int animCount = 0;
            AnimationStateIterator itr = ent.AllAnimationStates.GetAnimationStateIterator();
            while (itr.MoveNext())
            {
                animBox.Items.Add(itr.Current.AnimationName);
                animCount++;
            }

            disableAnimControls();

            return animCount;
        }
        /// <summary>
        /// Disables the animation controls.
        /// </summary>
        private void disableAnimControls()
        {
            Btn_Anim_Stop.Enabled = false;
            Btn_Anim_Play.Enabled = false;
            loopAnim.Enabled = false;
            playAnim = false;
        }
        
        



#region Events

        private void update()
        {
            float timeElapsed = ((float)frameTimer.Microseconds) / 1000000;
            frameTimer.Reset();

            if (mAnimState != null && playAnim)
            {
                mAnimState.AddTime( timeElapsed); //convert from milliseconds to seconds.
                if (mAnimState.HasEnded && !mAnimState.Loop)
                {
                    Btn_Anim_Stop_Click(null, null);
                }
            }



            // Capture all key presses since last check.
            inputKeyboard.Capture();
            // Capture all mouse movements and button presses since last check.
            inputMouse.Capture();
            ExCamera.update();

            MouseButtons currentMouseButtons = System.Windows.Forms.Control.MouseButtons;

            if (currentMouseButtons != MouseButtons.Left && currentMouseButtons != MouseButtons.Right && currentMouseButtons != MouseButtons.Middle)
            {
                if( inputKeyboard.IsKeyDown(KeyCode.KC_LCONTROL) || inputKeyboard.IsKeyDown(KeyCode.KC_RCONTROL))
                {
                    if (inputKeyboard.IsKeyDown(KeyCode.KC_S))
                    {
                        if (!actionPerformed)
                        {
                            actionPerformed = true;
                            Cursor = Cursors.WaitCursor;
                            saveCurrentTexture();
                            Cursor = Cursors.Default;
                        }
                    }
                    else if( inputKeyboard.IsKeyDown(KeyCode.KC_Z))
                    {
                        if (!actionPerformed)
                        {
                            undo();
                            actionPerformed = true;
                        }
                    }
                    else if (inputKeyboard.IsKeyDown(KeyCode.KC_Y))
                    {
                        if (!actionPerformed)
                        {
                            redo();
                            actionPerformed = true;
                        }
                    }
                    else
                    {
                        actionPerformed = false;
                    }
                }
            }

            if (busy)
            {
                if (meshPainter.paintMode == Painter.PaintModes.TOOLS && meshPainter.currentTool == Painter.Tools.SHAPE)
                {
                    Point mousePos = PointToClient(System.Windows.Forms.Control.MousePosition);
                    Point pos = new Point(mousePos.X, mousePos.Y - menuStrip1.Height);
                    drawShapePreview(meshPainter.shapeOrigin, pos);
                }
            }

            if ( !mRoot.RenderOneFrame() )
            {
                mShutdownRequested = true;
            }
        }

        
        

    #region Panel1

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            mouseInPanel1 = true;
            //splitContainer1.Panel1.Focus();
        }
        private void splitContainer1_Panel1_MouseLeave(object sender, EventArgs e)
        {
            mouseInPanel1 = false;
        }

    #endregion
    #region Meshes
        
        /// <summary>
        /// Adds a mesh's name to the mesh listbox
        /// </summary>
        /// <param name="mesh">The name of the mesh to add to the list.</param>
        private void addMeshToList( String mesh )
        {
            LogManager.Singleton.LogMessage("Adding mesh: " + mesh + " to list...");
            meshListBox.Items.Add(mesh);
            LogManager.Singleton.LogMessage("Mesh added to list!");
        }
        private void addMeshToList(StringVector meshes)
        {
            for( int i = 0; i < meshes.Count; i++ )
            {
                addMeshToList(meshes[i]);
            }
        }
        private void selectMesh(object sender, EventArgs e)
        {
            LogManager.Singleton.LogMessage("-= Event: Mesh Selection =-");
            //Stats
            String meshName = "";
            StringVector textureNames = new StringVector();
            StringVector materialNames = new StringVector();
            int animCount = 0;

            //Hide nodes that aren't selected, show the node that is
            Node.ChildNodeIterator itr = mSceneMgr.RootSceneNode.GetChildIterator();
            while (itr.MoveNext())
            { 
                SceneNode node = (SceneNode)itr.Current;
                if( node.Name != meshListBox.SelectedItem.ToString() )
                {
                    node.SetVisible(false);
                }
                else
                {
                    LogManager.Singleton.LogMessage("Active mesh found...");
                    Entity ent = (Entity)node.GetAttachedObject( node.Name );
                    mCurrentEntity = ent;
                    LogManager.Singleton.LogMessage("Resetting Animations...");
                    if (mAnimState != null)
                    {
                        mAnimState.TimePosition = 0;
                        mAnimState.Enabled = false;
                        mAnimState = null;
                    }
                    animBox.Items.Clear();                
                    
                    node.SetVisible(true);
                    meshName = node.Name;
                    if (ent.HasSkeleton)
                    {
                        LogManager.Singleton.LogMessage("Populating Animations...");
                        animCount = populateAnims( ent );
                    }
                    else
                        LogManager.Singleton.LogMessage("No Animations...Moving on");

                    LogManager.Singleton.LogMessage("Getting Materials, Textures...");
                    for (uint i = 0; i < ent.NumSubEntities; i++)
                    {
                        Material mat = ent.GetSubEntity(i).GetMaterial();
                        materialNames.Add(mat.Name);
                        Material.TechniqueIterator techniqueItr = mat.GetTechniqueIterator();
                        while (techniqueItr.MoveNext())
                        {
                            Technique.PassIterator passItr = techniqueItr.Current.GetPassIterator();
                            while (passItr.MoveNext())
                            {
                                Pass.TextureUnitStateIterator texItr = passItr.Current.GetTextureUnitStateIterator();
                                while (texItr.MoveNext())
                                {
                                    texItr.Current.SetTextureName(meshPainter.createDrawTexture(texItr.Current.TextureName));
                                    textureNames.Add(texItr.Current.TextureName);
                                }
                            }
                        }
                    }

                    LogManager.Singleton.LogMessage("Removing duplicate textures");
                    //remove duplicate texture files - same texture in different passes
                    for (int i = 0; i < textureNames.Count; i++)
                    {
                        for (int j = i + 1; j < textureNames.Count; j++)
                        { 
                            if( textureNames[j] == textureNames[i] )
                            {
                                textureNames.Erase( j, textureNames.Count - j);
                                j--;

                            }
                        }
                    }  
                    ExCamera.recenterCamera(ent.BoundingBox.Center);
                }
            }
            
            if (meshName.Length > 0)
            {
                //Mesh statistics
                LogManager.Singleton.LogMessage("Compiling mesh stats...");
                
                String textureNameOutput = "";
                String materialNameOutput = "";
                foreach( String textureName in textureNames )
                {
                    textureNameOutput += "\nTexture Name: " + textureName;
                }
                foreach (String material in materialNames)
                {
                    materialNameOutput += "\nMaterial Name: " + material;
                }
                statsLabel.Text = "Name: " + meshName + "\nAnimation Count: " + animCount.ToString()
                        + materialNameOutput + textureNameOutput;

                LogManager.Singleton.LogMessage("Adding Textures...");
                texturePreviewList.Images.Clear();
                texList.Items.Clear();
                if (textureNames.Count > 0)
                {
                    
                    foreach (String textureName in textureNames)
                    {
                        LogManager.Singleton.LogMessage("Fetching the preview texture '" + textureName + "' from Ogre...");
                        unsafe
                        {
                            TexturePtr currentTex = TextureManager.Singleton.GetByName(textureName);
                            currentTex.GetBuffer().Lock( HardwareBuffer.LockOptions.HBL_READ_ONLY );
                            PixelBox mPboxdst = currentTex.GetBuffer().CurrentLock;
                            Mogre.Image mImage = new Mogre.Image();
                            LogManager.Singleton.LogMessage("Loading texture data into image");
                            mImage.LoadDynamicImage((byte*)mPboxdst.data.ToPointer(), currentTex.Width, currentTex.Height, currentTex.Format);
                            currentTex.GetBuffer().Unlock();
                            LogManager.Singleton.LogMessage("Creating preview image");
                            System.Drawing.Image texImage = Painter.MogreImageToBitmap(mImage);
                            LogManager.Singleton.LogMessage("Registering image");
                            texturePreviewList.Images.Add(currentTex.Name, texImage);
                            LogManager.Singleton.LogMessage("Adding image to list...");
                            texList.Items.Add(currentTex.Name.Substring( currentTex.Name.IndexOf("_") + 1));
                            if (texList.Items.Count == 1)
                            {
                                texList.SelectedIndex = 0;
                            }
                            LogManager.Singleton.LogMessage("Texture added!");
                        }
                    }
                    if (texList.Items.Count == 0)//Seems textures within zip files won't load
                    {
                        pictureBox1.Image = pictureBox1.BackgroundImage;
                    }
                }
                else
                {
                    pictureBox1.Image = pictureBox1.BackgroundImage;
                }
            }
            LogManager.Singleton.LogMessage("-= Mesh Selection Complete =-");
        }
        private void Btn_LoadAll_Click(object sender, EventArgs e)
        {
            LogManager.Singleton.LogMessage("-= Loading all meshes =-");
            Cursor = Cursors.WaitCursor;
            bool autoSelectNewMesh = false;
            if (meshListBox.Items.Count == 0)
            {
                autoSelectNewMesh = true;
            }
            LogManager.Singleton.LogMessage("Getting all meshes...");
            StringVector meshesAdded = meshLoader.createAllMeshesFromResourceSystem();
            LogManager.Singleton.LogMessage("All meshes obtained.");
            addMeshToList(meshesAdded);
            LogManager.Singleton.LogMessage("Added all meshes to list.");
            if (autoSelectNewMesh)
            {
                LogManager.Singleton.LogMessage("Mesh list was previously empty, setting selected mesh...");
                meshListBox.SelectedIndex = 0;
            }
            Cursor = Cursors.Default;
            LogManager.Singleton.LogMessage("-= Loading all meshes: Finished =-");
        }
        private void Btn_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog openMeshDialog = new OpenFileDialog();
            openMeshDialog.CheckPathExists = true;
            openMeshDialog.CheckFileExists = true;
            openMeshDialog.Title = "Load new mesh";
            openMeshDialog.Multiselect = true;
            openMeshDialog.AddExtension = false;
            openMeshDialog.DefaultExt = ".mesh";
            openMeshDialog.Filter = "Ogre Meshes (*.mesh)|*.mesh";
            DialogResult userResult = openMeshDialog.ShowDialog();

            if( userResult == DialogResult.OK )
            {
                Cursor = Cursors.WaitCursor;
                bool autoSelectNewMesh = false;
                if (meshListBox.Items.Count == 0)
                {
                    autoSelectNewMesh = true;
                }
                for( int i = 0; i < openMeshDialog.FileNames.Length; i++)
                {
                    String mesh = meshLoader.createMeshFromFile(openMeshDialog.FileNames[i].Substring(0, openMeshDialog.FileNames[i].LastIndexOf("\\")), openMeshDialog.SafeFileNames[i]);
                    if (mesh.Length > 0)
                    {
                        addMeshToList(mesh);
                    }
                }
                if (autoSelectNewMesh)
                {
                    meshListBox.SelectedIndex = 0;
                }
                Cursor = Cursors.Default;
            }
        }
        private void updateTexturePreview()
        {
            if (texList.Items.Count > 0)
            {
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "-= Updating Preview Image =-");
                unsafe
                {
                    TexturePtr currentTex = TextureManager.Singleton.GetByName( meshPainter.getCurrentTextureName() );
                    currentTex.GetBuffer().Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                    PixelBox mPboxdst = currentTex.GetBuffer().CurrentLock;
                    Mogre.Image mImage = new Mogre.Image();
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Loading texture data into Mogre image...");
                    mImage.LoadDynamicImage((byte*)mPboxdst.data.ToPointer(), currentTex.Width, currentTex.Height, currentTex.Format);
                    currentTex.GetBuffer().Unlock();
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Creating preview image...");
                    System.Drawing.Image texImage = Painter.MogreImageToBitmap(mImage);
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Replacing preview...");
                    pictureBox1.Image = texImage;
                }
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "-= Preview Image Updated =-");
            }
        }
        private void addToUndo()
        {
            if (texList.Items.Count > 0)
            {
                if (undoList.Count >= 20)
                {
                    undoList.RemoveAt(undoList.Count - 1);
                }
                unsafe
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Saving Undo Snapshot...");
                    TexturePtr currentTex = TextureManager.Singleton.GetByName(meshPainter.getCurrentTextureName());
                    HardwarePixelBuffer mBuffer = currentTex.GetBuffer();
                    mBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                    byte[] frame = new byte[currentTex.Width * currentTex.Height * 4];
                    PixelBox pBox = mBuffer.CurrentLock;
                    Marshal.Copy(pBox.data, frame, 0, frame.Length);
                    undoList.Insert(0, frame);
                    mBuffer.Unlock();
                }
                LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "-= Undo List Updated =-");
                if (redoList.Count > 0)
                {
                    redoList.Clear();
                }
            }
        }

        private void undo()
        {
            if (texList.Items.Count > 0 && undoList.Count > 0)
            {
                LogManager.Singleton.LogMessage("-= Undoing Action =-");
                unsafe
                {
                    LogManager.Singleton.LogMessage( LogMessageLevel.LML_TRIVIAL, "Saving Redo Snapshot...");
                    TexturePtr currentTex = TextureManager.Singleton.GetByName(meshPainter.getCurrentTextureName());
                    HardwarePixelBuffer mBuffer = currentTex.GetBuffer();
                    mBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                    byte[] frame = new byte[currentTex.Width * currentTex.Height * 4];
                    PixelBox pBox = mBuffer.CurrentLock;
                    Marshal.Copy(pBox.data, frame, 0, frame.Length);
                    redoList.Insert(0, frame);

                    Marshal.Copy(undoList[0], 0, pBox.data, undoList[0].Length);
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Removing snapshot from list...");
                    undoList.RemoveAt(0);
                    mBuffer.Unlock();
                }
                updateTexturePreview();
                LogManager.Singleton.LogMessage("-= Undo Finished - Undo history currently contains: " + undoList.Count.ToString() + " actions =-");
            }
        }

        private void redo()
        {
            if (texList.Items.Count > 0 && redoList.Count > 0)
            {
                LogManager.Singleton.LogMessage("-= Redoing Action =-");
                unsafe
                {
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Saving Undo Snapshot...");
                    TexturePtr currentTex = TextureManager.Singleton.GetByName(meshPainter.getCurrentTextureName());
                    HardwarePixelBuffer mBuffer = currentTex.GetBuffer();
                    mBuffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                    byte[] frame = new byte[currentTex.Width * currentTex.Height * 4];
                    PixelBox pBox = mBuffer.CurrentLock;
                    Marshal.Copy(pBox.data, frame, 0, frame.Length);
                    undoList.Insert(0, frame);

                    Marshal.Copy(redoList[0], 0, pBox.data, undoList[0].Length);
                    LogManager.Singleton.LogMessage(LogMessageLevel.LML_TRIVIAL, "Removing snapshot from list...");
                    redoList.RemoveAt(0);
                    mBuffer.Unlock();
                }
                updateTexturePreview();
                LogManager.Singleton.LogMessage("-= Redo Finished - Undo history currently contains: " + undoList.Count.ToString() + " actions =-");
            }
        }

        

    #endregion        
    #region Animations
        
        private void selectAnim(object sender, EventArgs e)
        {
            if (animBox.SelectedItem != null)
            {
                if (mAnimState != null)
                {
                    if (mAnimState.AnimationName != animBox.SelectedText)
                    {
                        mAnimState.TimePosition = 0;
                        mAnimState.Enabled = false;
                    }
                }
                String meshName = meshListBox.SelectedItem.ToString();
                Entity ent = (Entity)mSceneMgr.GetSceneNode( meshName ).GetAttachedObject( meshName );
                mAnimState = ent.GetAnimationState(animBox.SelectedItem.ToString());
                mAnimState.Enabled = true;
                loopAnim.Checked = mAnimState.Loop;
                loopAnim.Enabled = true;
                Btn_Anim_Stop_Click(sender, e);
            }
        }
        private void Btn_Anim_Stop_Click(object sender, EventArgs e)
        {
            playAnim = false;
            Btn_Anim_Play.Enabled = true;
            Btn_Anim_Stop.Enabled = false;
        }
        private void Btn_Anim_Play_Click(object sender, EventArgs e)
        {
            playAnim = true;
            Btn_Anim_Play.Enabled = false;
            Btn_Anim_Stop.Enabled = true;
            if (mAnimState.HasEnded)
            {
                mAnimState.TimePosition = 0;
            }
        }
        private void loopAnim_MouseClick(object sender, MouseEventArgs e)
        {
            mAnimState.Loop = loopAnim.Checked;
        }    
    
    #endregion
#endregion

#region MOISEvents

        public bool MouseMotion(MOIS.MouseEvent e)
        {
            // you can use e.state.Y.rel for reltive position, and e.state.Y.abs for absolute
            if (mouseInPanel1)
            {
                if (System.Windows.Forms.Control.MouseButtons == MouseButtons.Right)
                {
                    if (e.state.X.rel != 0)
                    {
                        ExCamera.cameraYaw(new Degree(-e.state.X.rel));

                    }
                    if (e.state.Y.rel != 0)
                    {
                        ExCamera.cameraPitch(new Degree(e.state.Y.rel));
                    }
                }
                else if (System.Windows.Forms.Control.MouseButtons == MouseButtons.Left)
                {
                    if (meshPainter.paintMode == Painter.PaintModes.TOOLS && meshPainter.currentTool != Painter.Tools.SHAPE)
                    {
                        if (!busy)
                        {
                            busy = true;
                            addToUndo();
                        }
                        Point mousePos = PointToClient(System.Windows.Forms.Control.MousePosition);
                        Point pos = new Point(mousePos.X, mousePos.Y - menuStrip1.Height);
                        drawPreview(meshPainter.draw(pos.X, pos.Y, Painter.penColor));
                    }
                    else if (meshPainter.paintMode == Painter.PaintModes.TOOLS && meshPainter.currentTool == Painter.Tools.SHAPE)
                    {
                        Point mousePos = PointToClient(System.Windows.Forms.Control.MousePosition);
                        Point pos = new Point(mousePos.X, mousePos.Y - menuStrip1.Height);
                        if (!busy)
                        {
                            meshPainter.shapeOrigin = pos;
                            busy = true;
                            addToUndo();
                        }
                        drawShapePreview(meshPainter.shapeOrigin, pos);
                    }
                }
                else if (e.state.Z.rel != 0)
                {
                    ExCamera.cameraZoom(e.state.Z.rel * .1f);
                }
                else if (System.Windows.Forms.Control.MouseButtons == MouseButtons.Middle)
                {
                    ExCamera.pan(new Mogre.Vector3(e.state.X.rel, e.state.Y.rel, 0));
                }
                else
                {
                    if (busy)
                    {
                        busy = false;
                        updateTexturePreview();
                        if (meshPainter.paintMode == Painter.PaintModes.TOOLS && meshPainter.currentTool == Painter.Tools.SHAPE)
                        {
                            Point mousePos = PointToClient(System.Windows.Forms.Control.MousePosition);
                            Point pos = new Point(mousePos.X, mousePos.Y - menuStrip1.Height);
                            meshPainter.drawShape(meshPainter.shapeOrigin, pos);
                        }
                    }
                }
            }
            return true;
        }
        public bool MousePressed(MOIS.MouseEvent e, MOIS.MouseButtonID button)
        {
            switch (button)
            {
                case MOIS.MouseButtonID.MB_Left:
                    break;
            }
            return true;
        }
        public bool MouseReleased(MOIS.MouseEvent e, MOIS.MouseButtonID button)
        {
            return true;
        }
        public bool KeyPressed(MOIS.KeyEvent e)
        {
            if (mouseInPanel1)
            {
                switch (e.key)
                {
                    case MOIS.KeyCode.KC_ESCAPE:
                        break;
                }
            }
            return true;
        }
        public bool KeyReleased(MOIS.KeyEvent e)
        {
            switch (e.key)
            {
                case MOIS.KeyCode.KC_ESCAPE:
                    break;
            }
            return true;
        }

#endregion

#region Texture
        
        private void saveCurrentTexture()
        {
            string path = texturePreviewList.Images.Keys[texList.SelectedIndex];

            LogManager.Singleton.LogMessage("Saving..." );

            meshPainter.saveCurrentTextureAs(path);
        }

        private void drawPreview(PointF drawPoint)
        {
            if (drawPoint.X >= 0 && drawPoint.Y >= 0)
            {
                int picBoxWidth = pictureBox1.Size.Width;
                int picBoxHeight = pictureBox1.Size.Height;

                System.Drawing.Graphics objGraphic = pictureBox1.CreateGraphics();


                if (drawPoint.X >= 0 && drawPoint.Y >= 0 && drawPoint.X <= 1 && drawPoint.Y <= 1)
                {

                    System.Drawing.Pen pen = new Pen(Color.FromArgb((int)Painter.penColor.GetAsARGB()));
                    int x = (int)((float)drawPoint.X * ((float)picBoxWidth - 1));
                    int y = (int)((float)drawPoint.Y * ((float)picBoxHeight - 1));

                    objGraphic.DrawRectangle(pen, x, y, 1, 1);

                    System.Drawing.Drawing2D.GraphicsState graph = objGraphic.Save();
                    objGraphic.Restore(graph);
                }
            }
        }

        private void selectTexture(object sender, EventArgs e)
        {
            undoList.Clear();
            redoList.Clear();
            
            string fullPath = texturePreviewList.Images.Keys[texList.SelectedIndex];
            pictureBox1.Image = texturePreviewList.Images[texList.SelectedIndex];
            string texture = fullPath;

            if (TextureManager.Singleton.ResourceExists( texturePreviewList.Images.Keys[texList.SelectedIndex]))
            {
                LogManager.Singleton.LogMessage("Attempting to set texture:" + texturePreviewList.Images.Keys[texList.SelectedIndex]);
                meshPainter.setActiveTexture(texturePreviewList.Images.Keys[texList.SelectedIndex]);
            }
            else
            {
                LogManager.Singleton.LogMessage("Error: Draw functionality couldn't find texture: " + texturePreviewList.Images.Keys[texList.SelectedIndex]);
            }
        }       
        private void selectColor(object sender, MouseEventArgs e)
        {
            DialogResult colorResult = this.colorDialog1.ShowDialog();
            colorSelector.BackColor = colorDialog1.Color;
            Painter.penColor.SetAsARGB((uint)colorDialog1.Color.ToArgb());
        }
        private void brushScaleControl_ValueChanged(object sender, EventArgs e)
        {
            meshPainter.BrushScale = (float)brushScaleControl.Value;
        }
        private void brushOpacityControl_ValueChanged(object sender, EventArgs e)
        {
            meshPainter.BrushOpacity = (float)brushOpacityControl.Value;
        }
        private void brushList_SelectedIndexChanged(object sender, EventArgs e)
        {
            for( int i = 0; i < brushList.Items.Count; i++ )
            {
                if( brushList.Items[i].Selected )
                {
                    meshPainter.currentBrush = i;
                }
            }
        }

        private void drawShapePreview(Point origin, Point mouse)
        {
            int panelWidth = splitContainer1.Panel1.Width;
            int panelHeight = splitContainer1.Panel1.Height;

            if( pointIsWithinPanel( origin, panelWidth, panelHeight ) && pointIsWithinPanel( mouse, panelWidth, panelHeight ) )
            {
                System.Drawing.Graphics objGraphic = splitContainer1.Panel1.CreateGraphics();


                System.Drawing.Pen pen = new Pen(Color.FromArgb((int)Painter.penColor.GetAsARGB()));

                objGraphic.DrawLine(pen, origin.X, origin.Y, mouse.X, mouse.Y);
            }
        }

        private bool pointIsWithinPanel(Point point, int width, int height)
        {
            bool isWithin = false;
            if (point.X >= 0 && point.Y >= 0 && point.X < width && point.Y < height)
            {
                isWithin = true;
            }
            return isWithin;
        }
#endregion

#region UI

        private void setViewMode(object sender, EventArgs e)
        {
            Menu_View_Paint.Checked = false;
            Menu_View_View.Checked = true;
            viewPanel.Visible = true;
            paintPanel.Visible = false;
            Btn_View.BackColor = Color.FromArgb(64, 0, 0);
            Btn_Paint.BackColor = Color.Black;
        }

        private void setPaintMode(object sender, EventArgs e)
        {
            int currentBrushIndex = meshPainter.currentBrush;
            Menu_View_View.Checked = false;
            Menu_View_Paint.Checked = true;
            paintPanel.Visible = true;
            viewPanel.Visible = false;
            Btn_View.BackColor = Color.Black;
            Btn_Paint.BackColor = Color.FromArgb(64, 0, 0);
            brushList.Items[currentBrushIndex].Selected = true;
        }

        private void splitContainer1_Panel1_MouseEnter_1(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Focus();
            mouseInPanel1 = true;
        }

        private void splitContainer1_Panel1_MouseLeave_1(object sender, EventArgs e)
        {
            mouseInPanel1 = false;
        }

        private void paintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setPaintMode(sender, e);
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setViewMode(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Btn_Load_Click(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveCurrentTexture();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save Current Texture";
            saveDialog.DefaultExt = ".png";
            DialogResult dr = saveDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                meshPainter.saveCurrentTextureAs(saveDialog.FileName);
            }
            saveDialog.Dispose();
            saveDialog = null;
        }
#endregion

        private void invertColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.INVERT, null);
            updateTexturePreview();
        }

        private void convertToGrayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.GRAYSCALE, null);
            updateTexturePreview();
        }

        internal void previewBrightnessContrastFilter( float brightness, float contrast )
        {
            undo();
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.CONTRAST, contrast);
            meshPainter.applyFilter(Painter.Filters.BRIGHTNESS, brightness);
            updateTexturePreview();
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            BrightnessContrastControl brightnessContrastForm = new BrightnessContrastControl(this);
            brightnessContrastForm.Show();
            meshPainter.paintMode = Painter.PaintModes.FILTER;
            this.menuStrip1.Enabled = false;
            Btn_Paint.Enabled = false;
            Btn_View.Enabled = false;
            paintPanel.Enabled = false;
            viewPanel.Enabled = false;
            brightnessContrastForm.FormClosing += new FormClosingEventHandler(brightnessContrastForm_FormClosing);
        }

        void brightnessContrastForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (((BrightnessContrastControl)sender).DialogResult == DialogResult.Cancel)
            {
                undo();
            }
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            this.menuStrip1.Enabled = true;
            Btn_Paint.Enabled = true;
            Btn_View.Enabled = true;
            paintPanel.Enabled = true;
            viewPanel.Enabled = true;
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.BLUR, null);
            updateTexturePreview();
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.GAUSSIAN_BLUR, null);
            updateTexturePreview();
        }

        private void sharpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addToUndo();
            meshPainter.applyFilter(Painter.Filters.SHARPEN, null);
            updateTexturePreview();
        }

        private void btn_Brush_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.BRUSH;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            btn_Brush.BackColor = Color.FromArgb(64, 0, 0);
        }

        private void btn_Blur_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.BLUR;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            btn_Blur.BackColor = Color.FromArgb(64, 0, 0);
        }

        private void btn_Sharpen_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.SHARPEN;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            btn_Sharpen.BackColor = Color.FromArgb(64, 0, 0);
        }

        private void btn_Burn_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.BURN;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            btn_Burn.BackColor = Color.FromArgb(64, 0, 0);
        }

        private void btn_Dodge_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.DODGE;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            btn_Dodge.BackColor = Color.FromArgb(64, 0, 0);
        }

        private void deactivateTools()
        {
            btn_Blur.BackColor = Color.Black;
            btn_Brush.BackColor = Color.Black;
            btn_Sharpen.BackColor = Color.Black;
            btn_Burn.BackColor = Color.Black;
            btn_Dodge.BackColor = Color.Black;
            btn_Shape.BackColor = Color.Black;
        }

        private void btn_Shape_Click(object sender, EventArgs e)
        {
            deactivateTools();
            meshPainter.currentTool = Painter.Tools.SHAPE;
            meshPainter.paintMode = Painter.PaintModes.TOOLS;
            meshPainter.currentShape = Painter.Shapes.LINE;
            btn_Shape.BackColor = Color.FromArgb(64, 0, 0);
        }
    }
}