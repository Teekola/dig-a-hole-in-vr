using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private bool rotate;
    MeshRenderer rend;
    Color original;
    public static float waitTime;
    public static bool wait;

    private void Start()
    {
        rotate = true;
        rend = GetComponentInChildren<MeshRenderer>();
        original = rend.material.color;
        waitTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 45f, Space.World);
        }
    }

    public void InterruptRotation()
    {
        if (rotate)
        {
            StopAllCoroutines();
            rend.material.color = Color.green;
        }
        else
        {
            if (wait)
            {
                StartCoroutine(waitASecond(waitTime));
            }
            else
            {
                rend.material.color = original;
            }
        }
        rotate = !rotate;
    }

    IEnumerator waitASecond(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rend.material.color = original;
    }
}
