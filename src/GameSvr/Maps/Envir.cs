using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameSvr
{
    /// <summary>
    /// 可见的地图物品
    /// </summary>
    public class TVisibleMapItem
    {
        public int nX;
        public int nY;
        public TMapItem MapItem;
        public string sName;
        public ushort wLooks;
        public int nVisibleFlag;
    } 
    
    public class TOSObject
    {
        public byte btType;
        public object CellObj;
        public int dwAddTime;
        public bool boObjectDisPose;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TMapHeader
    {
        public short wWidth;
        public short wHeight;
    }

    public struct TMapUnitInfo
    {
        public short wBkImg;
        public short wMidImg;
        public short wFrImg;
        public byte btDoorIndex;
        public byte btDoorOffset;
        public byte btAniFrame;
        public byte btAniTick;
        public byte btArea;
        public byte btLight;
    }

    public enum CellAttribute : byte
    {
        Walk = 0,
        HighWall = 1,
        LowWall = 2,
    }

    public class TMapCellinfo
    {
        public static TMapCellinfo LowWall => new TMapCellinfo { Attribute = CellAttribute.LowWall };
        public static TMapCellinfo HighWall => new TMapCellinfo { Attribute = CellAttribute.HighWall };

        public bool Valid => Attribute == CellAttribute.Walk;

        public CellAttribute Attribute;
        public IList<TOSObject> ObjList;

        public TMapCellinfo()
        {
            ObjList = new List<TOSObject>();
        }
    }

    public class PointInfo
    {
        public short nX;
        public short nY;

        public PointInfo(short x,short y)
        {
            nX = x;
            nY = y;
        }
    }

    public class TRect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public TRect(int left,int top,int right,int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}