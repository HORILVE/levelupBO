using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Collider))]

public class SleutelCollectible : MonoBehaviour
{
    [HideInInspector]
    public SlotScript slotscript;
    public Collider col;
    private float smoothTime = 0.8f;
    private float distanceThreshold = 1;
    private Vector3 velocity = Vector3.zero;
    private float offset = -1;



    [HideInInspector]
    public bool unlocked = false;
    private bool connected = false;


    [Header("Gizmo Settings")]
    [SerializeField, HideInInspector] public Color gizmoColor = Color.yellow;
    [SerializeField, HideInInspector] public float gizmoSize = 0.5f;
    [Header("State (read-only in Inspector)")]
    [SerializeField, HideInInspector] public bool IsCollected = false;
    [SerializeField, HideInInspector] private SlotScript currentSlot = null;
    [Header("Icon Settings")]
    [Range(0f, 1f), HideInInspector] public float iconAlpha = 0.4f;
    [Range(0f, 1f), HideInInspector] public float iconSize = 40f;

    private float xas;
    private float yas;
    private float zas;
    [SerializeField] private float speed = 40f;
    [SerializeField] private bool xaxis = false;
    [SerializeField] private bool yaxis = false;
    [SerializeField] private bool zaxis = false;
    public float amplitude = 0.2f;
    public float frequency = 0.7f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();


    private void Awake()
    {
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    void Start()
    {
        posOffset = transform.position;
    }


    void FixedUpdate()
    {
        if (!connected)
        {
            if (xaxis)
                xas = 1;
            else
                xas = 0;

            if (yaxis)
                yas = 1;
            else
                yas = 0;

            if (zaxis)
                zas = 1;
            else
                zas = 0;

            transform.Rotate(new Vector3(xas, yas, zas) * speed * Time.deltaTime);
            floating();
        }
    }

    private void floating()
    {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }

    private IEnumerator followplayer(Transform player)
    {
        slotscript.Collected(this);
        connected = true;
        while (unlocked == false)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if(distance > distanceThreshold)
            {
                Vector3 direction = player.TransformDirection(Vector3.forward) * offset;
                Vector3 playerposition = new Vector3(player.position.x, player.position.y + 1, player.position.z) + direction;  

                transform.position = Vector3.SmoothDamp(transform.position, playerposition, ref velocity, smoothTime);
            }
            yield return null;
        }

        Vector3 currentsize = transform.localScale;
        float timeElapsed = 0;
        float lerpDuration = 0.5f;
        while (timeElapsed < lerpDuration)
        {
            transform.localScale = Vector3.Lerp(currentsize, Vector3.zero, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Jammo")
        {
            if(slotscript != null)
            {
                StartCoroutine(followplayer(other.transform));
            }
            else
            {
                Debug.LogError("dit sleutelobject is niet gekoppeld aan een slotscript");
            }
        }
    }
}


