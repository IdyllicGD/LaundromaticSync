using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    public static MusicMan instance;

    [Title("Music", null, TitleAlignments.Centered)]
    [SerializeField] StudioEventEmitter startOfGameMusic;
    [SerializeField] StudioEventEmitter MenuMusic;
    [SerializeField] StudioEventEmitter ShopMusic;
    [SerializeField] StudioEventEmitter CrazyMusic;


    public void Start()
    {
        instance = this;
        StopAllMusic();
        PlayThisSong(PlayableMusic.StartOfGameMusic);
    }

    public void PlayThisSong(PlayableMusic musicToPlay)
    {
        switch (musicToPlay)
        {
            case PlayableMusic.StartOfGameMusic:
                startOfGameMusic.Play();
                break;
            case PlayableMusic.MenuMusic:
                MenuMusic.Play();
                break;
            case PlayableMusic.ShopMusic:
                ShopMusic.Play();
                break;
            case PlayableMusic.CrazyMusic:
                CrazyMusic.Play();
                break;
        }
    }

    public void StopAllMusic()
    {
        startOfGameMusic.Stop();
        MenuMusic.Stop();
        ShopMusic.Stop();
        CrazyMusic.Stop();
    }
}
public enum PlayableMusic
{
    StartOfGameMusic, MenuMusic, ShopMusic, CrazyMusic
}
