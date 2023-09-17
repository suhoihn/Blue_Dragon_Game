using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RL_Script : MonoBehaviour
{
    float gamma = 0.9f;
    int num_improve = 10;
    int num_eval_per_improve = 10;

    float epsilon;
    float epsilon_discount_factor = 0.99f;

    float collision_cost = -50f;

    int num_states = 4;
    public int num_state_levels = 5;
    public int num_directions;

    public float[,,,] value;
    public int[,,,] policy;
    public float[,,,] reward;
    public int[,,,] iter_cnt;

    public static RL_Script instance = null;

    string basePath;

    void Awake(){
        if(instance == null){
            instance = this;
        }    
        else if(instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void initialize(){
        epsilon = 0.95f;

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
                        policy[i, j, k, l] = Random.Range(1, num_directions + 1);
                        iter_cnt[i, j, k, l] = 0;
                    }
                }
            }
        }
    }

    void loadData(){
        string dataPath, temp;
        string[] valueList = new string[num_state_levels];
        string data = "";

        StreamReader reader;

        dataPath = basePath + "/value.txt";
        reader = new StreamReader(dataPath);
        // for(int i = 0; i < num_state_levels; i++){
        //     for(int j = 0; j < num_state_levels; j++){
        //         for(int k = 0; k < num_state_levels; k++){
        //             for(int l = 0; l < num_state_levels; l++){
        //                 float rw = PlayerPrefs.GetFloat("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString(),-1);
        //                 if(rw > 0f){Debug.Log(rw);Debug.Log("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString());}

        //                 float v = PlayerPrefs.GetFloat("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString(),-1);
        //                 if(v > 0f){Debug.Log(v);Debug.Log("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString());}

        //                 float pol = PlayerPrefs.GetFloat("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString(),-1);
        //                 if(pol > 0f){Debug.Log(pol);Debug.Log("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString());}

        //                 float itc = PlayerPrefs.GetFloat("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString(),-1);
        //                 if(itc > 0f){Debug.Log(itc);Debug.Log("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString());}

        //             }
        //         }
        //     }
        // }

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                temp = reader.ReadLine();
                for(int k = 0; k < num_state_levels; k++){
                    temp = reader.ReadLine();
                    valueList = temp.Split(",");
                    
                    // Debug.Log("valueList is...");
                    // for(int wef = 0; wef < valueList.Length; wef++){
                    //     Debug.Log(valueList[wef]);
                    // }
                    // Debug.Log("parsed to...");
                    // Debug.Log(float.Parse(valueList[0]));

                    for(int l = 0; l < num_state_levels; l++){
                        // Debug.Log(i.ToString()+j.ToString()+k.ToString()+l.ToString());
                        value[i,j,k,l] = PlayerPrefs.GetFloat("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0f);
                        //value[i,j,k,l] = float.Parse(valueList[l]);

                    }
                }

            }
        }

        dataPath = basePath + "/policy.txt";
        reader = new StreamReader(dataPath);
        
        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                temp = reader.ReadLine();
                for(int k = 0; k < num_state_levels; k++){
                    temp = reader.ReadLine();
                    valueList = temp.Split(",");

                    for(int l = 0; l < num_state_levels; l++){
                        //policy[i,j,k,l] = int.Parse(valueList[l]);//, CultureInfo.InvariantCulture.NumberFormat);
                        policy[i,j,k,l] = PlayerPrefs.GetInt("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0);
                    }
                }

            }
        }

        dataPath = basePath + "/reward.txt";
        reader = new StreamReader(dataPath);
        
        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                temp = reader.ReadLine();
                for(int k = 0; k < num_state_levels; k++){
                    temp = reader.ReadLine();
                    valueList = temp.Split(",");

                    for(int l = 0; l < num_state_levels; l++){
                        //reward[i,j,k,l] = float.Parse(valueList[l]);//, CultureInfo.InvariantCulture.NumberFormat);
                        reward[i,j,k,l] = PlayerPrefs.GetFloat("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0f);
                    }
                }

            }
        }


        dataPath = basePath + "/iter_cnt.txt";
        reader = new StreamReader(dataPath);
        
        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                temp = reader.ReadLine();
                for(int k = 0; k < num_state_levels; k++){
                    temp = reader.ReadLine();
                    valueList = temp.Split(",");

                    for(int l = 0; l < num_state_levels; l++){
                        //iter_cnt[i,j,k,l] = int.Parse(valueList[l]);//, CultureInfo.InvariantCulture.NumberFormat);
                        iter_cnt[i,j,k,l] = PlayerPrefs.GetInt("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0);
                    }
                }

            }
        }



    }

    public void saveData(){
        string dataPath;
        string data = "";
        // Debug.Log("Now printing...");
        // for(int i = 0; i < num_state_levels; i++){
        //     for(int j = 0; j < num_state_levels; j++){
        //         for(int k = 0; k < num_state_levels; k++){
        //             for(int l = 0; l < num_state_levels; l++){
        //                 if(value[i,j,k,l] > 0f){
        //                     Debug.Log(value[i,j,k,l]);
        //                 }
        //             }
        //         }
        //     }
        // }

        StreamWriter writer;

        dataPath = basePath + "/value.txt";
        writer = File.CreateText(dataPath);

        Debug.Log("saving");


        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n";

                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        data += value[i, j, k, l].ToString("F1") + ",";
                        PlayerPrefs.SetFloat("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), value[i,j,k,l]);
                    }
                    data += "\n";
                }
            }
        }

        //writer.WriteLine(data);
        writer.Close();

        // reward
        dataPath = basePath + "/reward.txt";
        writer = File.CreateText(dataPath);
        data = "";

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n";

                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        data += reward[i, j, k, l].ToString("F1") + ",";
                        PlayerPrefs.SetFloat("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), reward[i,j,k,l]);

                    }
                    data += "\n";
                }
            }
        }

        //writer.WriteLine(data);
        writer.Close();

        dataPath = basePath + "/policy.txt";
        writer = File.CreateText(dataPath);
        data = "";

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n";

                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        data += policy[i, j, k, l].ToString() + ",";
                        PlayerPrefs.SetFloat("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), policy[i,j,k,l]);

                    }
                    data += "\n";
                }
            }
        }

        //writer.WriteLine(data);
        writer.Close();


        dataPath = basePath + "/iter_cnt.txt";
        writer = File.CreateText(dataPath);
        data = "";

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n";

                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        data += iter_cnt[i, j, k, l].ToString() + ",";
                        PlayerPrefs.SetFloat("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), iter_cnt[i,j,k,l]);

                    }
                    data += "\n";
                }
            }
        }

        //writer.WriteLine(data);
        writer.Close();
    }

    void Start()
    {
        num_directions = 2 * num_states;
        basePath = Application.dataPath + "/data";
        // initialize();
        // loadData();
        value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        reward = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        policy = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        iter_cnt = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];

        int dataExists = PlayerPrefs.GetInt("dataExists",0);
        if(dataExists == 1){loadData();}
        else{
            initialize();
            saveData();
            PlayerPrefs.SetInt("dataExists",1);
        }

    }

    public void policy_iteration(){
        float[,,,] new_value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        new_value[i, j, k, l] = 0f;
                    }
                }
            }
        }

        for(int aa = 0; aa < num_improve; aa++){
            for(int bb = 0; bb < num_eval_per_improve; bb++){
                
                new_value = policy_eval(policy, value, reward);
                for(int i = 0; i < num_state_levels; i++){
                    for(int j = 0; j < num_state_levels; j++){
                        for(int k = 0; k < num_state_levels; k++){
                            for(int l = 0; l < num_state_levels; l++){
                                value[i, j, k, l] = new_value[i, j, k ,l];
                            }
                        }
                    }
                }


            }

            policy = policy_improve(value, reward);
        }

    }

    bool check_collision(int[] state, int direction){
        if(direction == 1 && state[0] == num_state_levels - 1){ return true; }
        else if(direction == 2 && state[0] == 0){ return true; }
        else if(direction == 3 && state[1] == num_state_levels - 1){ return true; }
        else if(direction == 4 && state[1] == 0){ return true; }
        else if(direction == 5 && state[2] == num_state_levels - 1){ return true; }
        else if(direction == 6 && state[2] == 0){ return true; }
        else if(direction == 7 && state[3] == num_state_levels - 1){ return true; }
        else if(direction == 8 && state[3] == 0){ return true; }

        return false;
    }

    int[] update_state(int[] state, int direction){
        int[] next_state = new int[num_states];

        for(int i = 0; i < num_states; i++){
            next_state[i] = state[i];
        }

        switch(direction){
            case 1:
                next_state[0] += 1;
                break;
            case 2:
                next_state[0] -= 1;
                break;
            case 3:
                next_state[1] += 1;
                break;
            case 4:
                next_state[1] -= 1;
                break;
            case 5:
                next_state[2] += 1;
                break;
            case 6:
                next_state[2] -= 1;
                break;
            case 7:
                next_state[3] += 1;
                break;
            case 8:
                next_state[3] -= 1;
                break;
            
        }

        for(int i = 0; i < num_states; i++){
            next_state[i] = Mathf.Clamp(next_state[i], 0, num_state_levels - 1);
        }
        return next_state;
    }

    public void calc_next_state(int map_speed_level, int map_delay_level, int map_obstacle_level, int map_angle_level){
        int updated_policy = policy[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        int next_map_speed_level = map_speed_level;
        int next_map_delay_level = map_delay_level;
        int next_map_obstacle_level = map_obstacle_level;
        int next_map_angle_level = map_angle_level;

        float random_v = Random.Range(0.0f, 1.0f);
        epsilon = epsilon * epsilon_discount_factor;

        if(random_v < epsilon){
            updated_policy = Random.Range(1, num_directions + 1);
        }

        switch(updated_policy){
            case 1:
                next_map_speed_level += 1;
                break;
            case 2:
                next_map_speed_level -= 1;
                break;
            case 3:
                next_map_delay_level += 1;
                break;
            case 4:
                next_map_delay_level -= 1;
                break;
            case 5:
                next_map_obstacle_level += 1;
                break;
            case 6:
                next_map_obstacle_level -= 1;
                break;
            case 7:
                next_map_angle_level += 1;
                break;
            case 8:
                next_map_angle_level -= 1;
                break;
                
        }

        if (random_v < epsilon){
            next_map_speed_level += -1 + 2 * Random.Range(0, 2);
            next_map_delay_level += -1 + 2 * Random.Range(0,2);
            next_map_obstacle_level += -1 + 2 * Random.Range(0,2);
            next_map_angle_level += -1 + 2 * Random.Range(0,2);
        }

        next_map_speed_level = Mathf.Clamp(next_map_speed_level, 0, num_state_levels - 1);
        next_map_delay_level = Mathf.Clamp(next_map_delay_level, 0, num_state_levels - 1);
        next_map_obstacle_level = Mathf.Clamp(next_map_obstacle_level, 0, num_state_levels - 1);
        next_map_angle_level = Mathf.Clamp(next_map_angle_level, 0, num_state_levels - 1);

        PlayerPrefs.SetInt("map_speed_level", next_map_speed_level);
        PlayerPrefs.SetInt("map_delay_level", next_map_delay_level);
        PlayerPrefs.SetInt("map_obstacle_level", next_map_obstacle_level);
        PlayerPrefs.SetInt("map_angle_level", next_map_angle_level);

    }

    float[,,,] policy_eval(int[,,,] policy, float[,,,] value, float[,,,] reward){
        float[,,,] new_value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        int[] next_state = new int[num_states];
        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        next_state = update_state(new int[4]{i,j,k,l}, policy[i,j,k,l]);
                        new_value[i,j,k,l] = reward[i,j,k,l] + gamma * value[next_state[0], next_state[1], next_state[2], next_state[3]];
                    }
                }
            }
        }
        
        return new_value;
    }
    
    int[,,,] policy_improve(float[,,,] value, float[,,,] reward){
        int[] next_state;
        int[,,,] new_policy = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        float max_value;
        bool is_collision;

        for(int i = 0; i < num_state_levels; i++){
            for(int j = 0; j < num_state_levels; j++){
                for(int k = 0; k < num_state_levels; k++){
                    for(int l = 0; l < num_state_levels; l++){
                        max_value = -999999999999;

                        for(int u = 1; u < num_directions + 1; u++){
                            next_state = update_state(new int[4]{i,j,k,l},u);
                            is_collision = check_collision(new int[4]{i,j,k,l},u);

                            float v = gamma * value[next_state[0], next_state[1], next_state[2], next_state[3]];
                            if(is_collision) { v += collision_cost; }

                            if(v > max_value){
                                max_value = v;
                                new_policy[i,j,k,l] = u;
                            }
                        }

                    }
                }
            }
        }

        return new_policy;
    }


    void Update()
    {
        
    }
}
