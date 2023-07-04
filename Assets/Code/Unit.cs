using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    int health;
    Coroutine heathUp;
    const float INTERVAL=0.5f;
    public void ReceiveHealing()
    {
        if (heathUp != null)
            StopCoroutine(heathUp);
        heathUp = StartCoroutine(HealthUpCoroutine());
    }

    IEnumerator HealthUpCoroutine()
    {
        float timer = 3f;
        while(timer>0 && health < 100)
        {
            health += 5;
            timer -= INTERVAL;
            yield return new WaitForSeconds(INTERVAL);
        }
        if (health > 100) health = 100;
    }
}
