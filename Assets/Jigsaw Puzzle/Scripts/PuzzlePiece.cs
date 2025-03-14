using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public Transform Group { get; private set; }

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
        if (Group == null)
        {
            startMovePosition = transform.position;
        }
        else
        {
            startMovePosition = Group.position;
        }

    }

    public void Move(Vector3 moveDelta)
    {
        Vector3 targetPosition = startMovePosition + moveDelta;

        if (Group == null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 60 * .3f);
        }
        else
        {
            Group.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 60 * .3f);
        }
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
        for (int i = 0; i < neighbors.Length; ++i)
        {
            if (neighbors[i] == null)
            {
                continue;
            }

            if (neighbors[i].IsValid)
            {
                continue;
            }

            Vector3 correctLocalPosition = Quaternion.Euler(0, 0, -90 * i) * Vector3.right * transform.localScale.x;
            

            Vector3 correctWorldPosition = transform.position + correctLocalPosition;
            correctPosition.z = neighbors[i].transform.position.z;

            if (Vector3.Distance(correctWorldPosition, neighbors[i].transform.position) < GetMinValidDistance())
            {
                SnapNeighbor(neighbors[i], correctWorldPosition, i);
            }
        }
    }

    private void SnapNeighbor(PuzzlePiece neighbor, Vector3 correctWorldPosition, int neighborIndex)
    {
        if (Group != null && neighbor.Group != null && Group == neighbor.Group)
        {
            return;
        }

        if (Group == null && neighbor.Group == null)
        {
            neighbor.transform.position = correctWorldPosition;

            GameObject pieceGroup = new GameObject("Piece Group " + Random.Range(100, 200));
            pieceGroup.transform.position = transform.position;
            pieceGroup.transform.SetParent(transform.parent);

            transform.SetParent(pieceGroup.transform);
            neighbor.transform.SetParent(pieceGroup.transform);

            Group = pieceGroup.transform;
            neighbor.Group = pieceGroup.transform;
        }

        if (Group != null && neighbor.Group == null)
        {
            neighbor.transform.position = correctWorldPosition;
            neighbor.transform.SetParent(Group);
            neighbor.Group = Group;
        }

        if (Group == null && neighbor.Group != null)
        {
            Group = neighbor.Group;
            transform.SetParent(Group);

            Vector3 thisCorrectWorldPosition = neighbor.transform.position + 
                Quaternion.Euler(0, 0, -90 * (neighborIndex + 2)) * Vector3.right * transform.localScale.x;

            transform.position = thisCorrectWorldPosition;
        }
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
