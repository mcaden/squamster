using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mogre;

namespace Squamster
{
    class MeshLoader
    {
        public MeshLoader()
        {
        }

        /// <summary>
        /// Creates all meshes currently resident in the resource system.
        /// 
        /// //TODO:  It seems to not find EVERYTHING in the manager, just a large part.
        /// </summary>
        /// <returns>StringVector containing a list of all meshes that were created</returns>
        public StringVector createAllMeshesFromResourceSystem()
        {
            StringVector groups = ResourceGroupManager.Singleton.GetResourceGroups();
            StringVector meshes = new StringVector();
            StringVector meshesAdded = new StringVector();

            foreach (String group in groups)
            {
                ResourceGroupManager.Singleton.InitialiseResourceGroup(group);
                StringVector newMeshes = ResourceGroupManager.Singleton.FindResourceNames(group, "*.mesh");
                foreach (String newMesh in newMeshes)
                {
                    meshes.Add(newMesh);
                }
            }

            foreach (String mesh in meshes)
            {
                if (loadMesh(mesh))
                {
                    meshesAdded.Add(mesh);
                }
            }
            return meshesAdded;
        }

        /// <summary>
        /// Creates a single mesh from a given file
        /// </summary>
        /// <param name="path">Path to the file - will be added to the ResourceManager</param>
        /// <param name="fileName">The name of the mesh file to create</param>
        /// <returns>A string containing the name of the mesh</returns>
        public String createMeshFromFile(string path, string fileName)
        {
            if (!ResourceGroupManager.Singleton.ResourceExists(ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, fileName))
            {
                MessageBox.Show("Warning:  Resources added outside the pre-defined configuration will not have materials, skeletons, or textures from different directories loaded");
                ResourceGroupManager.Singleton.AddResourceLocation(path, "FileSystem", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, true);
                MeshManager.Singleton.Load(fileName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            }
            if ( !loadMesh(fileName))
            {
                fileName = "";
            }
            return fileName;
        }

        /// <summary>
        /// Creates the SceneNode and Entity for the mesh name added in. 
        /// </summary>
        /// <param name="meshName">The name of the mesh within the resource system.</param>
        /// <returns>True if successful</returns>
        public bool loadMesh(String meshName)
        {
            meshName.Trim();
            bool isMeshAdded = false;
            if (meshName.Length > 0)
            {
                LogManager.Singleton.LogMessage("Adding: " + meshName);

                if( !OgreForm.mSceneMgr.HasSceneNode( meshName ) )
                {
                    SceneNode meshNode = OgreForm.mSceneMgr.RootSceneNode.CreateChildSceneNode(meshName, new Mogre.Vector3(0, 0, 0));
                    Entity ent = OgreForm.mSceneMgr.CreateEntity(meshName, meshName);
                    meshNode.AttachObject(ent);
                    isMeshAdded = true;
                }
                else
                {
                    LogManager.Singleton.LogMessage("Skipping " + meshName + " because it already exists.");
                }
                LogManager.Singleton.LogMessage(meshName + " - Added ");
            }
            return isMeshAdded;
        }
    }
}
