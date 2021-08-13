using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public Sprite doorOpen;
    public SpriteRenderer image;
    public bool open = false;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip enterSound;
    public GameObject end;

    public void Open()
    {
        audioSource.PlayOneShot(openSound);
        open = true;
        image.sprite = doorOpen;
        gameObject.layer = 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine("NextScene");
        }
    }

    IEnumerator NextScene()
    {
        audioSource.PlayOneShot(enterSound);
        yield return new WaitForSeconds(.5f);
        if(SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1)
        {
            end.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
    }
}
