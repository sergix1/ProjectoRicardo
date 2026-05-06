using System.ComponentModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int hp;
    private int maxHp;
    public int getHP => hp;
    private bool healing;
    void Start()
    {
        maxHp = 100;
        hp = maxHp;
        hp = 50;
    }

    float timeToHeal;
    void Update()
    {

        timeToHeal += Time.deltaTime;
        if(timeToHeal>5)
        {
            if (hp < maxHp)
            {
                healing = true;
           
                timeToHeal = 0;
            }

        }
        if(healing)
        {
            Heal();
        }
    }
    
    public void Heal()
    {
        if(hp<maxHp)
        {
            Mathf.Lerp(hp, hp + hp%10,maxHp);
            Debug.Log("Current hp : " + hp + " healing : " +hp%10);
        }
    }
}
