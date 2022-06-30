using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLefter : MonoBehaviour
{
    Rigidbody2D rb;
    bool awaiter = false;
    float scailar = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = (awaiter ? Vector2.left : Vector2.right) * scailar;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //StartCoroutine(WaitABit());
    }

    private IEnumerator WaitABit()
    {
        scailar = 0f;
        awaiter = true;
        yield return new WaitForSeconds(1.5f);
        scailar = 1f;
    }
}
