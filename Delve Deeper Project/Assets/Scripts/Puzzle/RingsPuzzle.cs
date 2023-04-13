using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RingsPuzzle : MonoBehaviour
{
    [SerializeField] Transform outerPillars;
    [SerializeField] Transform middlePillars;
    [SerializeField] Transform innerPillars;
    [SerializeField] Transform centralPillar;

    [SerializeField] float correctRotOuter = 120f;
    [SerializeField] float correctRotMiddle = 240f;
    [SerializeField] float correctRotInner = 75f;
    [SerializeField] float correctThreshold = 5f;

    [SerializeField] float pushSpeed = 10f;

    public static bool RingsPuzzleCompleted = false;

    public static UnityAction OnRingsPuzzleCompleted;

    RingPuzzleTrigger[] triggers;

    CharacterController playerController;
    ThirdPersonController player;

    bool OuterAligned = false;
    bool MiddleAligned = false;
    bool InnerAligned = false;

    Vector3 outerStartVictory;
    Vector3 middleStartVictory;
    Vector3 innerStartVictory;
    Vector3 outerEndVictory = new Vector3(0f, 0f, 0f);
    Vector3 middleEndVictory = new Vector3(0f, 0f, 0f);
    Vector3 innerEndVictory = new Vector3(0f, 0f, 0f);

    bool DisplayedVictory = false;
    bool VictoryLerpComplete = false;

    float victoryStartTime;
    float victoryDuration = 2f;

    float centralPillarStartHeight = -2.5f;
    float centralPillarVictoryHeight = 2.5f;

    List<GameObject> innerPillarFires = new();
    List<GameObject> middlePillarFires = new();
    List<GameObject> outerPillarFires = new();

    List<ParticleSystem> innerPillarParticles = new();
    List<ParticleSystem> middlePillarParticles = new();
    List<ParticleSystem> outerPillarParticles = new();

    private void Awake()
    {
        triggers = GetComponentsInChildren<RingPuzzleTrigger>();
        playerController = FindObjectOfType<CharacterController>();
        player = FindObjectOfType<ThirdPersonController>();

        outerPillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);
        middlePillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);
        innerPillars.eulerAngles = new Vector3(0f, Random.Range(-180f, 180f), 0f);

        foreach (Transform pillar in innerPillars)
        {
            innerPillarFires.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_fire);
            innerPillarParticles.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_particles);
        }

        foreach (Transform pillar in middlePillars)
        {
            middlePillarFires.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_fire);
            middlePillarParticles.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_particles);
        }

        foreach (Transform pillar in outerPillars)
        {
            outerPillarFires.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_fire);
            outerPillarParticles.Add(pillar.gameObject.GetComponentInChildren<BrazierInteractable>().m_particles);
        }

        centralPillar.position = new Vector3(centralPillar.position.x, centralPillarStartHeight, centralPillar.position.z);
    }

    private void Update()
    {
        if (!RingsPuzzleCompleted)
        {
            RingsPuzzleCompleted = CheckPuzzleCompleted();
        }

        if (RingsPuzzleCompleted && !DisplayedVictory)
        {
            DisplayVictory();
            DisplayedVictory = true;
        }

        if (DisplayedVictory && !VictoryLerpComplete)
        {
            player.HandlePushing(false);
            PerformVictoryLerp();
        }
    }

    bool CheckPuzzleCompleted()
    {
        OuterAligned = CheckAlignment(outerPillars, outerPillarFires, outerPillarParticles, correctRotOuter);
        MiddleAligned = CheckAlignment(middlePillars, middlePillarFires, middlePillarParticles, correctRotMiddle);
        InnerAligned = CheckAlignment(innerPillars, innerPillarFires, innerPillarParticles, correctRotInner);

        if (OuterAligned && MiddleAligned && InnerAligned)
        {
            Debug.Log("All Aligned!");

            foreach (RingPuzzleTrigger trigger in triggers)
            {
                trigger.transform.gameObject.SetActive(false);
            }

            player.transform.GetComponent<PlayerInteract>().m_Interactable = null;

            return true;
        }

        return false;
    }

    bool CheckAlignment(Transform pillarGroup, List<GameObject> pillarFire, List<ParticleSystem> pillarPS, float correctRot)
    {
        if (Mathf.Abs(pillarGroup.localEulerAngles.y) < correctThreshold)
        {
            foreach (GameObject fire in pillarFire)
            {
                fire.SetActive(true);
            }

            foreach (ParticleSystem ps in pillarPS)
            {
                ps.Play();
            }

            return true;
        }
        else
        {
            foreach (GameObject fire in pillarFire)
            {
                fire.SetActive(false);
            }

            return false;
        }
    }

    void DisplayVictory()
    {
        victoryStartTime = Time.time;
        outerStartVictory = outerPillars.localEulerAngles;
        middleStartVictory = middlePillars.localEulerAngles;
        innerStartVictory = innerPillars.localEulerAngles;
    }

    void PerformVictoryLerp()
    {
        float lerpVal = (Time.time - victoryStartTime) / victoryDuration;

        if (lerpVal >= 1f)
        {
            lerpVal = 1f;
            VictoryLerpComplete = true;

            OnRingsPuzzleCompleted?.Invoke();
        }

        outerPillars.localEulerAngles = Vector3.Lerp(outerStartVictory, outerEndVictory, lerpVal);
        middlePillars.localEulerAngles = Vector3.Lerp(middleStartVictory, middleEndVictory, lerpVal);
        innerPillars.localEulerAngles = Vector3.Lerp(innerStartVictory, innerEndVictory, lerpVal);

        float centralHeight = Mathf.Lerp(centralPillarStartHeight, centralPillarVictoryHeight, lerpVal);
        centralPillar.position = new Vector3(centralPillar.position.x, centralHeight, centralPillar.position.z);
    }

    public void RotatePillar(RingPuzzleTrigger trigger)
    {
        float rot = (trigger.direction == RingPuzzleTriggerDirection.Clockwise ? pushSpeed : -pushSpeed) * Time.deltaTime;
        trigger.transform.parent.parent.Rotate(Vector3.up * rot);

        playerController.enabled = false;
        float origY = playerController.transform.position.y;
        playerController.transform.position = new Vector3(trigger.transform.position.x, origY, trigger.transform.position.z);
        playerController.transform.forward = trigger.transform.forward;
        playerController.enabled = true;
    }
}
