using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SudokuSolverLib
{
    public class Solver
    {
        private readonly INode rootNode;
        private INode currentNode;
        private readonly List<INode> tree;

        private int[,] grid;

        public Solver()
        {
            tree = new List<INode>();
            rootNode = new Node();
            tree.Add(rootNode);
            UpdateCurrentNodeTo(rootNode);
        }

        public bool SetValue(int rowIndex, int columnIndex, int value)
        {
            return this.rootNode.Set(new Location(rowIndex, columnIndex), value, false);
        }

        private void AddChild(int rowIndex, int columnIndex, int value)
        {
            INode nextNode = new Node(currentNode, rowIndex, columnIndex, value);
            currentNode.Children.Add(nextNode);
            tree.Add(nextNode);
        }

        private void UpdateCurrentNodeTo(INode nextNode)
        {
            currentNode = nextNode;
        }

        public bool Solve(out int[,] grid)
        {
            Location currentLocation;
            while (currentNode.GetUniquePossibility(out currentLocation))
            {
                int valueToSet = currentNode.GetUniqueValue(currentLocation);

                if (valueToSet > 0)
                {
                    if (!currentNode.Set(currentLocation, valueToSet, true))
                    {
                        grid = currentNode.GetConstraintCore().Grid;
                        return false; // error
                    }
                }
                else
                {
                    grid = currentNode.GetConstraintCore().Grid;
                    return false; // no unique value found
                }

            }

            grid = currentNode.GetConstraintCore().Grid;
            return currentNode.IsSolved();
        }

    }
}
