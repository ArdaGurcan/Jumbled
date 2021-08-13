using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour
{

    void Update()
    {
        if(Input.anyKeyDown || PlayerPrefs.GetInt("Progress") >= SceneManager.GetActiveScene().buildIndex)
        {
            PlayerPrefs.SetInt("Progress", SceneManager.GetActiveScene().buildIndex);
            Destroy(gameObject);
        }

    }
}
