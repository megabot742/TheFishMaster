using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Hook : MonoBehaviour
{
    [SerializeField] Transform hookedTransfrom;

    Camera mainCamera;
    Collider2D coll2D;

    int lenght;
    int strength;
    int fishCount;

    bool canMove;
    List<Fish> hookedFishes;
    Tweener cameraTween;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        mainCamera = Camera.main;
        coll2D = GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }
    public void StartFishing()
    {
        lenght = IdleManager.instance.length - 20;
        strength = IdleManager.instance.strength;
        fishCount = 0;
        float time = (-lenght) * 0.1f;

        cameraTween = mainCamera.transform.DOMoveY(lenght, 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if(mainCamera.transform.position.y <= -11)
            {
                transform.SetParent(mainCamera.transform);
            }
        }).OnComplete(delegate
        {
            coll2D.enabled =  true;
            cameraTween = mainCamera.transform.DOMoveY(0, time*5, false).OnUpdate(delegate
            {
                if(mainCamera.transform.position.y >= -25f)
                {
                    StopFishing();
                }
            });
        });

        ScreensManager.instance.ChangeScreen(Screens.GAME);
        coll2D.enabled = false;
        canMove = true;
        hookedFishes.Clear();
    }
    void StopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if(mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll2D.enabled = true;
            int num = 0;
            for(int i = 0; i < hookedFishes.Count; i++)
            {
                hookedFishes[i].transform.SetParent(null);
                hookedFishes[i].ResetFish();
                num += hookedFishes[i].Type.price;
            }
            IdleManager.instance.totalGain = num;
            ScreensManager.instance.ChangeScreen(Screens.END);
        });
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Fish") && fishCount != strength)
        {
            fishCount++;
            Fish fishComponent = target.GetComponent<Fish>();
            fishComponent.Hooked();
            hookedFishes.Add(fishComponent);
            target.transform.SetParent(transform);
            target.transform.position = hookedTransfrom.position;
            target.transform.rotation = hookedTransfrom.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakePosition(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;
            });
            if(fishCount == strength)
                StopFishing();
        }
    }
}
