using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DungeonDoor : MonoBehaviour
{
    [SerializeField]
    private Sprite closedSprite;
    [SerializeField]
    private Sprite openSprite;

    private SpriteRenderer spriteRenderer;
    private bool locked = true;
    private bool open = false;
    [SerializeField]
    private Vector2 direction;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    }

    public void SetLocked(bool value)
    {
        locked = value;
    }

    public void SetOpen(bool value)
    {
        open = value;
        if(open) spriteRenderer.sprite = openSprite;
        else spriteRenderer.sprite = closedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && open) GameManager.Instance.TransitionRoom(direction);
    }


}
