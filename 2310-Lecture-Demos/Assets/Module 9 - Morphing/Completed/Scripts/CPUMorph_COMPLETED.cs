//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CPUMorph_COMPLETED : MonoBehaviour
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
        currentMesh = new Mesh();
        currentMesh.name = "Morph Mesh";

        //This tells Unity that we will frequently update data associated with this mesh.
        //(This is the same in spirit as marking a buffer with GL_DYNAMIC_DRAW.)
        currentMesh.MarkDynamic();

        //Initialize our mesh to the first "frame" of our morph targets.
        currentVertices = mesh1.vertices;
        currentNormals = mesh1.normals;

        currentMesh.vertices = currentVertices;
        currentMesh.triangles = mesh1.triangles;
        currentMesh.normals = currentNormals;
        
        GetComponent<MeshFilter>().mesh = currentMesh;
    }

    void Update()
    {
        //Update our timer...
        timer += (forwards) ? Time.deltaTime : -Time.deltaTime;

        if (timer <= 0.0f || timer >= totalTime)
            forwards = !forwards;

        timer = Mathf.Clamp(timer, 0.0f, totalTime);

        //Calculate our interpolation parameter...
        float t = timer / totalTime;

        /* Important - we assume here that both meshes have the same number of vertices
         * and have vertex normals defined. 
         * 
         * These are safe assumptions given that we're in Unity and we're using two 
         * meshes that we know have the same number of vertices.
         * 
         * If you're doing something "fancier", though, just keep in mind that these 
         * assumptions might not always be true.
         */
        for (int i = 0; i < currentVertices.Length; ++i)
        {
            currentVertices[i] = Vector3.Lerp(mesh1.vertices[i], mesh2.vertices[i], t);

            //We also need to interpolate our normals!
            currentNormals[i] = Vector3.Normalize(Vector3.Lerp(mesh1.normals[i], mesh2.normals[i], t));
        }

        //Finally, tell our mesh to use the calculated vertices and normals.
        currentMesh.SetVertices(currentVertices);
        currentMesh.SetNormals(currentNormals);
    }
}
