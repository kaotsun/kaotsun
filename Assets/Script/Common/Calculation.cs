using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculation : MonoBehaviour
{
    // データ関連
    public GameObject data_relation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 抽選関数
    void Lottery()
    {

    }

    // クレジット追加
    public int AddCredit(int orijin_credit, int add_credit)
    {
        int result_credit = orijin_credit + add_credit;
        // ここにクレジット上書きファンクション呼び出し入れる
        return result_credit;
    }


    // クレジット減少
    public int UseCredit(int orijin_credit, int use_credit)
    {
        int result_credit = orijin_credit - use_credit;
        // data_relation.GetComponent<DataRelation>().SetCredit(result_credit);

        // ここにクレジット上書きファンクション呼び出し入れる
        return result_credit;
    }

    public int DecideProbability(List<int> _setting_distribution)
    {
        int rand_num = Random.Range(1, 100);
        int result;

        if (rand_num <= _setting_distribution[0])
        {
            result = 0;
        }
        else if (rand_num <= _setting_distribution[1])
        {
            result = 1;
        }
        else if (rand_num <= _setting_distribution[2])
        {
            result = 2;
        }
        else if (rand_num <= _setting_distribution[3])
        {
            result = 3;
        }
        else if (rand_num <= _setting_distribution[4])
        {
            result = 4;
        }
        else if (rand_num <= _setting_distribution[5])
        {
            result = 5;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    public int GameResult(int _type, int _big, int _reg)
    {
        int total_parameter = _big * _reg;
        int rand_num = Random.Range(1, total_parameter);
        int big = _reg;
        int reg = _big + _reg;
        int result = 0;

        Debug.Log(total_parameter);
        Debug.Log(big);
        Debug.Log(reg);
        Debug.Log("-----乱数----");
        Debug.Log(rand_num);

        if (rand_num < big)
        {
            // big
            result = 1;
        }
        else if (rand_num < reg)
        {
            // reg
            result = 2;
        }
        Debug.Log("-----結果----");
        Debug.Log(result);
        return result;
    }
}
