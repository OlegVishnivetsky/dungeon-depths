using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    [Header("DUNGEON")]
    public RoomNodeTypeListSO roomNodeTypeList;

    [Header("PLAYER")]
    public CurrentPlayerSO currentPlayerSO;

    [Header("MATERIALS")]
    public Material dimmedMaterial;
    public Material litMaterial;

    public Shader variableLitShader;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayerSO), currentPlayerSO);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
    }

#endif

    #endregion Validation
}