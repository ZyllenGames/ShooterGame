using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControll : MonoBehaviour
{
    public GameObject PlayMenu;
    public GameObject OptionMenu;

    public Toggle[] ScreenAspToggleGroup;
    public Scrollbar[] VolumeSliders;

    Vector2[] m_Resolutions;

    private void Awake()
    {
        m_Resolutions = new Vector2[2];
        m_Resolutions[0] = new Vector2(1600, 900);
        m_Resolutions[1] = new Vector2(1366, 768);

        Screen.SetResolution((int)m_Resolutions[0].x, (int)m_Resolutions[0].y,false);

    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Game");
        AudioManager.Instance.PlayMusic("Game");
    }

    public void OnOptionMenu()
    {
        PlayMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void BackToMain()
    {
        PlayMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnScreenAspToggleChanged(bool b)
    {
        for(int i = 0; i < ScreenAspToggleGroup.Length; i++)
        {
            if(ScreenAspToggleGroup[i].isOn)
            {
                if(i == ScreenAspToggleGroup.Length - 1)
                {
                    Screen.SetResolution((int)m_Resolutions[0].x, (int)m_Resolutions[0].y, true);
                    break;
                }
                else
                {
                    Screen.SetResolution((int)m_Resolutions[i].x, (int)m_Resolutions[i].y, false);
                    break;
                }
            }
        }
    }

    public void OnMasterVolumeChanged()
    {
        float value = VolumeSliders[0].value;
        AudioManager.Instance.SetVolume(VolumeSliders[0].value, AudioManager.VolumeType.EnMaster);
    }
    public void OnMusicVolumeChanged()
    {
        AudioManager.Instance.SetVolume(VolumeSliders[1].value, AudioManager.VolumeType.Music);
    }
    public void OnSFXVolumeChanged()
    {
        AudioManager.Instance.SetVolume(VolumeSliders[2].value, AudioManager.VolumeType.SFX);
    }
}
