using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookshelf : MonoBehaviour, ICanInteract
{
    private const string SHOW = "isShow";
    private const string HIDE = "isHide";

    [SerializeField] private BookshelfUI m_bookshelfUI;
    private Collider2D m_collider;
    private Animator m_animator;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        m_bookshelfUI.OnBookshelfClosed += BookshelfUI_OnBookshelfClosed;   
    }

    private void BookshelfUI_OnBookshelfClosed(object sender, EventArgs e)
    {
        m_collider.enabled = true;
    }

    public void Interact()
    {
        m_collider.enabled = false;
        if(m_bookshelfUI.IsAnimationDone())
        {
            m_bookshelfUI.CheckBookshelf();
        }
    }

}
