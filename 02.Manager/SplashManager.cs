using DREADBORN;
using Sound;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DREADBORN.SceneName;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;

    public void OnLoadTitleImage()
    {
        SoundManager.instance.SFXPlay("sfx", sfx);
    }

    public void OnSplashEnded()
    {
        FadeManager.Instance.FadeOut(() =>
        {
            SceneManager.LoadScene(TitleScene);
        });
    }
}
