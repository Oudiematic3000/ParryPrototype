using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;

public class MonsterScript : MonoBehaviour
{
    public enum State
    {
        ready,
        attacking,
        vulnurable
    }

    public State state=State.ready;
    public int health, damage;
    public Vector2 vulnDirection= Vector2.zero;
    public GameObject[] arrows;
    public GameObject attackIndicator;
    public TextMeshProUGUI healthText;

    public static event Action<int> doDamage;

    private void Awake()
    {
        PlayerScript.doParry += getParried;
        PlayerScript.doCounter += getCountered;
    }

    private void OnDestroy()
    {
        PlayerScript.doParry -= getParried;
        PlayerScript.doCounter -= getCountered;
    }

    void Start()
    {
        StartCoroutine(attackTimer());
        attackIndicator.SetActive(false);
        foreach (GameObject ar in arrows)
        {
            ar.transform.localScale= Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void standby()
    {
        if (state != State.vulnurable)
        {
            state = State.ready;
            doDamage(damage);
            StartCoroutine(attackTimer());
        }
        
        
    }
    public void Attack()
    {
        if(state==State.ready)
        StartCoroutine(attackWindow());
    }

    public IEnumerator attackWindow()
    {
        foreach (GameObject ar in arrows)
        {
            ar.transform.localScale = Vector3.zero;
        }
        attackIndicator.SetActive(true);
        state = State.attacking;
        yield return new WaitForSeconds(1f);
        attackIndicator.SetActive(false);
        standby();
    }

    public IEnumerator counterWindow()
    {
        state = State.vulnurable;
        yield return new WaitForSeconds(1f);
        StopAllCoroutines();
        state = State.ready;
        StartCoroutine(attackTimer());

    }
    public IEnumerator attackTimer()
    {
        
        
        if (state == State.ready)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.5f,4f));
            Attack();
            
        }
    }

    public void getParried()
    {
        StopCoroutine(attackTimer());
        if (state==State.attacking)
        {
            attackIndicator.SetActive(false);
            StartCoroutine(counterWindow());
            int randomInt = UnityEngine.Random.Range(0, 4);

            switch (randomInt)
            {
                case 0:
                    vulnDirection = Vector2.up; 
                    arrows[0].transform.localScale = Vector3.one; break;
                case 1:
                    vulnDirection = Vector2.down;
                    arrows[2].transform.localScale = Vector3.one; break;
                case 2:
                    vulnDirection = Vector2.left;
                    arrows[3].transform.localScale = Vector3.one; break;
                case 3:
                    vulnDirection = Vector2.right; 
                    arrows[1].transform.localScale = Vector3.one; break;


            }
        }
    }

    public void getCountered(Vector2 attackDirection, int damage)
    {
        
        
        
        if (state == State.vulnurable)
        {
            if(attackDirection == vulnDirection)
            {
                TakeDamage(damage);
            }
            else
            {
                doDamage(damage);
            }
        }
        foreach (GameObject ar in arrows)
        {
            ar.transform.localScale = Vector3.zero;
        }
        

    }

    public void TakeDamage(int damage)
    {
        health-=damage;
        healthText.text = "Monster health: " + health;

    }
}
