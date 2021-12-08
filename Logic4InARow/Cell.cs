using System;

namespace Logic4InARow
{
    public class Cell
    {
        public const char k_Blank = ' ';
        private readonly char[] r_SingleCell;

        public Cell()
        {
            r_SingleCell = new char[3] { k_Blank, k_Blank, k_Blank };
        }

        public char SingleCell
        {
            get { return r_SingleCell[1]; }
            set { r_SingleCell[1] = value; }
        }

        public bool CellIsEmpty()
        {
            return r_SingleCell[1] == k_Blank;
        }
    }
}