using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public bool madeMove;
    public int enemyType;
    public LayerMask playerMask;
    public LayerMask antiPlayerMask;
    public LayerMask enemyMask;

    public bool movingUp;
    public bool movingRight;


    public PlayerControl player;
    public GameManager gm;

    public SpriteRenderer spriteRenderer;

    public Sprite weakBlackSkeleton;
    public Sprite weakWhiteSkeleton;

    public bool isBoss;
    public Sprite strongBlackSkeleton;
    public Sprite strongWhiteSkeleton;

    public SpriteRenderer redUp;
    public SpriteRenderer redDown;
    public SpriteRenderer redLeft;
    public SpriteRenderer redRight;

    public AudioSource audioSource;

    public bool isSkeleton;



    public bool attacking;
    
    private void Start()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        if (health == 0)
        {
            Destroy(gameObject);
        }
        else if(health == 1 && isSkeleton && (spriteRenderer.sprite != weakBlackSkeleton && spriteRenderer.sprite != weakWhiteSkeleton))
        {
            if(gameObject.layer == 11)
            {
                spriteRenderer.sprite = weakBlackSkeleton;
            }
            else if (gameObject.layer == 12)
            {
                spriteRenderer.sprite = weakWhiteSkeleton;
            }
        }
        Debug.DrawRay(transform.position, transform.right);
    }



    public void MakeMove()
    {
        

        if (enemyType == 0)
        {
            StartCoroutine("Attack0");
        }
        else if (enemyType == 3)
        {
            StartCoroutine("Attack3");
        }
        else if (enemyType == 1)
        {
            StartCoroutine("Attack1");
        }
        else if (enemyType == 2)
        {
            StartCoroutine("Attack2");
        }
        else if (enemyType == 4)
        {

            StartCoroutine("Attack4");
        }
        else if (enemyType == 5)
        {
            StartCoroutine("Attack5");
        }


    }

    IEnumerator Attack0()
    {
        if (!attacking)
        {
            attacking = true;
            yield return new WaitForSeconds(.2f);


            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }

            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }

            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;


            gm.currentEnemy++;
            attacking = false;
        }

    }
    IEnumerator Attack1()
    {

        if (!attacking)
        {
            attacking = true;
            //yield return new WaitForSeconds(.2f);
            #region move
            if (movingUp)
            {
                if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask) && Physics2D.RaycastAll(transform.position, transform.up, 1f, enemyMask).Length == 1)
                {
                    //transform.position += transform.up;

                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position + transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
                else
                {
                    movingUp = false;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = true;
                    //transform.position += -transform.up;


                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position - transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
            }
            else
            {
                if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask) && Physics2D.RaycastAll(transform.position, -transform.up, 1f, enemyMask).Length == 1)
                {
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position - transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
                else
                {
                    movingUp = true;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = false;
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position + transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
            }
            #endregion
            yield return new WaitForSeconds(.2f);

            #region attack
            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }


            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;
            #endregion
            yield return new WaitForSeconds(.2f);
            #region move
            if (movingUp)
            {
                if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask) && Physics2D.RaycastAll(transform.position, transform.up, 1f, enemyMask).Length == 1)
                {
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position + transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
                else
                {
                    movingUp = false;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = true;
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position - transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
            }
            else
            {
                if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask) && Physics2D.RaycastAll(transform.position, -transform.up, 1f, enemyMask).Length == 1)
                {
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position - transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
                else
                {
                    movingUp = true;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = false;
                    Vector3 startingPos = transform.position;
                    Vector3 finalPos = transform.position + transform.up;
                    float time = .2f;
                    float elapsedTime = 0;

                    while (elapsedTime < time)
                    {
                        transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }
                    transform.position = finalPos;
                }
            }
            #endregion
            yield return new WaitForSeconds(.2f);

            #region attack
            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }


            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;
            #endregion

            gm.currentEnemy++;
            attacking = false;
        }

    }
    IEnumerator Attack2()
    {

        if (!attacking)
        {
            attacking = true;
            yield return new WaitForSeconds(.2f);
            #region move
            if (movingRight)
            {
                if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
                {
                    transform.position += Vector3.right;
                }
                else
                {
                    movingRight = false;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = true;
                    transform.position += -Vector3.right;
                }
            }
            else
            {
                if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
                {
                    transform.position += -Vector3.right;
                }
                else
                {
                    movingRight = true;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = false;
                    transform.position += Vector3.right;
                }
            }
            #endregion
            yield return new WaitForSeconds(.2f);

            #region attack
            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }


            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;
            #endregion
            yield return new WaitForSeconds(.2f);
            #region move
            if (movingUp)
            {
                if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
                {
                    transform.position += Vector3.up;
                }
                else
                {
                    movingUp = false;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = true;
                    transform.position += -Vector3.up;
                }
            }
            else
            {
                if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
                {
                    transform.position += -Vector3.up;
                }
                else
                {
                    movingUp = true;
                    spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipY = false;
                    transform.position += Vector3.up;
                }
            }
            #endregion
            yield return new WaitForSeconds(.2f);

            #region attack
            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }


            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;
            #endregion

            gm.currentEnemy++;
            attacking = false;
        }

    }
    IEnumerator Attack3()
    {
        if (!attacking)
        {
            attacking = true;
            if (player.transform.position.y > transform.position.y && Physics2D.RaycastAll(transform.position, transform.up, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.y < transform.position.y && Physics2D.RaycastAll(transform.position, -transform.up, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;

            }
            else if (player.transform.position.x > transform.position.x && Physics2D.RaycastAll(transform.position, transform.right, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.x < transform.position.x && Physics2D.RaycastAll(transform.position, -transform.right, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }



            attacking = true;
            if (player.transform.position.y > transform.position.y && Physics2D.RaycastAll(transform.position, transform.up, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.y < transform.position.y && Physics2D.RaycastAll(transform.position, -transform.up, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;

            }
            else if (player.transform.position.x > transform.position.x && Physics2D.RaycastAll(transform.position, transform.right, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.x < transform.position.x && Physics2D.RaycastAll(transform.position, -transform.right, 3f, enemyMask).Length == 1 && !Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }


            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }

            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;


            gm.currentEnemy++;
            attacking = false;
        }
    }
    IEnumerator Attack4()
    {
        if (!attacking)
        {

            attacking = true;
            if (gameObject.layer == 12 && isBoss)
            {
                gameObject.layer = 11;
                if (spriteRenderer.sprite == strongWhiteSkeleton)
                {
                    spriteRenderer.sprite = strongBlackSkeleton;
                }
            }
            else if (gameObject.layer == 11 && isBoss)
            {
                gameObject.layer = 12;
                if (spriteRenderer.sprite == strongBlackSkeleton)
                {
                    spriteRenderer.sprite = strongWhiteSkeleton;
                }
            }
            if (player.transform.position.y > transform.position.y)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.y < transform.position.y)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;

            }
            else if (player.transform.position.x > transform.position.x)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }


            if (!Physics2D.Raycast(transform.position, transform.up, 1f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 1f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 1f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 1f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 1f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 1f, playerMask))
            {
                gm.gameOver = true;
            }

            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;


            gm.currentEnemy++;
            attacking = false;
        }
    }
    IEnumerator Attack5()
    {
        if (!attacking)
        {

            attacking = true;
            if (gameObject.layer == 12 && isBoss)
            {
                gameObject.layer = 11;
                if (spriteRenderer.sprite == strongWhiteSkeleton)
                {
                    spriteRenderer.sprite = strongBlackSkeleton;
                }
                if (spriteRenderer.sprite == weakWhiteSkeleton)
                {
                    spriteRenderer.sprite = weakBlackSkeleton;
                }
            }
            else if (gameObject.layer == 11 && isBoss)
            {
                gameObject.layer = 12;
                if (spriteRenderer.sprite == strongBlackSkeleton)
                {
                    spriteRenderer.sprite = strongWhiteSkeleton;
                }
                if (spriteRenderer.sprite == weakWhiteSkeleton)
                {
                    spriteRenderer.sprite = weakBlackSkeleton;
                }
            }
            if (2 > transform.position.y - player.transform.position.y)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.y - transform.position.y < 2)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.up;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;

            }
            else if (player.transform.position.x > transform.position.x)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position + transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                Vector3 startingPos = transform.position;
                Vector3 finalPos = transform.position - transform.right;
                float time = .2f;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = finalPos;
            }


            if (!Physics2D.Raycast(transform.position, transform.up, 2f, antiPlayerMask))
            {
                redUp.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.up, 2f, antiPlayerMask))
            {
                redDown.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, -transform.right, 2f, antiPlayerMask))
            {
                redLeft.enabled = true;
            }

            if (!Physics2D.Raycast(transform.position, transform.right, 2f, antiPlayerMask))
            {
                redRight.enabled = true;
            }
            AttackSound();
            yield return new WaitForSeconds(.2f);

            if (Physics2D.Raycast(transform.position, transform.up, 4f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.up, 4f, playerMask) ||
                       Physics2D.Raycast(transform.position, transform.right, 4f, playerMask) ||
                       Physics2D.Raycast(transform.position, -transform.right, 4f, playerMask))
            {
                gm.gameOver = true;
            }

            yield return new WaitForSeconds(.2f);

            redUp.enabled = false;
            redDown.enabled = false;
            redLeft.enabled = false;
            redRight.enabled = false;


            gm.currentEnemy++;
            attacking = false;
        }
    }

    public void AttackSound()
    {
        audioSource.Play();

    }


}