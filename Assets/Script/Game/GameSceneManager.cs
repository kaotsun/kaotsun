using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // 一時保存データ
    public GameObject TemporarySaveData;

    // データ関連
    public GameObject data_relation;

    // 計算関連
    public GameObject calculation;

    // クレジット表示用テキスト
    public GameObject credite_text_object;

    // クレジットベットアニメーション
    public GameObject credite_bet_anim;

    // クレジットベットアニメーション
    public GameObject get_bonus_text_anim;

    // Tapアニメエフェクト
    public GameObject tapTextAnim;

    // 所持クレジット
    public int my_credit = 0;

    // 台情報
    public GameObject KaotsunCS;

    // ゲームタイプスクリプト格納用
    private List<int> probability_setting_distribution;
//    private List<int> big_probability_distribution;
//    private List<int> reg_probability_distribution;
    private int big_probability;
    private int reg_probability;

    // 台type
    private int type = 1;

    // 台の設定
    private int type_setting = 0;

    // かおつんボタン関連
    public AudioClip audioClip;
    private AudioSource audioSource;
    public bool isPlayAudio = false;
    public GameObject Kaotsun;

    // 台の設定表
    public GameObject SettingSheets;

    public float timeOut = 5.0f;
    private float timeTrigger;

    // Start is called before the first frame update
    void Start()
    {
        type = TemporarySaveData.GetComponent<TemporarySaveData>().GetType();
        Debug.Log("設定");
        Debug.Log(type);

        if (type != 0) {
            probability_setting_distribution = KaotsunCS.GetComponent<Kaotsun>().probability_setting;

            // 台設定を決める
            SetTypeSetting();

            big_probability = KaotsunCS.GetComponent<Kaotsun>().probability_big[type_setting];
            reg_probability = KaotsunCS.GetComponent<Kaotsun>().probability_reg[type_setting];
        }

        // 所持クレジットを取得する
        my_credit = GetCredite();

        // ゲーム画面をセットする(クレジット枚数)
        SetCrediteText(my_credit);

        // 試行可能判定
        if (IsBetValidity())
        {
            UnlockPlyaAudio();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeTrigger)
        {
            if (isPlayAudio)
            {
                tapTextAnim.SetActive(true);
            }

            timeTrigger = Time.time + timeOut;
        }

    }

    // 戻るボタン
    public void OnClickBack()
    {
        SceneManager.LoadScene("TopScene");
    }

    // 台の設定を変更
    private void SetTypeSetting() {
        type_setting = calculation.GetComponent<Calculation>().DecideProbability(probability_setting_distribution);
        Debug.Log("確率設定");
        Debug.Log(type_setting);
    }

    // クレジットを取得する
    private int GetCredite()
    {
        return data_relation.GetComponent<DataRelation>().GetCredit();
    }

    // クレジット数値をセットする
    private void SetCrediteText(int num)
    {
        Debug.Log(credite_text_object);
        credite_text_object.GetComponent<Text> ().text = num.ToString();
    }

    private void SetCredite(int num)
    {
        data_relation.GetComponent<DataRelation>().SetCredit(num);
    }

    // クレジットチェック
    private bool IsBetValidity()
    {
        return (my_credit > 0) ? true : false;
    }

    // クレジット消費
    private void ConsumptionCredite()
    {
        my_credit = calculation.GetComponent<Calculation>().UseCredit(my_credit, 1);
    }




    /**** ----- かおつんボタン ----- ****/
    // かおつんボタンが押された場合、今回呼び出される関数
    public void OnClickKaotsun()
    {
        Debug.Log("----");

        Debug.Log(type);
        Debug.Log(isPlayAudio);
        Debug.Log("----");

        // TapAnim止める
        tapTextAnim.SetActive(false);
        //tapTextAnim.GetComponent<Animator>().SetBool("IsTap", true);

        if (isPlayAudio && IsBetValidity())
        {
            // ロック
            LockPlyaAudio();

            // クレジット消費
            ConsumptionCredite();
            credite_bet_anim.GetComponent<Animator>().SetBool("Bet", true);

            // 抽選結果
            int result = GameResult();
            Debug.Log("結果");
            Debug.Log(result);

            // シミュレーション or 大当り
            if (type == 0)
            {
                Debug.Log("シミュレーション");
                PlayAudio(1);
                Invoke("EndEffectBet", 0.5f);
                Invoke("EndEffectTextAnim", 1.0f);
                Invoke("EndEffect", 3.5f);

                // ゲットアニメーションテキスト
                get_bonus_text_anim.GetComponent<Animator>().SetInteger("GetBonus", 1);

                // Bonus時かおつんアニメーション
                Kaotsun.GetComponent<Animator>().SetInteger("BonusType", 1);
                int get_credit = KaotsunCS.GetComponent<Kaotsun>().GetBonus(1);
                my_credit = my_credit + get_credit;
            } else if(result != 0)
            {
                Debug.Log("大当り");
                PlayAudio(result);
                Invoke("EndEffectBet", 0.5f);
                Invoke("EndEffectTextAnim", 1.0f);
                Invoke("EndEffect", 3.5f);

                // ゲットアニメーションテキスト
                get_bonus_text_anim.GetComponent<Animator>().SetInteger("GetBonus", result);

                // Bonus時かおつんアニメーション
                Kaotsun.GetComponent<Animator>().SetInteger("BonusType", result);
                int get_credit = KaotsunCS.GetComponent<Kaotsun>().GetBonus(result);
                my_credit = my_credit + get_credit;

            } else {
                Debug.Log("ハズレ");

                // Bonus時かおつんアニメーション
                Kaotsun.GetComponent<Animator>().SetInteger("BonusType", 3);


                Invoke("EndEffect", 0.4f);
                Invoke("EndEffectBet", 0.5f);
            }

            // クレジット更新
            SetCredite(my_credit);
            SetCrediteText(my_credit);
        }
    }

    // 抽選
    private int GameResult()
    {
       return calculation.GetComponent<Calculation>().GameResult(type, big_probability, reg_probability);
    }

    // エフェクトを終了
    private void EndEffect()
    {
        Kaotsun.GetComponent<Animator>().SetInteger("BonusType", 0);
        UnlockPlyaAudio();
    }

    // betエフェクトを終了
    private void EndEffectBet() {
        credite_bet_anim.GetComponent<Animator>().SetBool("Bet", false);
    }

    // bonus textエフェクトを終了
    private void EndEffectTextAnim(){
        get_bonus_text_anim.GetComponent<Animator>().SetInteger("GetBonus", 0);
    }

    // ボタン押下フラグを解除
    private void UnlockPlyaAudio()
    {
        isPlayAudio = true;
    }

    // ボタン押下フラグをロック
    private void LockPlyaAudio()
    {
        isPlayAudio = false;
    }

    // プレイAudio
    private void PlayAudio(int _audio_type)
    {
        audioClip = Resources.Load(KaotsunCS.GetComponent<Kaotsun>().GetSound(_audio_type)) as AudioClip;
        audioSource = Kaotsun.AddComponent<AudioSource>();
        Debug.Log(audioClip);
        audioSource.clip = audioClip;

        audioSource.Play();
    }

    /**  informationボタン押下　 **/
    public void OnClickInfo(){
        Debug.Log("通った");
        SettingSheets.SetActive(!SettingSheets.activeInHierarchy);
    }

}
