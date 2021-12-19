using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager instance;

    public PlayerController activePlayer;
    public CharacterSelector activeCharacterSelector;

    // Cheat weapons for presenting purpose
    public GameObject[] cheatWeapons;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TEMP cheat weapons enable
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(GameObject weapon in cheatWeapons)
            {
                weapon.SetActive(true);
            }
        }
    }
}
