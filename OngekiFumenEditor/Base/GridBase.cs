using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Base
{
    public abstract class GridBase : PropertyChangedBase, IComparable<GridBase>, ISerializable
    {
        private int grid = 0; //grid
        private float unit = 0; //unit

        private uint gridRadix = 2857;
        public uint GridRadix
        {
            get => gridRadix;
            protected set
            {
                gridRadix = value;
                RecalculateTotalValues();
            }
        }

        public int TotalGrid { get; private set; }
        public double TotalUnit { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RecalculateTotalValues()
        {
            TotalGrid = (int)(Unit * GridRadix + Grid);
            TotalUnit = Unit + Grid * 1.0 / GridRadix;
        }

        public GridBase(float unit = default, int grid = default)
        {
            Grid = grid;
            Unit = unit;
        }

        public int Grid
        {
            get
            {
                return grid;
            }
            set
            {
                grid = value;
                RecalculateTotalValues();
                NotifyOfPropertyChange(() => Grid);
            }
        }

        public float Unit
        {
            get
            {
                return unit;
            }
            set
            {
                unit = value;
                RecalculateTotalValues();
                NotifyOfPropertyChange(() => Unit);
            }
        }

        public void NormalizeSelf()
        {
            var addUnit = Grid / GridRadix;
            Unit += addUnit;
            Grid = (int)(Grid % GridRadix);

            if (Grid < 0)
            {
                Grid += (int)GridRadix;
                Unit--;
            }
        }

        public int Compare(GridBase x, GridBase y)
        {
            return x.CompareTo(y);
        }

        public int CompareTo(GridBase other)
        {
            return TotalGrid.CompareTo(other.TotalGrid);
        }

        public static bool operator ==(GridBase l, GridBase r)
        {
            if (l is null)
                return r is null;
            if (r is null)
                return false;
            return l.CompareTo(r) == 0;
        }

        public static bool operator !=(GridBase l, GridBase r)
        {
            return !(l == r);
        }

        public static GridOffset operator -(GridBase l, GridBase r)
        {
            var unitDiff = l.Unit - r.Unit;
            long gridDiff = l.Grid - r.Grid;

            while (gridDiff < 0)
            {
                unitDiff = unitDiff - 1;
                gridDiff = gridDiff + l.GridRadix;
            }

            return new GridOffset(unitDiff, (int)gridDiff);
        }

        public abstract string Serialize();

        #region Implement Equals and Compares

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return obj is not GridBase g ? false : (g == this);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Unit, Grid, GridRadix);
        }

        public static bool operator <(GridBase left, GridBase right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(GridBase left, GridBase right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(GridBase left, GridBase right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(GridBase left, GridBase right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
        #endregion
    }
}
