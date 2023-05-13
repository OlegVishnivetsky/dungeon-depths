using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    [SerializeField] private Transform cursorTargetTransform;

    private CinemachineTargetGroup cinemachineTargetGroup;

    private void Awake()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachineTargetGroup();
    }

    private void Update()
    {
        cursorTargetTransform.position = HelperUtilities.GetMouseWorldPosition();
    }

    private void SetCinemachineTargetGroup()
    {
        CinemachineTargetGroup.Target cinemachinePlayerTarget = new CinemachineTargetGroup.Target
        { weight = 1f, radius = 5f, target = GameManager.Instance.GetPlayer().transform };

        CinemachineTargetGroup.Target cinemachineCursorTarget = new CinemachineTargetGroup.Target
        { weight = 1f, radius = 1f, target = cursorTargetTransform };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] 
        { cinemachinePlayerTarget, cinemachineCursorTarget };

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
    }
}