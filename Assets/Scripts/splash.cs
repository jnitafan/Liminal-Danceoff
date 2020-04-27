using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splash : MonoBehaviour
{
    // Start is called before the first frame update

    public int SceneNumber;
    void Start()
    {
        if (SceneNumber == 0)
        {
            StartCoroutine(ToBeach());
        }
    }
    IEnumerator ToBeach()
    {
        yield return new WaitForSeconds(2);
        SceneNumber = 1;
        SceneManager.LoadScene(1);
    }


}  
