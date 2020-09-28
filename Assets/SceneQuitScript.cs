using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.Core;
using Liminal.Core.Fader;


public class SceneQuitScript : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("is running");
        var fader = ScreenFader.Instance;
        fader.FadeTo(Color.black, 10f);
        ExperienceApp.End();
        Debug.Log("is running");


    }

}
