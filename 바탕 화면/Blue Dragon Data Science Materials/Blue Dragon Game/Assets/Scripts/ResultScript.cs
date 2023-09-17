using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResultScript : MonoBehaviour
{
    // Start is called before the first frame update

    Text modeText, timeText, wordsText, coinText, coinGainText, expText, levelText;

    string mode;
    int time, words, totalCoin, coinGain, exp, level, score, levelGain, previousExp;
    int expGain;
    int map_speed_level, map_delay_level, map_obstacle_level, map_angle_level;

    int difficulty_level = -1;

    void Start()
    {
        mode = PlayerPrefs.GetString("modeName", "default mode");
        time = (int)PlayerPrefs.GetFloat("elapsedTime", 0f);
        words = PlayerPrefs.GetInt("completedWords", 0);

        totalCoin = PlayerPrefs.GetInt("coinCount", 0);
        coinGain = PlayerPrefs.GetInt("coinGainCount", 0);

        level = PlayerPrefs.GetInt("level", 0);

        exp = PlayerPrefs.GetInt("exp", 0);
        previousExp = exp;
        exp += calculateExp(words, time);
        score = calculateExp(words, time);
        while (exp >= 100){
            exp -= 100;
            level++;
            levelGain++;
        }
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("exp", exp);


        modeText = GameObject.Find("ModeText").GetComponent<Text>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        wordsText = GameObject.Find("CompletedWordsText").GetComponent<Text>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        coinGainText = GameObject.Find("CoinGainText").GetComponent<Text>();
        expText = GameObject.Find("ExpText").GetComponent<Text>();
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        

        RL_Script _RLScript = GameObject.Find("RLManager").GetComponent<RL_Script>();

        map_speed_level = PlayerPrefs.GetInt("map_speed_level",2);
        map_delay_level = PlayerPrefs.GetInt("map_delay_level",2);
        map_obstacle_level = PlayerPrefs.GetInt("map_obstacle_level",2);
        map_angle_level = PlayerPrefs.GetInt("map_angle_level",2);

        difficulty_level = (int) (((float) (map_speed_level + map_delay_level + map_obstacle_level + map_obstacle_level) / 16) * 3);
        difficulty_level = Mathf.Clamp(difficulty_level, 0, 2);

        Debug.Log("D LEVEL: " + difficulty_level.ToString());   
        int tempCnt = PlayerPrefs.GetInt("ScoreCnt" + difficulty_level.ToString(), 0);
        float tempAvr = PlayerPrefs.GetFloat("ScoreAvr" + difficulty_level.ToString(), 0);

        PlayerPrefs.SetInt("ScoreCnt" + difficulty_level.ToString(), tempCnt + 1);
        PlayerPrefs.SetFloat("ScoreAvr" + difficulty_level.ToString(), (tempAvr * tempCnt + expGain) / (tempCnt + 1));
        
        // Moved
        updateTotalCoin();
        // UserManager.userInstance.updateData();
        // UserManager.userInstance.saveJson();

        if(mode != "Mode 2"){return;}

        float prev_reward, next_reward;
        int total_simul_cnt;

        float reward_from_score = 1000f / (score + (float)1e-7);
        if(score < 1){//50
            reward_from_score = 0f;
        }
        reward_from_score = Mathf.Clamp(reward_from_score, 0f, 100f);

        total_simul_cnt = _RLScript.iter_cnt[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        prev_reward = _RLScript.reward[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        next_reward = (prev_reward * total_simul_cnt + reward_from_score) / (total_simul_cnt + 1);
        
        _RLScript.reward[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level] = next_reward;
        _RLScript.iter_cnt[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level] += 1;

        _RLScript.policy_iteration();

        _RLScript.calc_next_state(map_speed_level, map_delay_level, map_obstacle_level, map_angle_level);

        string dataPath = Application.dataPath + "/simpleTest.txt";
        StreamWriter writer;
        writer = File.CreateText(dataPath);
        writer.WriteLine("0.125");
        writer.WriteLine("Lv: " + level.ToString() + ", Exp: " + exp.ToString());
        writer.Close();

        StreamReader reader = new StreamReader(dataPath);
        string testString = reader.ReadLine();
        Debug.Log("loaded: " + float.Parse(testString).ToString());



        _RLScript.saveData();
        // UserManager.userInstance.updateData();
        // UserManager.userInstance.saveJson();

    }

    int calculateExp(int w, int t){
        expGain = t * 1 + w * 10;
        return expGain;
    }

    public void updateTotalCoin(){
        PlayerPrefs.SetInt("coinCount", totalCoin + coinGain);
    }

    void Update()
    {
        modeText.text = mode;

        timeText.text = "Time: " + (time / 60).ToString() + " minutes " + (time % 60).ToString() + " seconds";
        coinText.text = "$ " + totalCoin.ToString();
        coinGainText.text = "+ " + coinGain.ToString(); 
        expText.text = "Exp: " + previousExp.ToString() + " + " + expGain.ToString();
        levelText.text = "Level: " + level.ToString() + " + " + levelGain.ToString();
        wordsText.text = "Completed Words: " + words.ToString(); 
    }
}
