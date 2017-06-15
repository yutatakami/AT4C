using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : SingletonMonoBehaviour<Sound_Manager>
{
    GameObject  SoundPlayer;

    AudioSource Source_SE;
    AudioSource Source_BGM;

    Dictionary<string, AudioClipInfo> Clips = new Dictionary<string, AudioClipInfo>();

    float Now_PlayTime;
    string Before_BGM_Name;

    class AudioClipInfo
    {
        public string ResourceName;
        public string Name;
        public AudioClip Clip;

        public AudioClipInfo( string ResourceName, string Name )
        {
            // カレント変更( "Resourcesフォルダ直下" → "Resources/Soundsフォルダ直下へ" )
            this.ResourceName = "Sounds/" + ResourceName;
            this.Name = Name;
        }

    }

    // 初期化関数
    public void Awake()
    {
        Now_PlayTime    = 0.0f;
        Before_BGM_Name = "";

        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // コンストラクタ
    public Sound_Manager()
    {
        // サウンド登録("Resoueces/Sounds フォルダ直下に置く")
        Clips.Add( "SE001",  new AudioClipInfo( "tama",   "SE001" ) );
        Clips.Add( "SE002",  new AudioClipInfo( "cancel", "SE002" ) );
        Clips.Add( "BGM001", new AudioClipInfo( "bgm001", "BGM001" ) );
        Clips.Add( "BGM002", new AudioClipInfo( "bgm002", "BGM002" ) );
    }

    // SE再生
    public void PlaySE( string Name )
    {
        // 指定した名前が登録されていなかったら終了
        if ( Clips.ContainsKey( Name ) == false )
            return;

        //
        AudioClipInfo Info = Clips[ Name ];

        // Load
        if ( Info.Clip == null )
            Info.Clip = (AudioClip)Resources.Load( Info.ResourceName );

        if ( SoundPlayer == null )
            SoundPlayer = new GameObject( "SoundPlayer" );

        Source_SE = SoundPlayer.AddComponent<AudioSource>();

        // 再生開始(音の重複あり)
        Source_SE.PlayOneShot( Info.Clip );
    }

    // BGM再生
    public void PlayBGM( string Name, bool Loop )
    {
        // 指定した名前が登録されていなかったら終了
        if (Clips.ContainsKey(Name) == false)
            return;

        // 前回とは違う名前のBGMを再生する命令が来たら再生位置を初期位置にリセット
        if (Before_BGM_Name != Name)
        {
            if (Source_BGM != null)
            {
                Source_BGM.Stop();
                Source_BGM.volume = 1.0f;
            }


            Now_PlayTime = 0.0f;
        }
        else
        {
            // 前回と今回、同じBGMを再生する命令がきた+前回のBGMがまだ再生中だったら前回のBGM再生を中止
            if( Source_BGM.isPlaying )
                Source_BGM.Stop();
        }

        // 
        Before_BGM_Name = Name;

        //
        AudioClipInfo Info = Clips[Name];

        // Load
        if (Info.Clip == null)
            Info.Clip = (AudioClip)Resources.Load(Info.ResourceName);

        if (SoundPlayer == null)
            SoundPlayer = new GameObject("SoundPlayer");
        

        Source_BGM = SoundPlayer.AddComponent<AudioSource>();
        Source_BGM.clip = Info.Clip;

        // 再生位置を一時停止した場所からスタート
        Source_BGM.time = Now_PlayTime;

        Source_BGM.loop = Loop;

        // 再生開始(音の重複なし)
        Source_BGM.Play();
    }

    // BGM一時停止
    public void PauseBGM()
    {
        if ( Source_BGM == null )
            return;

        Now_PlayTime = Source_BGM.time;
        Source_BGM.Stop();
    }

    // BGM停止
    public void StopBGM()
    {
        if ( Source_BGM == null )
            return;

        Source_BGM.Stop();
        Now_PlayTime = 0.0f;
    }

    // BGMのボリューム変更
    public void BGM_VolumeChange( float Vol )
    {
        Source_BGM.volume += Vol;

        if ( Source_BGM.volume >= 1.0f )
            Source_BGM.volume = 1.0f; 

        if ( Source_BGM.volume <= 0.0f )
            Source_BGM.volume = 0.0f;
    }

    // BGMにループ設定をつけるかどうか
    public void BGM_Loop( bool Loop )
    {
        Source_BGM.loop = Loop;
    }
}