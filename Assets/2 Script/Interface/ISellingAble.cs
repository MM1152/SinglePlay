using UnityEngine;

public interface ISellingAble {
    public Sprite image{get; set;}
    public ClassStruct classStruct{get; set;}
    public string saveDataType { get; set;}
    public int saveDatanum { get; set; }
}