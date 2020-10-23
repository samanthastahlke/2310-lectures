using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class AnimatorUIControls : MonoBehaviour
{
    public Animator controller;
    public Toggle moveToggle;
    public Slider blendSlider;

    void Update()
    {
        controller.SetBool("Moving", moveToggle.isOn);
        controller.SetFloat("WalkRunBlend", blendSlider.value);
    }
}
