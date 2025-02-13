using UnityEngine;
using System;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int health, damage;
    public static event Action doParry;
    public static event Action<Vector2, int> doCounter;
    public Vector2 attackDirection;
    public TextMeshProUGUI healthText;

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
    }

    public void TakeDamage(int damage)
    {
        health-=damage;
        healthText.text = "Player Health: " + health;
    }
}
