using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int _areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.Instance.PlaySFX(_areaSoundIndex, null, true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.Instance.StopSFX(_areaSoundIndex, true);
        }
    }
}