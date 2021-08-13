using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool playersTurn;
    public bool gameOver;

    public PlayerControl player;
    public List<EnemyScript> enemies;

    public AudioSource audioSource;

    public int currentEnemy;

    public DoorScript door;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (enemies.Count == 0 && !door.open)
        {
            door.Open();
        }


        //if (!GameObject.Find("Panel"))
        //{


            if (!gameOver)
            {
                Debug.Log(enemies.Count);
                if (playersTurn)
                {
                    player.MakeMove();
                }
                else if (currentEnemy < enemies.Count)
                {
                    if (enemies[currentEnemy])
                    {
                        enemies[currentEnemy].MakeMove();
                    }
                    else
                    {
                        enemies.Remove(enemies[currentEnemy]);
                    }
                }
                else
                {
                    currentEnemy = 0;
                    playersTurn = true;
                    player.usedAttacks = 0;
                    player.usedMoves = 0;
                    player.itemChosen = false;
                }
            }
            else
            {
                if (player.heading != "dead")
                {
                    audioSource.Play();
                    player.heading = "dead";
                }
                Debug.Log("RIP");

            }

        //}




    }
}
