using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer _sr;
    private float _colorLooseRate;

    public void SetupAfterImage(float loosingSpeed, Sprite spriteImage)
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = spriteImage;
        _colorLooseRate = loosingSpeed;
    }

    private void Update()
    {
        float alpha = _sr.color.a - _colorLooseRate * Time.deltaTime;
        Color newColor = _sr.color;
        newColor.a = alpha;
        _sr.color = newColor;


        if (alpha < 0)
            Destroy(gameObject);
    }
}
