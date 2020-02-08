using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySaveData : MonoBehaviour
{

    // 台のタイプ：フリー > 0, 実践 > 1
    private static int type = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // タイプをセット
    public void SetType(int select_type)
    {
        type = select_type;
    }

    // タイプを取得
    public int GetType()
    {
        return type;
    }

}
