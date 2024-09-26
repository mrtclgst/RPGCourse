using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After Image FX")]
    [SerializeField] private GameObject _afterImagePrefab;
    [SerializeField] private float _colorLooseRate;
    [SerializeField] private float _afterImageCooldown;
    private float _afterImageCooldownTimer;

    [Header("Screen Shake")]
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private float _shakeMultiplier;
    [SerializeField] internal Vector3 _shakeCatchSword;
    [SerializeField] internal Vector3 _shakeHighDamage;

    [Space]
    [SerializeField] private ParticleSystem _dustFX;

    protected override void Start()
    {
        base.Start();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void Update()
    {
        _afterImageCooldownTimer -= Time.deltaTime;
    }
    public void PlayDustFX()
    {
        if (_dustFX != null)
            _dustFX.Play();
    }
    public void CreateAfterImage()
    {
        if (_afterImageCooldownTimer < 0)
        {
            _afterImageCooldownTimer = _afterImageCooldown;
            GameObject newAfterImage = Instantiate(_afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(_colorLooseRate, _spriteRenderer.sprite);
        }
    }
    public void ScreenShake(Vector3 shakePower)
    {
        _impulseSource.m_DefaultVelocity = new Vector3(shakePower.x * _player.GetFacingDirection(), shakePower.y) * _shakeMultiplier;
        _impulseSource.GenerateImpulse();
    }
}