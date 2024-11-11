using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private Vector2 roomSpacing;

    [SerializeField]
    private float transitionSpeed = 5f;
    private float transitionTimeElapsed = 0f;
    private Vector3 transitionStartPosition;
    private Vector3 transitionEndPosition;
    private bool transitionComplete = false;

    
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        transitionStartPosition = transform.position;
        transitionEndPosition = transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        transitionTimeElapsed += transitionSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transitionStartPosition, transitionEndPosition, transitionTimeElapsed);
        if(transform.position.Equals(transitionEndPosition) && !transitionComplete)
        {
            transitionComplete = true;
            HeroController.Instance.gameObject.SetActive(true);
            HeroController.Instance.transform.position = (Vector2)transform.position;

        }
    }
    

    public void TransitionRoom(Vector2 direction)
    {
        HeroController.Instance.gameObject.SetActive(false);
        transitionStartPosition = transform.position;
        transitionEndPosition = transform.position + (Vector3)(direction * roomSpacing);
        transitionTimeElapsed = 0f;
        transitionComplete = false;
    }
    
}
