using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SudokuSolverLib
{
    public interface INode
    {
        List<INode> Children { get; set; }
        INode Parent { get; }

        int selectedRowIndex { get; }
        int selectedColumnIndex { get; }
        int selectedValue { get; }

        bool Set(Location location, int value, bool registerStep);
        bool GetUniquePossibility(out Location uniquePossibilityLocation);
        int GetUniqueValue(Location location);
        bool IsSolved();
        IConstraintCore GetConstraintCore();
    }

    internal class Node : INode
    {
        public readonly IConstraintCore constraintCore;

        public Node()
        {
            constraintCore = new ConstraintCore();
            Parent = null;
        }

        public Node(ConstraintCore constraintCoreToCopy)
        {
            constraintCore = new ConstraintCore(constraintCoreToCopy);
            Parent = null;
        }

        public Node(INode parent)
        {
            Parent = parent;
        }

        public Node(INode parent, int rowIndexIndex, int columnIndexIndex, int value)
        {
            Parent = parent;
            selectedRowIndex = rowIndexIndex;
            selectedColumnIndex = columnIndexIndex;
            selectedValue = value;
        }

        public List<INode> Children { get; set; }
        public INode Parent { get; }
        public int selectedRowIndex { get; private set; }
        public int selectedColumnIndex { get; private set; }
        public int selectedValue { get; private set; }

        public bool Set(Location location, int value, bool registerStep)
        {
            return this.constraintCore.Set(location.RowIndex, location.ColumnIndex, value, registerStep);
        }

        public bool GetUniquePossibility(out Location uniquePossibilityLocation)
        {
            for (int i = 0; i < constraintCore.Size; i++)
            {
                for (int j = 0; j < constraintCore.Size; j++)
                {
                    if (this.constraintCore.PossibilityGrid[i, j] == 1)
                    {
                        uniquePossibilityLocation = new Location(i, j);
                        return true;
                    }
                }
            }

            uniquePossibilityLocation = null;
            return false;
        }

        public int GetUniqueValue(Location location)
        {
            for (int i = 0; i < constraintCore.Size; i++)
            {
                if (constraintCore.Core[i, location.RowIndex, location.ColumnIndex]==Constraint.Unknown)
                {
                    return i+1;
                }
            }

            return -1;
        }

        public bool IsSolved()
        {
            return constraintCore.LeftCell == 0;
        }

        public IConstraintCore GetConstraintCore()
        {
            return constraintCore;
        }


    }
}
