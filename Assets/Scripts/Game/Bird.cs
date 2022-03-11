using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    Rigidbody2D rigidBody2D;
    [SerializeField] float JUMP_AMOUNT = 10f;
    static Bird instance;
    State state;
    enum State
    {
        WaittingToPlay,
        Playing,
        Dead
    }
    public static Bird GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        instance = this;
        rigidBody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaittingToPlay;
    }


    void Update()
    {
        switch (state)
        {
            case State.WaittingToPlay:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                    state = State.Playing;
                    Jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                }

                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }

                transform.eulerAngles = new Vector3(0, 0, rigidBody2D.velocity.y * .2f);

                break;

            case State.Dead:
                break;

        }


    }

    void Jump()
    {
        rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        rigidBody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.Jump);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        state = State.Dead;
        SoundManager.PlaySound(SoundManager.Sound.Dead);
        rigidBody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

}
