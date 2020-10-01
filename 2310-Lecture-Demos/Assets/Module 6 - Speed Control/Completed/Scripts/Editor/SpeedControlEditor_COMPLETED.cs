//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEditor;

//You don't need to worry about understanding this file for this class.
//If you are curious:
//It hooks into Unity's scene GUI update and calls some custom utilities to
//draw a path in the Editor scene.
[CustomEditor(typeof(SpeedControlMover_COMPLETED))]
public class SpeedControlEditor_COMPLETED : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += GUICallback;
    }

    private void GUICallback(SceneView sceneview)
    {
        SpeedControlMover_COMPLETED obj = target as SpeedControlMover_COMPLETED;

        if (obj == null)
            return;

        PathDrawUtility.DrawCatmullPath(obj.points, 
                                        MathUtility.Catmull,
                                        obj.samples);
    }
}
