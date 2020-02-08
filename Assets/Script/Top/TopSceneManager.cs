using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TopSceneManager : MonoBehaviour
{
    private BannerView bannerView;

    // 一時保存データ
    public GameObject TemporarySaveData;

    // かおつん
    public GameObject KaotsunSC;

    // データ関連
    public GameObject data_relation;

    // ためるボタン
    public GameObject btn_tameru;

    private RewardedAd rewardedAd;

    // Start is called before the first frame update
    void Start()
    {
        SetFirstCredit();

        // データチェック
        if (!IsCreditData())
        {
            // データがない場合のセット
            SetFirstCredit();
        }

        // 所持クレジット取得
        int _my_credit = data_relation.GetComponent<DataRelation>().GetCredit();
        Debug.Log(_my_credit);

        // 現在時刻
        string _now_date = DateTime.Now.ToString("yyyyMMdd");

        // 更新日が今日じゃない かつ クレジットが規定枚数以下
        if (IsAddCreditByUpdateDate(_now_date) && IsAddCreditByCredit(_my_credit))
        {
            // 規定枚数以下の場合のクレジット処理
            SetFirstCredit();
        }

        // 動画広告初期判定
        if (data_relation.GetComponent<DataRelation>().IsAdMov())
        {
            var AdMobDate = data_relation.GetComponent<DataRelation>().GetAdMovUpdateDate();
            // 本日の動画広告再生済判定
            if(AdMobDate != _now_date)
            {
                btn_tameru.GetComponent<Image>().sprite = Resources.Load<Sprite>("btn_s_tameru");
            } else {
                btn_tameru.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        } else {
            btn_tameru.GetComponent<Image>().sprite = Resources.Load<Sprite>("btn_s_tameru");
        }

        // UNITY_ANDROID
        string appId = "ca-app-pub-9934448897883222~2632737663";
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        this.RequestBanner();

        // testリワード
        //string adUnitRewardId = "ca-app-pub-3940256099942544/5224354917";

        // 本番リワード
        string adUnitRewardId = "ca-app-pub-9934448897883222/7914297585";

        this.rewardedAd = new RewardedAd(adUnitRewardId);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RequestBanner()
    {
        // UNITY_ANDROID

        // 本番バナー
        string adUnitId = "ca-app-pub-9934448897883222/6188839294";

        // testバナー
        // string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    // ボタンが押された場合、今回呼び出される関数
    public void OnClickStartPractice()
    {
        int type = KaotsunSC.GetComponent<Kaotsun>().GetTypePRACTICE();
        TemporarySaveData.GetComponent<TemporarySaveData>().SetType(type);
        Debug.Log("設定");
        Debug.Log(type);
        SceneManager.LoadScene("GameScene");
    }

    // クレジットデータ有無チェック
    private bool IsCreditData()
    {
        return data_relation.GetComponent<DataRelation>().IsCreditData();
    }

    // 初回のクレジット登録
    private void SetFirstCredit()
    {
        data_relation.GetComponent<DataRelation>().SetFirstCredit();
    }

    // クレジット更新日判定
    private bool IsAddCreditByUpdateDate(string _now)
    {
        // 更新日取得
        string _update_date = data_relation.GetComponent<DataRelation>().GetFirstCreditDate();
        return (_now != _update_date) ? true : false;
    }

    // クレジット枚数判定
    private bool IsAddCreditByCredit(int _credit)
    {
        return (_credit < data_relation.GetComponent<DataRelation>().daily_spending) ? true : false;
    }

    // 設定リセット判定(更新日)
    private bool IsResetSettingByUpdateDate(string _now, int _type)
    {
        // 更新日取得
        string _update_date = data_relation.GetComponent<DataRelation>().GetProbabilitySettingUpdateDate(_type);
        return (_now != _update_date) ? true : false;
    }


    /***交換ボタン***/
    // 交換ページへ遷移
    public void OnClickMoveChangePage()
    {
        SceneManager.LoadScene("ItemScene");
    }

    /***チャージボタン(動画広告閲覧でで100クレジット追加)***/
    public void OnClickSeeAdMovie()
    {
        // データがある && 更新日時が今日(今日は既にみている)ならば途中終了
        /*
         * if(data_relation.GetComponent<DataRelation>().IsAdMov())
        {
            // ここに動画広告の関数？をいれる
            string _update_date = data_relation.GetComponent<DataRelation>().GetAdMovUpdateDate();

            // 現在時刻
            string _now_date = DateTime.Now.ToString("yyyyMMdd");

            if (_now_date == _update_date)
            {
                return;
            }
        }*/

        // ここから動画広告再生の処理
        Debug.Log("test");

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        int _my_credit = data_relation.GetComponent<DataRelation>().GetCredit();
        _my_credit = _my_credit + 100;
        data_relation.GetComponent<DataRelation>().SetCredit(_my_credit);
        data_relation.GetComponent<DataRelation>().SetUpdateDate();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        this.rewardedAd.Show();
    }
    
}
