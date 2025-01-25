using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public float microMoveSpeed;
    public float microJumpForce;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius;
    private bool isGround;

    private Rigidbody2D rb;

    private AudioSource audioSource;
    private AudioClip audioClip;
    public float realVolume;
    public float maxVolume;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioClip = Microphone.Start(null, true, 999, 44100);
    }


    void Update()
    {
        maxVolume = 0f;
        float[] volumeData = new float[128];
        int offset = Microphone.GetPosition(null) - 128 + 1;

        if (offset < 0)
        {
            offset = 0;
        }
        audioClip.GetData(volumeData, offset);
        for (int i = 0; i < 128; i++)
        {
            float tempMax = volumeData[i];//修改音量的敏感值
            if (maxVolume < tempMax)
            {
                maxVolume = tempMax;
            }
        }

        realVolume = (int)(maxVolume * 100);

        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (realVolume > 0 && realVolume < 10)
        {
            rb.velocity = new Vector2(realVolume / 2, rb.velocity.y);
        }

        if (realVolume > 10)
        {
            rb.velocity = new Vector2(microMoveSpeed, realVolume / 6);
        }

        //这里是正常的2D运动逻辑
        //float dirX = Input.GetAxis("Horizontal");     
        //rb.velocity = new Vector2 (dirX * moveSpeed, rb.velocity.y);

        //if(Input.GetButtonDown("Jump") && isGround)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    //Microphone.End(null);
        //    audioSource.Play();
        //}
    }

}