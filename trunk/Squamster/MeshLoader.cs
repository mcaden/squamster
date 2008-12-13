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
