using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenemanager : SingletonMonoBehaviour<Scenemanager>
{
    
    [SerializeField]
    private Fade fade;  // フェード

    private bool isFading = false;  // フェード中かどうか

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 描画遷移
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="fadeinTime"></param>
    /// <param name="waitTime"></param>
    /// <param name="fadeoutTime"></param>
    public void LoadLevel(string scene, float FadeInTime, float FadeOutTime)
    {
        this.isFading = true;

        //  フェードイン
        fade.FadeIn(FadeInTime, () =>
        {
            // シーン切り替え
            if (scene != null)
                SceneManager.LoadScene(scene);

            // フェードアウト
            fade.FadeOut(FadeOutTime, () =>
            {
                this.isFading = false;
           });
        });
    }

    /// <summary>
    /// フェードしているか
    /// </summary>
    /// <returns></returns>
    public bool GetisFading()
    {
        return isFading;
    }
}
