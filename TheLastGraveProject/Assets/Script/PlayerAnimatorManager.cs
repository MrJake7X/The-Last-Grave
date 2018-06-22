using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAnimatorManager : MonoBehaviour {

    private Animator anim;

    public GameObject farolillo;

    public Animator animAngel;

    public GameObject enemiesCinematica;

    public GameObject enemies;

    private SoundPlayer sounds;

    [Header("ToBeContinue Settings")]
    public Image panel;
    public Image image;
    Color c = Color.black;
    Color w = Color.white;
    private bool fadeIn;
    private float counter;

	void Start ()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        sounds = GetComponent<SoundPlayer>();
        c.a = 0;
        w.a = 0;
	}

    private void Update()
    {
        panel.color = c;
        image.color = w;
        if (fadeIn)
        {
            c.a += 1 * Time.deltaTime;
            w.a += 1 * Time.deltaTime;
        }
        if(c.a >= 1)
        {
            counter += 1 * Time.deltaTime;
            if(counter >= 4)
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void StartFinal()
    {
        anim.enabled = true;
        anim.Play("Animacion_Final");
        Destroy(enemies);
    }

    public void SpawnEnemies()
    {
        enemiesCinematica.SetActive(true);
    }

    public void ExitFinal()
    {
        fadeIn = true;
    }

    public void StartCinematic2()
    {
        anim.enabled = true;
        anim.Play("Animacion_Farolillo_2");
    }

    public void MoveAngel()
    {
        animAngel.enabled = true;
        sounds.Play(0, 1);
    }

    public void ExitCinematic2()
    {
        Destroy(sounds);
        anim.enabled = false;
    }

    public void StartCinematic()
    {
        anim.enabled = true;
        anim.Play("Animacion_Farolillo");
    }

    public void DestroyFarolillo()
    {
        Destroy(farolillo);
    }

    public void ExitCinematic()
    {
        anim.enabled = false;
    }
}