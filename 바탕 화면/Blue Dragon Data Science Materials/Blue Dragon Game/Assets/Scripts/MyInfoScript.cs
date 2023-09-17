using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoScript : MonoBehaviour
{
    Text avgEzText, avgNmText, avgHdText, cntEzText, cntNmText, cntHdText;

    float[] avg;
    int[] cnt;

    void Start()
    {
        cnt = new int[3];
        avg = new float[3];

        for(int i = 0; i < 3; i++){
            cnt[i] = PlayerPrefs.GetInt("ScoreCnt" + i.ToString(), 0);
            avg[i] = PlayerPrefs.GetFloat("ScoreAvr" + i.ToString(), 0);
        }

        avgEzText = GameObject.Find("EasyAverageText").GetComponent<Text>();
        avgNmText = GameObject.Find("NormalAverageText").GetComponent<Text>();
        avgHdText = GameObject.Find("HardAverageText").GetComponent<Text>();

        cntEzText = GameObject.Find("EasyCountText").GetComponent<Text>();
        cntNmText = GameObject.Find("NormalCountText").GetComponent<Text>();
        cntHdText = GameObject.Find("HardCountText").GetComponent<Text>();


    }

    // Update is called once per frame
    void Update()
    {
        if(cnt[0] == 0){avgEzText.text = "-";}
        else{avgEzText.text = Mathf.Round(avg[0]).ToString();}

        if(cnt[1] == 0){avgNmText.text = "-";}
        else{avgNmText.text = Mathf.Round(avg[1]).ToString();}

        if(cnt[2] == 0){avgHdText.text = "-";}
        else{avgHdText.text = Mathf.Round(avg[2]).ToString();}

        cntEzText.text = cnt[0].ToString();
        cntNmText.text = cnt[1].ToString();
        cntHdText.text = cnt[2].ToString();

    }
}
