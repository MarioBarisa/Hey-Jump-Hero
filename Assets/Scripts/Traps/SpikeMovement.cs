using System.Collections;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [SerializeField] private float downPosition = -1f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waitDown = 2f;
    [SerializeField] private Collider2D spikeCollider;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
        StartCoroutine(SpikeLoop());
    }

    IEnumerator SpikeLoop()
    {
        while (true)
        {
            yield return MoveTo(_startPos.y + downPosition); // idi dolje
            spikeCollider.enabled = false;
            yield return new WaitForSeconds(waitDown); // cekaj
            spikeCollider.enabled = true;
            yield return MoveTo(_startPos.y); // idi gore
        }
    }

    IEnumerator MoveTo(float targetY)
    {
        while (!Mathf.Approximately(transform.position.y, targetY))
        {
            float y = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(_startPos.x, y, _startPos.z);
            yield return null;
        }
    }
}