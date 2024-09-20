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
    private bool _canPlaySFX;

    private Coroutine _currentCoroutine;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        Invoke("AllowSFX", 0.5f);
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
    public void PlaySFX(int sfxIndex, Transform soundSource, bool fadeIn = false)
    {
        if (!_canPlaySFX) { return; }

        if (soundSource != null && Vector2.Distance(PlayerManager.Instance.Player.transform.position, soundSource.position) > _sfxMinimumDistance)
            return;

        if (sfxIndex < _sfxArray.Length)
        {
            if (fadeIn)
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);

                _currentCoroutine = StartCoroutine(IE_SFXFadeIn(_sfxArray[sfxIndex]));
            }
            _sfxArray[sfxIndex].pitch = Random.Range(0.9f, 1.1f);
            _sfxArray[sfxIndex].Play();
        }
    }
    public void StopSFX(int sfxIndex, bool fadeOut = false)
    {
        if (sfxIndex < _sfxArray.Length)
        {
            if (fadeOut)
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);

                _currentCoroutine = StartCoroutine(IE_SFXFadeOut(_sfxArray[sfxIndex]));
            }
            else
            {
                _sfxArray[sfxIndex].Stop();
            }
        }
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
    private void AllowSFX()
    {
        _canPlaySFX = true;
    }
    private IEnumerator IE_SFXFadeIn(AudioSource audioSource)
    {
        audioSource.volume = 0;
        while (audioSource.volume < 1f)
        {
            audioSource.volume += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
    private IEnumerator IE_SFXFadeOut(AudioSource audioSource)
    {

        while (audioSource.volume >= 0f)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
        yield break;
    }
}