using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private const string _activatingAnimatorBoolean = "Active";

    private Animator _animator;

    public string CheckpointID;
    public bool Activation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    [ContextMenu("Generate Checkpoint ID")]
    private void GenerateID()
    {
        if (CheckpointID.Length == 0)
        {
            CheckpointID = System.Guid.NewGuid().ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (!Activation)
        {
            AudioManager.Instance.PlaySFX(30, transform);
            Activation = true;
        }
        _animator.SetBool(_activatingAnimatorBoolean, true);
    }
}
