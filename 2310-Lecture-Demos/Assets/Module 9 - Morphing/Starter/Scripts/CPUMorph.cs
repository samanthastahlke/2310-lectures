//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CPUMorph : MonoBehaviour
{
    /* Important note:
     * For morph targets to make sense, you will need to make sure that
     * the vertex data for both meshes is specified in the same order.
     * 
     * Your Blender workflow probably already supports this; if you are
     * exporting multiple "versions" of the same mesh, you're good.
     * 
     * If you wanted to do something like morph between two completely unrelated
     * meshes, you would need to define a sensible way to map between 
     * the vertices of the two meshes (otherwise there's no guarantee that
     * your vertex indices would line up nicely). You'd also need to make sure
     * that both meshes had the same number of vertices - otherwise you'd
     * need to get even fancier.
     */
    public Mesh mesh1;
    public Mesh mesh2;

    //This will store the data of the mesh we're currently rendering -
    //an "in-between" of the two "keyframe" meshes we've specified.
    private Mesh currentMesh;
    private Vector3[] currentVertices;
    private Vector3[] currentNormals;

    //How long the transformation should take.
    public float totalTime = 2.0f;

    private float timer = 0.0f;
    private bool forwards = true;

    void Start()
    {
        //TODO: Create and initialize the mesh we will render each frame.
    }

    void Update()
    {
        //TODO: Update timer and calculate t.

        //TODO: Morph targets!
    }
}
