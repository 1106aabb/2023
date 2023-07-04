using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f;
    GameObject currentFloor;
    [SerializeField] int HP;
    [SerializeField] GameObject HPBar;
    [SerializeField] Text scoreText;
    int score;
    float scoreTime;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;

    // Start is called before the first frame update
    void Start()
    {
        HP = 10;
        UpdateHPBar();
        score = 0;
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        UpdateScore();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                ModifyHP(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                ModifyHP(-3);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "ceiling1")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHP(-2);
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (other.gameObject.tag == "ceiling2")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHP(-2);
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "deadline")
        {
            currentFloor = other.gameObject;
            Die();
        }
    }

    void ModifyHP(int num)
    {
        HP += num;
        if (HP > 10)
        {
            HP = 10;
        }
        else if (HP <= 0)
        {
            HP = 0;
            Die();
        }
        UpdateHPBar();
    }

    void UpdateHPBar()
    {
        for (int i = 0; i < HPBar.transform.childCount; i++)
        {
            if (HP > i)
            {
                HPBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HPBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }    
    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
            if (scoreTime > 2f)
            {
                score++;
                scoreTime = 0;
                scoreText.text = "地下" + score.ToString() + "層";
            }
    }

    void Die()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
}    


