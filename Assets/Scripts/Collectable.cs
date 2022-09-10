using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion, manaPotion, money,
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    bool isCollected = false;
    public int value = 0;
    public AudioClip collectSound;

    private void Show()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        isCollected = false;
    }

    private void Hide()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }

    private void Collect()
    {
        isCollected = true;
        Hide();
        AudioSource audio = GetComponent<AudioSource>();
        if(audio != null && this.collectSound != null)
        {
            audio.PlayOneShot(this.collectSound);
        }
        switch(this.type)
        {
            case CollectableType.money:
            GameManager.sharedInstance.CollectObject(value);
            break;

            case CollectableType.healthPotion:
            PlayerController.sharedInstance.CollectHealth(value);
            break;

            case CollectableType.manaPotion:
            PlayerController.sharedInstance.CollectMana(value);
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            Collect();
        }
    }
}

