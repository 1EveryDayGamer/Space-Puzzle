using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    public float t;
    public float t3;
    public bool t2;
    public string timer;
    public int levelTime;
    public int levelNum = 1;
    public enum GamePlay
    {
        play,
        stop
    }
    public GamePlay state = GamePlay.play;
    // Start is called before the first frame update
    void Start()
    {

       

    }
    public void Levels()
    {
        var s = FindObjectOfType<ShipAssembly>();
        if (s.assembled == 5)
        {
            levelNum += 1;
            var g = FindObjectOfType<GameText>();
            g.GetComponent<Text>().text = ("Awesome Job! Onto Level :" + levelNum);
        }
    }

    public void Timer()
    {

        if (state == GamePlay.play)
        {
            t += Time.deltaTime;
        }
        //int score1 = (int)t / 60;//used to increase the score based on lengthier run
        //int score2 = (int)t % 60;
        //t3 = (score1 * 275) + (score2 * 5);
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");
        timer = (minutes + ":" + seconds);//sends this info to timer
        if ((int)t % 60 == 10)
        {
            t2 = false;
        }
        if ((int)t % 60 == 59 && t2 == false)
        {
           
            levelTime += 1;
            t2 = true;

                Debug.Log(levelTime);
            if (levelTime == 5)
            {
                state = GamePlay.stop;
                var g = FindObjectOfType<GameText>();
                g.GetComponent<Text>().text = ("Youre out of time");
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        GetComponentInChildren<TextMeshProUGUI>().text = ("Time: \n" + timer);
    }
}
