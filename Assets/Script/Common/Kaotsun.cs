using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaotsun : MonoBehaviour
{
    public const int FREE_GAME = 0; // フリー
    public const int PRACTICE_GAME = 1; // 実践

    // 確率{ 131, 111, 136, 161, 178, 190};
    public List<int> probability_big = new List<int> { 248, 242, 235, 229, 223, 210 };
    public List<int> probability_reg = new List<int> { 255, 250, 239, 234, 226, 210 };
    public List<int> probability_setting = new List<int> { 5, 15, 47, 74, 92, 100};

    // 払い出しBIG
    public const int payout_big = 200;

    // 払い出しREG
    public const int payout_reg = 50;

    public const int BIG = 1;
    public const int REG = 2;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    // ランプの色を変更
    public string GetSound(int _bouns_type){
        string _sound_path = "";

        switch(_bouns_type)
        {
            case BIG:
                _sound_path = "Sound/thankskaotsun";
                break;
            case REG:
                _sound_path = "Sound/kasaihirokisan";
                break;
        }

        return _sound_path;
    }

    public int GetBonus(int _result){
        int get_credit = 0;
        switch (_result){
            case BIG:
                get_credit = payout_big;
                break;
            case REG:
                get_credit = payout_reg;
                break;
        }
        return get_credit;
    }

    // フリータイプを取得
    public int GetTypeFREE()
    {
        return FREE_GAME;
    }
    // 実践タイプを取得
    public int GetTypePRACTICE()
    {
        return PRACTICE_GAME;
    }
}
