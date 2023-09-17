using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class UserData
{
    public string username;
    public int level;

    public int exp, coin;
    public int[] scoreCnt;
    public float[] scoreAvr;
    public float[,,,] value;
    public int[,,,] policy;
    public float[,,,] reward;
    public int[,,,] iter_cnt;
    int num_state_levels = RL_Script.instance.num_state_levels; // 5
    int num_directions = RL_Script.instance.num_directions; // 2 * num_state_levels
    public UserData(){
        level = 1;
        exp = 0;
        coin = 0;
        scoreCnt = new int[3];
        scoreAvr = new float[3];

        value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        reward = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        policy = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        iter_cnt = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        value[i, j, k, l] = 0f;
                        reward[i, j, k, l] = 0f;
                        policy[i, j, k, l] = UnityEngine.Random.Range(1, num_directions + 1);
                        iter_cnt[i, j, k, l] = 0;
                    }
                }
            }
        }
    }
    
    public void setUsername(string name){
        username = name;
    }

    public void setData(string n, int l, int e, int c, int[] scoreC, float[] scoreA){
        username = n;
        level = l;
        exp = e;
        coin = c;
        scoreCnt = scoreC;
        scoreAvr = scoreA;
    }

    public void setRLData(float[,,,] v, float[,,,] r, int[,,,] p, int[,,,] ic){
        value = v;
        reward = r;
        policy = p;
        iter_cnt = ic;

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        if(iter_cnt[i,j,k,l] > 0){
                            Debug.Log(i.ToString() + j.ToString() + k.ToString() + l.ToString() + ":" + iter_cnt[i,j,k,l].ToString() );
                        }
                    }
                }
            }
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
