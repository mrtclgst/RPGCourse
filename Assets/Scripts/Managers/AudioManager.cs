using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float _sfxMinimumDistance;
    [SerializeField] private AudioSource[] _sfxArray;
    [SerializeField] private AudioSource[] _bgmArray;

    public bool ActivateBGM;
    private int _currentBGMIndex;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
    private void Update()
    {
        if (!ActivateBGM)
        {
            StopAllBGM();
        }
        else
        {
            if (!_bgmArray[_currentBGMIndex].isPlaying)
            {
                PlayBGM(_currentBGMIndex);
            }
        }
    }

    public void PlaySFX(int sfxIndex, Transform soundSource)
    {
        if (soundSource != null && Vector2.Distance(PlayerManager.Instance.Player.transform.position, soundSource.position) > _sfxMinimumDistance)
            return;

        if (_sfxArray[sfxIndex].isPlaying)
            return;

        if (sfxIndex < _sfxArray.Length)
        {
            _sfxArray[sfxIndex].pitch = Random.Range(0.9f, 1.1f);
            _sfxArray[sfxIndex].Play();
        }
    }
    public void StopSFX(int sfxIndex)
    {
        if (sfxIndex < _sfxArray.Length)
            _sfxArray[sfxIndex].Stop();
    }
    public void PlayBGM(int bgmIndex)
    {
        StopAllBGM();
        if (bgmIndex < _bgmArray.Length)
        {
            _currentBGMIndex = bgmIndex;
            _bgmArray[_currentBGMIndex].Play();
        }
    }
    public void PlayRandomBGM()
    {
        _currentBGMIndex = Random.Range(0, _bgmArray.Length);
        PlayBGM(_currentBGMIndex);
    }
    public void StopAllBGM()
    {
        for (int i = 0; i < _bgmArray.Length; i++)
        {
            _bgmArray[i].Stop();
        }
    }
}
