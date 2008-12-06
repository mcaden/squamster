using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MOIS;
using Mogre;

namespace Squamster
{
    public partial class OgreForm : Form
    {
        const int tickInterval = 40; //(milliseconds between ticks) 40 = ~25 FPS - This is the camera/input update interval
        
        public static Root mRoot = null;
        public static SceneManager mSceneMgr = null;
        public static Entity mCurrentEntity = null;
        RenderWindow mWindow;
        ExtendedCamera ExCamera;
        bool mShutdownRequested = false;           
        
        public Rectangle2D mMiniScreen;
        public SceneNode mMiniScreenNode;

        MeshLoader meshLoader;
        StringVector materials = new StringVector();

        AnimationState mAnimState = null;
        bool playAnim = false;        

        protected MOIS.InputManager inputManager;
        protected MOIS.Keyboard inputKeyboard;
        protected MOIS.Mouse inputMouse;
        bool mouseInPanel1 = false;

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
            mRoot.Dispose();
            mRoot = null;
        }

        public void Go()
        {
            Show();
            while (mRoot != null && !mShutdownRequested)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(0);
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
            RenderSystem rs = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
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
            meshLoader = new MeshLoader();
            Camera cam = mSceneMgr.CreateCamera("Camera");
            cam.NearClipDistance = .01f;

            mWindow.AddViewport(cam);

            ExCamera = new ExtendedCamera("myExtCam", "sceneMgr", "Camera");

            // Init resources
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

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


            // RTT code
            
            TexturePtr texture = TextureManager.Singleton.CreateManual("RttTex", 
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, TextureType.TEX_TYPE_2D,
                mWindow.Width, mWindow.Height, 0, PixelFormat.PF_R8G8B8A8, (int)TextureUsage.TU_RENDERTARGET);

            RenderTexture renderTexture = texture.GetBuffer().GetRenderTarget();

            renderTexture.AddViewport( ExCamera.getOgreCamera );
            renderTexture.GetViewport(0).SetClearEveryFrame(true);
            renderTexture.GetViewport(0).BackgroundColour = ColourValue.White;
            renderTexture.GetViewport(0).OverlaysEnabled = false;

            mMiniScreen = new Rectangle2D(true);
            mMiniScreen.SetCorners(0.5f, -0.5f, 1.0f, -1.0f);
            mMiniScreen.BoundingBox = new AxisAlignedBox(-100000.0f * Mogre.Vector3.UNIT_SCALE, 100000.0f * Mogre.Vector3.UNIT_SCALE);

            mMiniScreenNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("MiniScreenNode");
            mMiniScreenNode.AttachObject(mMiniScreen);

            //Materials for the rtt Screen
            MaterialPtr rttMaterial = MaterialManager.Singleton.Create("RttMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            rttMaterial.CreateTechnique().CreatePass();
            rttMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
            rttMaterial.GetTechnique(0).GetPass(0).CreateTextureUnitState("RttTex");
            mMiniScreen.SetMaterial("RttMat");


            //Materials for the uv encoding
            MaterialPtr uvEncodingMaterial = MaterialManager.Singleton.Create("uvEncodingMat", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            uvEncodingMaterial.CreateTechnique().CreatePass();
            uvEncodingMaterial.GetTechnique(0).GetPass(0).LightingEnabled = false;
            uvEncodingMaterial.GetTechnique(0).GetPass(0).CreateTextureUnitState("uvEncodingMat");
            uvEncodingMaterial.GetTechnique(0).GetPass(0).SetFragmentProgram("uvEncode_ps");


            renderTexture.PreRenderTargetUpdate += new RenderTargetListener.PreRenderTargetUpdateHandler( preRenderTargetUpdate );
            renderTexture.PostRenderTargetUpdate += new RenderTargetListener.PostRenderTargetUpdateHandler( postRenderTargetUpdate );


            timer1.Start();            
        }

 
        void preRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            if (mCurrentEntity != null)
            {
                materials.Clear();
                for (uint i = 0; i < mCurrentEntity.NumSubEntities; i++)
                {
                    materials.Add(mCurrentEntity.GetSubEntity(i).MaterialName);
                    mCurrentEntity.GetSubEntity(i).MaterialName = "uvEncodingMat";
                }
            }
            mMiniScreenNode.SetVisible(false);
        }

        void postRenderTargetUpdate(RenderTargetEvent_NativePtr evt)
        {
            if (mCurrentEntity != null)
            {
                for (uint i = 0; i < mCurrentEntity.NumSubEntities; i++)
                {
                    mCurrentEntity.GetSubEntity(i).MaterialName = materials[(int)i];
                }
            }
            mMiniScreenNode.SetVisible(true);
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
        /// <summary>
        /// Adds a mesh's name to the mesh listbox
        /// </summary>
        /// <param name="mesh">The name of the mesh to add to the list.</param>
        private void addMeshToList( String mesh )
        {
            meshListBox.Items.Add(mesh);


            mSceneMgr.GetSceneNode(mesh).SetVisible(false);
        }



#region Events
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = tickInterval;
            if (mAnimState != null && playAnim)
            {
                mAnimState.AddTime( (float)tickInterval / 1000); //convert from milliseconds to seconds.
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

            if (!mRoot.RenderOneFrame())
            {
                mShutdownRequested = true;
            }
        }             
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = System.Drawing.Image.FromFile( imageList1.Images.Keys[texList.SelectedIndex]);
        }       

    #region Panel1

        private void splitContainer1_Panel1_MouseEnter(object sender, EventArgs e)
        {
            mouseInPanel1 = true;
            splitContainer1.Panel1.Focus();
        }
        private void splitContainer1_Panel1_MouseLeave(object sender, EventArgs e)
        {
            mouseInPanel1 = false;
        }

    #endregion
    #region Meshes

        private void meshListBox_SelectedIndexChanged(object sender, EventArgs e)
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
                //Texture information
                imageList1.Images.Clear();
                texList.Items.Clear();
                if (textureNames.Count > 0)
                {
                    foreach (String textureName in textureNames)
                    {
                        FileInfoList fileInfoItr = ResourceGroupManager.Singleton.FindResourceFileInfo(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, textureName);
                        for (int j = 0; j < fileInfoItr.Count; j++)
                        {
                            string path = fileInfoItr[j].archive.Name + "/" + fileInfoItr[j].filename;
                            statsLabel.Text += "\n" + path;
                            System.Drawing.Image texImage = System.Drawing.Image.FromFile(path);
                            imageList1.Images.Add(path, texImage);
                            texList.Items.Add(textureName);
                            if (texList.Items.Count == 1)
                            {
                                texList.SelectedIndex = 0;
                            }
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
        private void Btd_LoadAll_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            bool autoSelectNewMesh = false;
            if (meshListBox.Items.Count == 0)
            {
                autoSelectNewMesh = true;
            }
            StringVector meshesAdded = meshLoader.createAllMeshesFromResourceSystem();
            foreach (String mesh in meshesAdded)
            {
                addMeshToList(mesh);
            }
            if (autoSelectNewMesh)
            {
                meshListBox.SetSelected(0, true);
            }
            Cursor = Cursors.Default;
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
                    meshListBox.SetSelected(0, true);
                }
                Cursor = Cursors.Default;
            }
        }        

    #endregion        
    #region Animations
        
        private void animBox_SelectionChangeCommitted(object sender, EventArgs e)
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
                if (e.state.ButtonDown(MouseButtonID.MB_Right))
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
                else if (e.state.ButtonDown(MouseButtonID.MB_Left))
                {
                    
                }
                else if (e.state.Z.rel != 0)
                {
                    ExCamera.cameraZoom(e.state.Z.rel * .1f);
                }
                else if (e.state.ButtonDown(MouseButtonID.MB_Middle))
                {
                    ExCamera.pan(new Mogre.Vector3(e.state.X.rel * .5f, e.state.Y.rel * .5f, 0));
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
            switch (e.key)
            {
                case MOIS.KeyCode.KC_ESCAPE:
                    break;
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


    }
}