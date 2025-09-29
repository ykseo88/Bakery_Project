using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    
    [SerializeField] private List<Transform> tutorialsArrows;
    public List<Transform> TutorialArrows => tutorialsArrows;
    public PlayerController player;
    public IState currentState;
    public Transform SeeTablePoint;
    public Transform SeeAnotherConsumePoint;
    public InputManager inputManager;

    [SerializeField] private GameObject Arrow;
    public Compass compass;
    private Transform currentArrowTransform;
    private int currentIndex = 0;
    public Transform CurrentArrowTransform => currentArrowTransform;

    private void Awake()
    {
        instance = this;
    }

    public void SetNextTutorial()
    {
        currentIndex++;
        currentArrowTransform = tutorialsArrows[currentIndex];
        Arrow.transform.position = currentArrowTransform.position;
    }
    
    public void MoveCameraToTransform(Transform point)
    {
        
    }

    private void Start()
    {
        currentArrowTransform = tutorialsArrows[currentIndex];
        ChangeState(new TGetBreadState());
    }

    public void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    private void Update()
    {
        currentState.Update();
    }
}
