using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Squamster
{

    //Much of this is derived from the sample 3rd-person camera on the Ogre3d Wiki
    class ExtendedCamera
    {
    	SceneNode mTargetNode; // The camera target
        SceneNode mCameraNode; // The camera itself
        Camera mCamera; // Ogre camera
        Light mLight;

        SceneNode mSightNode;
        SceneNode mSpinNode;
        SceneNode mPitchNode;
        SceneNode mZoomNode;

        SceneManager mSceneMgr;

        float mTightness; // Determines the movement of the camera - 1 means tight movement, while 0 means no movement
        float scale = 200f; //Orthographic Zoom


        public ExtendedCamera( String name, String sceneMgrName, String camName ) 
        {
	        // Basic member references setup
	        mSceneMgr = Root.Singleton.GetSceneManager( sceneMgrName );
            mCamera = mSceneMgr.GetCamera( camName );
            mCamera.ProjectionType = ProjectionType.PT_ORTHOGRAPHIC;
            mCamera.FarClipDistance = 500000;
            mCamera.NearClipDistance = 200; 
            
            mSceneMgr.AmbientLight = new Mogre.ColourValue(0.5f,0.5f,0.5f);
            mLight = mSceneMgr.CreateLight("Light");
	        mLight.Type = Light.LightTypes.LT_DIRECTIONAL;
	        mLight.Position = new Vector3( 0, 5, 0);
	        Vector3 dir = (-mLight.Position);
	        dir.Normalise();
	        mLight.Direction = dir;
	        mLight.DiffuseColour = new ColourValue(0.5f, 0.5f, 0.5f);
            mLight.SpecularColour = new ColourValue(0.75f, 0.75f, 0.75f);
	        mLight.CastShadows = true;

            //Camera guidance
            mSightNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(name + "_sight", new Vector3(0, 0, 0));
            mSpinNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(name + "_spin", new Vector3(0, 0, 0));
            mPitchNode = mSpinNode.CreateChildSceneNode(name + "_pitch", new Vector3(0, 0, 0));
            mZoomNode = mPitchNode.CreateChildSceneNode(name + "_zoom", new Vector3(0, 0, -40000));
            

	        //Camera's node structure(mCameraNode will drift towards mZoomNode, mTargetNode will drift towards mSightNode )
            mCameraNode = mSceneMgr.RootSceneNode.CreateChildSceneNode(name, new Vector3(0, 0, 0));
	        mTargetNode = mSceneMgr.RootSceneNode.CreateChildSceneNode (name + "_target", new Vector3(0, 0, 0));
	         //The camera will always look at the camera target
            mCameraNode.SetAutoTracking (true, mTargetNode);
	        mCameraNode.SetFixedYawAxis (true); // Needed because of auto tracking

            setTightness(.85f);


	        mCameraNode.AttachObject(mCamera);
        }

        ~ExtendedCamera() 
        {
        }

        public void setTightness(float tightness) 
        {
	        mTightness = tightness;
        }

        public float getTightness() 
        {
	        return mTightness;
        }

        public void recenterCamera( Vector3 newPosition )
        {
            mSightNode.Position = newPosition;
            mSpinNode.Position = newPosition;
            mSpinNode.ResetOrientation();
            mPitchNode.ResetOrientation();
            scale = 200f;
            mCamera.NearClipDistance = 200f;
            instantUpdate();
        }

        public Vector3 getCameraPosition() 
        {
	        return mCameraNode.Position;
        }

        public Vector3 getTargetPosition() 
        {
            return mTargetNode.Position;
        }

        public void cameraZoom(float zoomFactor)
        {
            if (scale < 200)
            {
                scale = scale + (zoomFactor * -1 * scale / 200);
            }
            else
            {
                scale = scale + (zoomFactor * -1);
            }
            if (scale < .01f)
            {
                scale = 0.01f;
            }

            mCamera.NearClipDistance = scale;
        }
        public void cameraYaw(Degree yawAmount)
        {
	        mSpinNode.Yaw( yawAmount );
        }

        public void pan( Vector3 moveAmount )
        {
            mSightNode.Translate(moveAmount, Node.TransformSpace.TS_LOCAL);
            mSpinNode.Translate(moveAmount, Node.TransformSpace.TS_LOCAL);
        }

        public void cameraPitch(Degree pitchAmount)
        {
            mPitchNode.Pitch( pitchAmount, Node.TransformSpace.TS_LOCAL );

            //Stop the camera from flipping over
            float PitchAngle; 
            PitchAngle = mPitchNode.Orientation.Pitch.ValueDegrees; 
            if(PitchAngle > 89) 
            {
                mPitchNode.Pitch((Degree)(89 - PitchAngle), Node.TransformSpace.TS_LOCAL); 
            } 

            if(PitchAngle < -89) 
            { 
              mPitchNode.Pitch(-(Degree)(89 + PitchAngle), Node.TransformSpace.TS_LOCAL); 
            } 
        }

        public void setLook(Vector3 pos)
        {
	        mCamera.LookAt( pos );
        }

        public void instantUpdate()
        {
	        mCameraNode.Position = mZoomNode.Position;
            mTargetNode.Position = mSightNode.Position;
        }

        public void update()
        {
            Vector3 cameraPosition = mZoomNode._getDerivedPosition();
            Vector3 targetPosition = mSpinNode._getDerivedPosition();
	        // Handle movement
	        Vector3 displacement = new Vector3();

	        displacement = (cameraPosition - mCameraNode.Position) * mTightness;
	        mCameraNode.Translate(displacement);

	        displacement = (targetPosition - mTargetNode.Position) * mTightness;
            mTargetNode.Translate(displacement);

            mLight.Direction = -mCameraNode.Position;
        }
    }
}
