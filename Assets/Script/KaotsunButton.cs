using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaotsunButton : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private bool isPlayAudio = true;
    public GameObject Kaotsun;

    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // かおつんボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        Debug.Log("test");
        if (isPlayAudio)
        {
            LockPlyaAudio();

            PlayAudio();
            Kaotsun.GetComponent<Animator>().SetBool("OnClick", true);

            Invoke("EndEffect", 3.5f);

        }
    }

    // エフェクトを終了
    private void EndEffect (){
        Kaotsun.GetComponent<Animator>().SetBool("OnClick", false);
        UnlockPlyaAudio();
    }

    // ボタン押下フラグを解除
    private void UnlockPlyaAudio(){
        isPlayAudio = true;
    }

    // ボタン押下フラグをロック
    private void LockPlyaAudio()
    {
        isPlayAudio = false;
    }

    // プレイAudio
    private void PlayAudio(){
        audioSource = Kaotsun.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}