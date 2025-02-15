using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int health, damage;
    public static event Action doParry;
    public static event Action<Vector2, int> doCounter;
    public Vector2 attackDirection;
    public TextMeshProUGUI healthText;
    public GameObject[] spawners;
    public GameObject hitPrefab;

    Controls playerInput;
    private void Awake()
    {
        MonsterScript.doDamage += TakeDamage;

    }

    private void OnDestroy()
    {
        MonsterScript.doDamage -= TakeDamage;
    }

    private void OnEnable()
    {
        playerInput = new Controls(); 

        playerInput.Player.Enable();

        playerInput.Player.Counter.performed += ctx => attackDirection = ctx.ReadValue<Vector2>();
        playerInput.Player.Counter.performed += ctx => Counter();

        playerInput.Player.Parry.performed += ctx => Parry();
            

    }
    void Start()
    {
        healthText.text = "Player Health: " + health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Parry()
    {

        doParry();
    }

    public void Counter()
    {
        doCounter(attackDirection, damage);
        SpawnHit(attackDirection);
    }

    public void TakeDamage(int damage)
    {
        health-=damage;
        healthText.text = "Player Health: " + health;

    }

    public void SpawnHit(Vector2 direction)
    {
        GameObject target;
        Vector3 rotation = Vector3.zero;
        if(direction==Vector2.up)
        {
            target = spawners[0];
            rotation = new Vector3(0, 0, 90); 
        }else if(direction==Vector2.down)
        {
            target = spawners[2];
            rotation = new Vector3(0, 0, 90);
        }
        else if (direction == Vector2.left)
        {
            target = spawners[3];

        }else if(direction==Vector2.right) {
            target = spawners[1];
        }
        else
        {
            target = spawners[0];
        }
        GameObject spawned= Instantiate(hitPrefab, target.transform.position, Quaternion.Euler(rotation));
        spawned.GetComponent<Rigidbody2D>().linearVelocity=(-direction*150);
        StartCoroutine(garbage());
    }

    public IEnumerator garbage()
    {
        yield return new WaitForSeconds(2f);
      GameObject[] stuff=  GameObject.FindGameObjectsWithTag("Finish");
        foreach(GameObject game in stuff)
        {
            Destroy(game);
        }
    }
}
