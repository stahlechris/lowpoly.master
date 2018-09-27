using UnityEngine;

/* DESCRIPTION:
 * 1.This script is a runtime performance enhancement! 
 * 
 * 2.This script takes X children GameObjects (GO) of this parent
 *   and converts them into a single mesh, which is assigned to this parent.
 * 
 * USE: 
 * 1. Have an empty GO with 0,0,0 rotation and scale values.
 * 2. Child GO's that you wish to combine to this parent.
 * 3. Add a component of type "MeshCombiner" - or drag and drop this script - 
 *    to this parent.
 * 4. Play the game to test.
 * 
 * 
 * README:
 * 1. Parent empty GO must have Rotational values of 0, 0, 0.
 * 2. Parent empty GO must have Scale values of 0, 0, 0.
 */


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour 
{
    [Tooltip("Do you want to make a mesh collider out of the combined mesh?")]
    [SerializeField] bool makeMeshCollider;

    [Tooltip("By default objects are just deactivated. Tick this to destroy them instead.")]
    [SerializeField] bool destroyChildObjects;

    [Tooltip("This will be useful if you're making an 'invisible mesh' and just need the mesh as a collider.")]
    [SerializeField] bool turnOffMeshRendererWhenDone;


    private void Start()
    {
        CombineMeshes();
    }

    public void CombineMeshes()
    {
        //"Moving a rotation might change a position, but moving a position will never change a rotation".

        //Cache this parent's local values.
        Vector3 oldPos = transform.localPosition;
        Quaternion oldRot = transform.localRotation;
        //Reset this parent's values.
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;


        //Do the magic stuff.
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        Mesh finalMesh = new Mesh();

        //Debug.Log(name + " is combining " + meshFilters.Length + meshFilters[1].name + " meshes!");

        for (int i = 1; i < meshFilters.Length; i++)
        {
            //skip the empty parent by starting on index 1

            combine[i].subMeshIndex = 0;
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            if (!destroyChildObjects)
            {
                meshFilters[i].gameObject.SetActive(false);
            }
            else
            {
                Destroy(meshFilters[i].gameObject);
            }
        }

        finalMesh.CombineMeshes(combine);
        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        //Assign parent's cached values to put objects in correct place.
        transform.localRotation = oldRot;
        transform.localPosition = oldPos;


        //If you ticked to make a MeshCollider out of your new Mesh...
        if (makeMeshCollider)
        {
            var myMeshCollider = GetComponent<MeshCollider>();

            if (myMeshCollider != null)
            {
                GetComponent<MeshCollider>().sharedMesh = finalMesh;
            }
            else
            {
                myMeshCollider = gameObject.AddComponent<MeshCollider>();
                myMeshCollider.sharedMesh = finalMesh;
            }
        }

        if (turnOffMeshRendererWhenDone)
        {
            //Don't render anything on screen. Used for 'invisible' mesh
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
