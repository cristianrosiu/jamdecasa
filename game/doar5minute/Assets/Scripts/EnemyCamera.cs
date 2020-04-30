using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : AbstractEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(1, 2, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
