using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource theMusic;
    public bool startPlaying;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;

    public Text scoreText;
    public Text multiText;
    public Text trackText;
    public Text trackLengthText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    float totalMins;
    float totalSecs;

    float currentMins;
    float currentSecs;

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        totalNotes = FindObjectsOfType<NoteObject>().Length;
        trackText.text = theMusic.name;

        //float musicLength = theMusic.clip.length;

        //totalMins = Mathf.Floor(musicLength / 60);
        //totalSecs = Mathf.Floor(musicLength % 60);

        //trackLengthText.text = "0:00 | " + theMusic.clip.length.ToString("0:00");
        //trackLengthText.text = "0:00 | " + totalMins.ToString("00") + ":"  + totalSecs.ToString("00");
        trackLengthText.text = "0:00 | " + GetTotalTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if(Input.anyKeyDown)
            {
                startPlaying = true;
                theBS.hasStarted = true;

                theMusic.Play();
            }
        }
        else
        {
            if(!theMusic.isPlaying && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);

                normalsText.text = "" + normalHits;
                goodsText.text = goodHits.ToString();
                perfectsText.text = perfectHits.ToString();
                missesText.text = "" + missedHits;

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (totalHit / totalNotes) * 100f;

                percentHitText.text = percentHit.ToString("F1") + "%";
                string rankVal = "F";

                if(percentHit > 40)
                {
                    rankVal = "D";
                    if(percentHit > 55)
                    {
                        rankVal = "C";
                        if(percentHit > 70)
                        {
                            rankVal = "B";
                            if(percentHit > 85)
                            {
                                rankVal = "A";
                                if(percentHit > 95)
                                {
                                    rankVal = "S";
                                }
                            }
                        }
                    }
                }
                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();
                
            }
        }
        if(theMusic.isPlaying)
        {


            //trackLengthText.text = theMusic.time.ToString("0:00") + " | " + theMusic.clip.length.ToString("0:00");
            //trackLengthText.text = GetCurrentTime() + " | " + theMusic.clip.length.ToString("0:00");
            trackLengthText.text = GetCurrentTime() + " | " + GetTotalTime();
        }
    }

    string GetCurrentTime()
    {
        currentMins = Mathf.Floor(theMusic.time / 60);
        currentSecs = Mathf.Floor(theMusic.time % 60);

        string outputTime = currentMins.ToString("00") + ":" + currentSecs.ToString("00");
        return outputTime;
    }

    string GetTotalTime()
    {
        totalMins = Mathf.Floor(theMusic.clip.length / 60);
        totalSecs = Mathf.Floor(theMusic.clip.length % 60);

        string outputTime = totalMins.ToString("00") + ":" + totalSecs.ToString("00");
        return outputTime;
    }

    public void NoteHit()
    {
        Debug.Log("Hit On Time");

        if(currentMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier.ToString(); 
        //currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        normalHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiText.text = "Multiplier: x" + currentMultiplier.ToString();
        missedHits++;
    }
}
