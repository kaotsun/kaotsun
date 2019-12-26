using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaotsunButton : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private bool isPlayAudio = true;

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
        if (isPlayAudio)
        {
            LockPlyaAudio();

            PlayAudio();

            Invoke("UnlockPlyaAudio", 3.5f);
        }
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
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}