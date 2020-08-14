using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceTyp
{
    none = -1,
    ramp = 0,
    longblock = 1,
    jump =2,
    slide =3,
}
public class Piece : MonoBehaviour
{
    public PieceTyp type;
    public int visualsIndex;

}
