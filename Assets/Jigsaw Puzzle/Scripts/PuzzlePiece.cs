using System;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour, IComparable<PuzzlePiece>
{
    [Header("Elements")]
    [SerializeField] private Renderer renderer;

    [Header("Movement")]
    private Vector3 startMovePosition;

    [Header("Validation")]
    private Vector3 correctPosition;

    [Header("Neighbors")]
    private PuzzlePiece[] neighbors;

    public bool IsValid { get; private set; }

    public void Configure(float scale, Vector2 tiling, Vector2 offset, Vector3 correctPosition)
    {
        transform.localScale = Vector3.one * scale;

        renderer.material.mainTextureScale = tiling;
        renderer.material.mainTextureOffset = offset;

        this.correctPosition = correctPosition;
    }

    public void SetNeighbors(params PuzzlePiece[] puzzlePieces)
    {
        neighbors = puzzlePieces;
    }

    public void StartMoving()
    {
        startMovePosition = transform.position;

    }

    public void Move(Vector3 moveDelta)
    {
        Vector3 targetPosition = startMovePosition + moveDelta;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 60 * .3f);
        //transform.position = targetPosition;
    }

    public void StopMoving()
    {
        bool isValid = CheckForValidation();

        if (isValid)
        {
            return;
        }

        CheckForNeighbors();
    }

    private bool CheckForValidation()
    {
        if (IsCloseToCorrectPosition())
        {
            Validate();
            return true;
        }

        return false;
    }

    private bool IsCloseToCorrectPosition()
    {
        return Vector3.Distance((Vector2)transform.position, (Vector2)correctPosition) < GetMinValidDistance();
    }

    private float GetMinValidDistance()
    {
        return Mathf.Max(.05f, transform.localScale.x / 5);
    }

    private void CheckForNeighbors()
    {

    }

    private void Validate()
    {
        correctPosition.z = 0;
        transform.position = correctPosition;

        IsValid = true;
        Debug.Log("Piece placed correctly " + name);
    }

    public int CompareTo(PuzzlePiece otherPiece)
    {
        return transform.position.z.CompareTo(otherPiece.transform.position.z);
    }
}
