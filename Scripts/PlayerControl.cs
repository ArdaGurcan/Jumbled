using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public bool gameOver = false;

    public List<string> inventory;

    public Sprite blackSword;
    public Sprite whiteSword;
    public Sprite blackArrow;
    public Sprite whiteArrow;

    public bool arrowIsBlack;
    public string shootDirection;
    public GameObject arrowPrefab;

    public AudioSource audioSource;

    public bool shielded;

    public int maxMoves;
    public int usedMoves;

    public int maxAttacks;
    public int usedAttacks;

    public int damage;

    public LayerMask blackEnemies;
    public LayerMask whiteEnemies;

    public LayerMask obstacles;
    public LayerMask nonAttackables;

    public SpriteRenderer character;

    public GameManager gm;

    public Image current;
    public Image next;

    public SpriteRenderer redUp;
    public SpriteRenderer redDown;
    public SpriteRenderer redLeft;
    public SpriteRenderer redRight;

    public SpriteRenderer greenUp;
    public SpriteRenderer greenDown;
    public SpriteRenderer greenLeft;
    public SpriteRenderer greenRight;

    public string currentItem;
    public string nextItem;

    public bool itemChosen;

    public List<string> inventorySequence;
    public int inventoryIndex;

    public Sprite characterUp;
    public Sprite characterDown;
    public Sprite characterLeft;
    public Sprite characterRight;
    public Sprite characterDead;

    public string heading = "down";

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        current = GameObject.Find("Current").GetComponent<Image>();
        if (GameObject.Find("Next"))
            next = GameObject.Find("Next").GetComponent<Image>();

        if (inventory.Count > 1)
        {
            inventorySequence = inventory;
            GenerateSequence();
            currentItem = inventorySequence[inventoryIndex];
            nextItem = inventorySequence[inventoryIndex + 1];
            Debug.Log("Current: " + currentItem + ", Next: " + nextItem);
            ShowItems();
            itemChosen = true;
        }
        else if(inventory.Count == 1)
        {
            currentItem = inventory[0];
            //nextItem = inventory[0];
            ShowItems();

        }

    }
    public void MakeMove()
    {
        if(heading!= "dead")
        {
            if (!itemChosen && inventory.Count > 1)
            {
                inventoryIndex++;
                currentItem = nextItem;
                if (inventoryIndex + 1 == inventory.Count)
                {
                    GenerateSequence();
                    inventoryIndex = -1;
                }

                nextItem = inventorySequence[inventoryIndex + 1];
                ShowItems();
                itemChosen = true;

            }

            if (usedAttacks < maxAttacks && inventory.Count > 0 && usedMoves < maxMoves)
            {
                switch (currentItem)
                {
                    case "black_sword":
                        #region black_sword
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;
                        redUp.enabled = false;
                        redDown.enabled = false;
                        redLeft.enabled = false;
                        redRight.enabled = false;

                        if (Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies))
                        {
                            redUp.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            greenUp.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies))
                        {
                            redDown.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            greenDown.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies))
                        {
                            redLeft.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            greenLeft.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies))
                        {
                            redRight.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            greenRight.enabled = true;
                        }

                        if (Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies))
                            {
                               
                                EnemyScript enemy = Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                enemy.health -= damage;
                            audioSource.Play();
                            heading = "up";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow) && Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies))
                            {
                                
                                EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                enemy.health -= damage;
                            audioSource.Play();

                            heading = "down";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies))
                            {
                                
                                EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                enemy.health -= damage;
                            audioSource.Play();
                            heading = "left";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow) && Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies))
                            {
                                
                                
                                EnemyScript enemy = Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                enemy.health -= damage;
                            audioSource.Play();
                            heading = "right";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.UpArrow) && !Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                            {
                                transform.position += Vector3.up;
                                heading = "up";
                                usedMoves++;
                                greenUp.enabled = false;
                                greenDown.enabled = false;
                                greenLeft.enabled = false;
                                greenRight.enabled = false;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow) && !Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                            {
                                transform.position += -Vector3.up;
                                heading = "down";
                                usedMoves++;
                                greenUp.enabled = false;
                                greenDown.enabled = false;
                                greenLeft.enabled = false;
                                greenRight.enabled = false;
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                            {
                                transform.position += -Vector3.right;
                                heading = "left";
                                usedMoves++;
                                greenUp.enabled = false;
                                greenDown.enabled = false;
                                greenLeft.enabled = false;
                                greenRight.enabled = false;
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow) && !Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                            {
                                transform.position += Vector3.right;
                                heading = "right";
                                usedMoves++;
                                greenUp.enabled = false;
                                greenDown.enabled = false;
                                greenLeft.enabled = false;
                                greenRight.enabled = false;
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                usedMoves = maxMoves;
                            }

                        #endregion
                        break;

                    case "white_sword":
                        #region white_sword
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;


                        if (Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies))
                        {
                            redUp.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            greenUp.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies))
                        {
                            redDown.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            greenDown.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies))
                        {
                            redLeft.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            greenLeft.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies))
                        {
                            redRight.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            greenRight.enabled = true;
                        }

                        if (Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies))
                        {

                            EnemyScript enemy = Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            heading = "up";
                            usedAttacks++;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies))
                        {

                            EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();

                            heading = "down";
                            usedAttacks++;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies))
                        {

                            EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            heading = "left";
                            usedAttacks++;
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies))
                        {


                            EnemyScript enemy = Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            heading = "right";
                            usedAttacks++;
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) && !Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            transform.position += Vector3.up;
                            heading = "up";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && !Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            transform.position += -Vector3.up;
                            heading = "down";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            transform.position += -Vector3.right;
                            heading = "left";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && !Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            transform.position += Vector3.right;
                            heading = "right";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            usedMoves = maxMoves;
                        }

                        #endregion
                        break;
                    case "black_arrow":
                        #region black_arrow
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;


                        if (Physics2D.Raycast(transform.position, transform.up, 15f, blackEnemies))
                        {
                            redUp.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            greenUp.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.up, 15f, blackEnemies))
                        {
                            redDown.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            greenDown.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.right, 15f, blackEnemies))
                        {
                            redLeft.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            greenLeft.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, transform.right, 15f, blackEnemies))
                        {
                            redRight.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            greenRight.enabled = true;
                        }

                        if (Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.Raycast(transform.position, transform.up, 15f, blackEnemies))
                        {

                            shootDirection = "up";
                            arrowIsBlack = true;
                            heading = "up";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && Physics2D.Raycast(transform.position, -transform.up, 15f, blackEnemies))
                        {

                            shootDirection = "down";
                            arrowIsBlack = true;
                            heading = "down";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position, -transform.right, 15f, blackEnemies))
                        {

                            shootDirection = "left";
                            arrowIsBlack = true;
                            heading = "left";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && Physics2D.Raycast(transform.position, transform.right, 15f, blackEnemies))
                        {


                            shootDirection = "right";
                            arrowIsBlack = true;
                            heading = "right";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) && !Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            transform.position += Vector3.up;
                            heading = "up";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && !Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            transform.position += -Vector3.up;
                            heading = "down";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            transform.position += -Vector3.right;
                            heading = "left";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && !Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            transform.position += Vector3.right;
                            heading = "right";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            usedMoves = maxMoves;
                        }

                        #endregion
                        break;

                    case "white_arrow":
                        #region white_arrow
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;


                        if (Physics2D.Raycast(transform.position, transform.up, 15f, whiteEnemies))
                        {
                            redUp.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            greenUp.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.up, 15f, whiteEnemies))
                        {
                            redDown.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            greenDown.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, -transform.right, 15f, whiteEnemies))
                        {
                            redLeft.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            greenLeft.enabled = true;
                        }

                        if (Physics2D.Raycast(transform.position, transform.right, 15f, whiteEnemies))
                        {
                            redRight.enabled = true;
                        }
                        else if (!Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            greenRight.enabled = true;
                        }

                        if (Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.Raycast(transform.position, transform.up, 15f, whiteEnemies))
                        {

                            shootDirection = "up";
                            arrowIsBlack = false;
                            heading = "up";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && Physics2D.Raycast(transform.position, -transform.up, 15f, whiteEnemies))
                        {

                            shootDirection = "down";
                            arrowIsBlack = false;
                            heading = "down";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && Physics2D.Raycast(transform.position, -transform.right, 15f, whiteEnemies))
                        {

                            shootDirection = "left";
                            arrowIsBlack = false;
                            heading = "left";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && Physics2D.Raycast(transform.position, transform.right, 15f, whiteEnemies))
                        {


                            shootDirection = "right";
                            arrowIsBlack = false;
                            heading = "right";
                            StartCoroutine("ShootArrow");
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow) && !Physics2D.Raycast(transform.position, transform.up, 1f, obstacles))
                        {
                            transform.position += Vector3.up;
                            heading = "up";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.DownArrow) && !Physics2D.Raycast(transform.position, -transform.up, 1f, obstacles))
                        {
                            transform.position += -Vector3.up;
                            heading = "down";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !Physics2D.Raycast(transform.position, -transform.right, 1f, obstacles))
                        {
                            transform.position += -Vector3.right;
                            heading = "left";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow) && !Physics2D.Raycast(transform.position, transform.right, 1f, obstacles))
                        {
                            transform.position += Vector3.right;
                            heading = "right";
                            usedMoves++;
                            greenUp.enabled = false;
                            greenDown.enabled = false;
                            greenLeft.enabled = false;
                            greenRight.enabled = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space))
                        {
                            usedMoves = maxMoves;
                        }

                        #endregion
                        break;

                    default:
                        break;
                }

            }
            else if (usedAttacks < maxAttacks && inventory.Count > 0)
            {
                switch (currentItem)
                {
                    case "black_sword":
                        #region black_sword
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;

                        if (Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies) || Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies) || Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies) || Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies))
                        {
                            if (Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies))
                            {
                                redUp.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies))
                            {
                                redDown.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies))
                            {
                                redLeft.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies))
                            {
                                redRight.enabled = true;
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                if (Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "up";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                if (Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();

                                }
                                heading = "down";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                if (Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.right, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "left";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                if (Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, transform.right, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "right";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                usedAttacks++;

                            }
                        }
                        else
                        {
                            usedAttacks = maxAttacks;
                        }
                        #endregion
                        break;

                    case "white_sword":
                        #region white_sword
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;

                        if (Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies) || Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies) || Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies) || Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies))
                        {
                            if (Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies))
                            {
                                redUp.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies))
                            {
                                redDown.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies))
                            {
                                redLeft.enabled = true;
                            }

                            if (Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies))
                            {
                                redRight.enabled = true;
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                if (Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "up";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                if (Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();

                                }
                                heading = "down";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                if (Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, -transform.right, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "left";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                if (Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies))
                                {
                                    EnemyScript enemy = Physics2D.Raycast(transform.position, transform.right, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                                    enemy.health -= damage;
                                    audioSource.Play();
                                }
                                heading = "right";
                                usedAttacks++;
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                usedAttacks++;

                            }
                        }
                        else
                        {
                            usedAttacks = maxAttacks;
                        }
                        #endregion
                        break;
                    case "black_arrow":
                        #region black_arrow
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;
                        if (Physics2D.Raycast(transform.position, transform.up, 15f, blackEnemies) || Physics2D.Raycast(transform.position, -transform.up, 15f, blackEnemies) || Physics2D.Raycast(transform.position, -transform.right, 15f, blackEnemies) || Physics2D.Raycast(transform.position, transform.right, 15f, blackEnemies))
                        {
                            if (!Physics2D.Raycast(transform.position, transform.up, 15f, nonAttackables))
                            {
                                redUp.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, -transform.up, 15f, nonAttackables))
                            {
                                redDown.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, -transform.right, 15f, nonAttackables))
                            {
                                redLeft.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, transform.right, 15f, nonAttackables))
                            {
                                redRight.enabled = true;
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                shootDirection = "up";
                                arrowIsBlack = true;
                                heading = "up";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                shootDirection = "down";
                                arrowIsBlack = true;
                                heading = "down";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                shootDirection = "left";
                                arrowIsBlack = true;
                                heading = "left";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                shootDirection = "right";
                                arrowIsBlack = true;
                                heading = "right";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                usedAttacks++;

                            }
                        }
                        else
                        {
                            usedAttacks = maxAttacks;
                        }
                        #endregion
                        break;

                    case "white_arrow":
                        #region white_arrow
                        greenUp.enabled = false;
                        greenDown.enabled = false;
                        greenLeft.enabled = false;
                        greenRight.enabled = false;
                        if (Physics2D.Raycast(transform.position, transform.up, 15f, whiteEnemies) || Physics2D.Raycast(transform.position, -transform.up, 15f, whiteEnemies) || Physics2D.Raycast(transform.position, -transform.right, 15f, whiteEnemies) || Physics2D.Raycast(transform.position, transform.right, 15f, whiteEnemies))
                        {
                            if (!Physics2D.Raycast(transform.position, transform.up, 15f, nonAttackables))
                            {
                                redUp.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, -transform.up, 15f, nonAttackables))
                            {
                                redDown.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, -transform.right, 15f, nonAttackables))
                            {
                                redLeft.enabled = true;
                            }

                            if (!Physics2D.Raycast(transform.position, transform.right, 15f, nonAttackables))
                            {
                                redRight.enabled = true;
                            }

                            if (Input.GetKeyDown(KeyCode.UpArrow))
                            {
                                shootDirection = "up";
                                arrowIsBlack = false;
                                heading = "up";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.DownArrow))
                            {
                                shootDirection = "down";
                                arrowIsBlack = false;
                                heading = "down";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                shootDirection = "left";
                                arrowIsBlack = false;
                                heading = "left";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                shootDirection = "right";
                                arrowIsBlack = false;
                                heading = "right";
                                StartCoroutine("ShootArrow");
                            }
                            else if (Input.GetKeyDown(KeyCode.Space))
                            {
                                usedAttacks++;

                            }
                        }
                        else
                        {
                            usedAttacks = maxAttacks;
                        }
                        #endregion
                        break;

                    default:
                        break;
                }
            }
            else
            {
                redUp.enabled = false;
                redDown.enabled = false;
                redLeft.enabled = false;
                redRight.enabled = false;
                greenUp.enabled = false;
                greenDown.enabled = false;
                greenLeft.enabled = false;
                greenRight.enabled = false;
                gm.playersTurn = false;
            }
        }
        

    }

    void ShowItems()
    {
        switch (currentItem)
        {
            case "black_sword":
                current.sprite = blackSword;
                break;
            case "white_sword":
                current.sprite = whiteSword;
                break;
            case "black_arrow":
                current.sprite = blackArrow;
                break;
            case "white_arrow":
                current.sprite = whiteArrow;
                break;
            default:
                break;
        }

        if(next)
        {
            switch (nextItem)
            {
                case "black_sword":
                    next.sprite = blackSword;
                    break;
                case "white_sword":
                    next.sprite = whiteSword;
                    break;
                case "black_arrow":
                    next.sprite = blackArrow;
                    break;
                case "white_arrow":
                    next.sprite = whiteArrow;
                    break;
                default:
                    break;
            }
        }
        
    }

    void GenerateSequence()
    {
        for (int i = 0; i < inventorySequence.Count; i++)
        {
            string temp = inventorySequence[i];
            int randomIndex = Random.Range(i, inventorySequence.Count);
            inventorySequence[i] = inventorySequence[randomIndex];
            inventorySequence[randomIndex] = temp;
        }
    }

    IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(.1f);
        if(arrowIsBlack)
        {
            GameObject arrow;
            switch (shootDirection)
            {
                case "up":
                    arrow = Instantiate(arrowPrefab, transform.position + transform.up, Quaternion.Euler(0, 0, 0));
                    arrow.GetComponent<SpriteRenderer>().sprite = blackArrow;
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if(Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }

                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "down":
                    arrow = Instantiate(arrowPrefab, transform.position - transform.up, Quaternion.Euler(0, 0, 180));
                    arrow.GetComponent<SpriteRenderer>().sprite = blackArrow;
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "left":
                    arrow = Instantiate(arrowPrefab, transform.position - transform.right, Quaternion.Euler(0, 0, 90));
                    arrow.GetComponent<SpriteRenderer>().sprite = blackArrow;
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "right":
                    arrow = Instantiate(arrowPrefab, transform.position + transform.right, Quaternion.Euler(0, 0, -90));
                    arrow.GetComponent<SpriteRenderer>().sprite = blackArrow;
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, blackEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;
                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;


            }
            //inventory.Remove("black_arrow");
            //inventoryIndex++;
            //currentItem = nextItem;
            //if (inventoryIndex + 1 == inventory.Count)
            //{
            //    GenerateSequence();
            //    inventoryIndex = -1;
            //}

            //nextItem = inventorySequence[inventoryIndex + 1];
            //ShowItems();
            //itemChosen = true;
            usedAttacks++;
        }
        else
        {
            GameObject arrow;
            switch (shootDirection)
            {
                case "up":
                    arrow = Instantiate(arrowPrefab, transform.position + transform.up, Quaternion.Euler(0, 0, 0));
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "down":
                    arrow = Instantiate(arrowPrefab, transform.position - transform.up, Quaternion.Euler(0, 0, 180));
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "left":
                    arrow = Instantiate(arrowPrefab, transform.position - transform.right, Quaternion.Euler(0, 0, 90));
                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                case "right":
                    arrow = Instantiate(arrowPrefab, transform.position + transform.right, Quaternion.Euler(0, 0, -90));

                    for (int i = 0; i < 15; i++)
                    {
                        if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies))
                        {
                            EnemyScript enemy = Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, whiteEnemies).collider.gameObject.GetComponent<EnemyScript>();
                            enemy.health -= damage;
                            audioSource.Play();
                            Destroy(arrow);
                            break;

                        }
                        else if (Physics2D.Raycast(arrow.transform.position, arrow.transform.up, 1f, nonAttackables))
                        {
                            Destroy(arrow);
                            break;
                        }
                        Vector3 startingPos = arrow.transform.position;
                        Vector3 finalPos = arrow.transform.position + arrow.transform.up;
                        float time = .1f;
                        float elapsedTime = 0;

                        while (elapsedTime < time)
                        {
                            arrow.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                            elapsedTime += Time.deltaTime;
                            yield return null;
                        }
                        arrow.transform.position = finalPos;
                    }
                    break;
                

            }
            
            //inventory.Remove("white_arrow");
            //inventoryIndex++;
            //currentItem = nextItem;
            //if (inventoryIndex + 1 == inventory.Count)
            //{
            //    GenerateSequence();
            //    inventoryIndex = -1;
            //}

            //nextItem = inventorySequence[inventoryIndex + 1];
            //ShowItems();
            //itemChosen = true;
            usedAttacks++;

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        switch (heading)
        {
            case "up":
                character.sprite = characterUp;
                break;
            case "down":
                character.sprite = characterDown;
                break;
            case "left":
                character.sprite = characterLeft;
                break;
            case "right":
                character.sprite = characterRight;
                break;
            case "dead":
                character.sprite = characterDead;
                StartCoroutine("Die");
                break;
            default:
                break;
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


