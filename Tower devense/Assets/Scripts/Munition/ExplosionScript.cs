using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    public void ended()
    {
        Destroy(gameObject);
    }
}
