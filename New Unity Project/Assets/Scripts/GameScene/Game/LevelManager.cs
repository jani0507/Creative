using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { set; get; }

    //Level Spawning

    //List of pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longblocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    public List<Piece> pieces = new List<Piece>();

    public Piece GetPiece(PieceTyp pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualsIndex == visualIndex && !x.gameObject.activeSelf);

        if(p == null)
        {
            GameObject go = null;
            if (pt == PieceTyp.ramp)
                go = ramps[visualIndex].gameObject;
            else if (pt == PieceTyp.longblock)
                go = longblocks[visualIndex].gameObject;
            else if (pt == PieceTyp.jump)
                go = jumps[visualIndex].gameObject;
            else if (pt == PieceTyp.slide)
                go = slides[visualIndex].gameObject;

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }

        return p;
    }

    
}
