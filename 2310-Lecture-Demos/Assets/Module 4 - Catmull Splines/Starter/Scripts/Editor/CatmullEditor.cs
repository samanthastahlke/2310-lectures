//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEditor;

//You don't need to worry about understanding this file for this class.
//If you are curious:
//It hooks into Unity's scene GUI update and calls some custom utilities to
//draw a path in the Editor scene.
[CustomEditor(typeof(CatmullMover))]
public class CatmullEditor : Editor
{
    private const int samples = 8;

    private void OnEnable()
    {
        SceneView.duringSceneGui += GUICallback;
    }

    private void GUICallback(SceneView sceneview)
    {
        CatmullMover obj = target as CatmullMover;

        if (obj == null)
            return;

        PathDrawUtility.DrawCatmullPath(obj.points,
                                        CatmullDemo.Catmull,
                                        samples);
    }
}
