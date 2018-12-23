using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SudokuSolverLib.Tools;

namespace SudokuSolverLib
{
    public struct Step
    {
        public Step(int value, int rowIndex, int columnIndex)
        {
            Value = value;
            Row = rowIndex;
            Column = columnIndex;
        }

        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Value { get; private set; }
    }

    public enum Constraint : byte
    {
        Unknown, Setted, Locked
    };

    public interface IConstraintCore
    {
        int LeftCell { get; }

        /// <summary>
        /// Gets the core.
        /// <para>index orders : 0 layerIndex, 1 rowIndex, 2 columnIndex</para>
        /// </summary>
        /// <value>
        /// The core.
        /// </value>
        Constraint[,,] Core { get; }
        int Size { get; }
        int[,] Grid { get; }
        int[,] PossibilityGrid { get; }
        List<Step> Steps { get; }
        bool Set(int rowIndex, int columnIndex, int value, bool registerStep);
    }

    public class ConstraintCore : IConstraintCore
    {
        public int Size
        {
            get { return size; }
        }

        private readonly int size;

        public int LeftCell { get; private set; }
        /// <summary>
        /// Gets the core.
        /// <para>index orders : 0 layerIndex, 1 rowIndex, 2 columnIndex</para>
        /// </summary>
        /// <value>
        /// The core.
        /// </value>
        public Constraint[,,] Core { get; private set; }
        public int[,] Grid { get; private set; }
        public int[,] PossibilityGrid { get; private set; }

        public List<Step> Steps { get; private set; }

        public ConstraintCore()
        {
            size = 9;
            LeftCell = 81;
            Core = InitializeCore();
            Grid = InitializeGrids(0);
            PossibilityGrid = InitializeGrids(9);
            Steps = new List<Step>();
        }

        public ConstraintCore(int desiredSize)
        {
            size = desiredSize;
            LeftCell = 81;
            Core = InitializeCore();
            Grid = InitializeGrids(0);
            PossibilityGrid = InitializeGrids(9);
            Steps = new List<Step>();
        }

        private int[,] copyGrid(int size, int[,] gridToCopy)
        {
            int[,] result = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    result[i, j] = gridToCopy[i, j];
                }
            }

            return result;
        }

        private Constraint[,,] copyCore(int size, Constraint[,,] coreToCopy)
        {
            Constraint[,,] result = new Constraint[size, size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        result[i, j, k] = coreToCopy[i, j, k];
                    }
                }
            }

            return result;
        }

        public ConstraintCore(ConstraintCore coreToCopyFrom)
        {
            this.size = coreToCopyFrom.Size;
            this.LeftCell = coreToCopyFrom.LeftCell;
            this.Grid = copyGrid(coreToCopyFrom.Size, coreToCopyFrom.Grid);
            this.PossibilityGrid = copyGrid(coreToCopyFrom.Size, coreToCopyFrom.PossibilityGrid);
            this.Core = copyCore(coreToCopyFrom.Size, coreToCopyFrom.Core);
            this.Steps = new List<Step>(coreToCopyFrom.Steps);
        }

        private Constraint[,,] InitializeCore()
        {
            Constraint[,,] tmp = new Constraint[this.Size, this.Size, this.Size];

            for (byte layerIndex = 0; layerIndex < this.Size; layerIndex++)
            {
                for (byte rowIndex = 0; rowIndex < this.Size; rowIndex++)
                {
                    for (byte columnIndex = 0; columnIndex < this.Size; columnIndex++)
                    {
                        tmp[layerIndex, rowIndex, columnIndex] = Constraint.Unknown;
                    }
                }
            }

            return tmp;
        }

        private int[,] InitializeGrids(int value)
        {
            int[,] tmp = new int[this.Size, this.Size];
            for (int rowIndex = 0; rowIndex < this.Size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.Size; columnIndex++)
                {
                    tmp[rowIndex, columnIndex] = value;
                }
            }

            return tmp;
        }

        private void SetRowConstraint(int layerIndex, int rowIndex, int columnIndex)
        {
            for (int ite = 0; ite < rowIndex; ite++)
            {
                if (Core[layerIndex, ite, columnIndex] == Constraint.Unknown)
                {
                    Core[layerIndex, ite, columnIndex] = Constraint.Locked;
                    PossibilityGrid[ite, columnIndex]--;
                }
            }

            for (int ite = this.Size - 1; ite > rowIndex; ite--)
            {
                if (Core[layerIndex, ite, columnIndex] == Constraint.Unknown)
                {
                    Core[layerIndex, ite, columnIndex] = Constraint.Locked;
                    PossibilityGrid[ite, columnIndex]--;
                }
            }
        }

        private void SetColumnConstraint(int layerIndex, int rowIndex, int columnIndex)
        {
            for (int ite = 0; ite < columnIndex; ite++)
            {
                if (Core[layerIndex, rowIndex, ite] == Constraint.Unknown)
                {
                    Core[layerIndex, rowIndex, ite] = Constraint.Locked;
                    PossibilityGrid[rowIndex, ite]--;
                }
            }

            for (int ite = this.Size - 1; ite > columnIndex; ite--)
            {
                if (Core[layerIndex, rowIndex, ite] == Constraint.Unknown)
                {
                    Core[layerIndex, rowIndex, ite] = Constraint.Locked;
                    PossibilityGrid[rowIndex, ite]--;
                }
            }
        }

        private void SetHeightConstraint(int layerIndex, int rowIndex, int columnIndex)
        {
            for (int ite = 0; ite < layerIndex; ite++)
            {
                Core[ite, rowIndex, columnIndex] = Constraint.Locked;
            }

            for (int ite = this.Size - 1; ite > layerIndex; ite--)
            {
                Core[ite, rowIndex, columnIndex] = Constraint.Locked;
            }
        }

        private void SetSquareConstraint(int layerIndex, int rowIndex, int columnIndex)
        {
            int sizeSqrt = (int)Math.Sqrt((double)Size);

            int startRowIndex = (rowIndex / sizeSqrt) * sizeSqrt;
            int startColumnIndex = (columnIndex / sizeSqrt) * sizeSqrt;

            for (int currentRowIndex = startRowIndex; currentRowIndex < startRowIndex + sizeSqrt; currentRowIndex++)
            {
                for (int currentColumnIndex = startColumnIndex; currentColumnIndex < startColumnIndex + sizeSqrt; currentColumnIndex++)
                {
                    if (currentRowIndex != rowIndex && currentColumnIndex != columnIndex)
                    {
                        if (Core[layerIndex, currentRowIndex, currentColumnIndex] == Constraint.Unknown)
                        {
                            Core[layerIndex, currentRowIndex, currentColumnIndex] = Constraint.Locked;
                            PossibilityGrid[currentRowIndex, currentColumnIndex]--;
                        }
                    }
                }
            }
        }


        private bool SetValue(int rowIndex, int columnIndex, int value)
        {
            int layerIndex = value - 1;

            if (this.Core[layerIndex, rowIndex, columnIndex] != Constraint.Unknown)
                return false;

            SetRowConstraint(layerIndex, rowIndex, columnIndex);
            SetColumnConstraint(layerIndex, rowIndex, columnIndex);
            SetHeightConstraint(layerIndex, rowIndex, columnIndex);
            SetSquareConstraint(layerIndex, rowIndex, columnIndex);

            Core[layerIndex, rowIndex, columnIndex] = Constraint.Setted;

            LeftCell--;
            PossibilityGrid[rowIndex, columnIndex] = 0;
            Grid[rowIndex, columnIndex] = value;

            DebugTool.DisplayArray(nameof(PossibilityGrid), PossibilityGrid);

            return true;
        }


        public bool Set(int rowIndex, int columnIndex, int value, bool registerStep)
        {
            bool res = this.SetValue(rowIndex, columnIndex, value);
            //Debug.WriteLine(string.Format("step add: {0} in [{1},{2}]", value, rowIndex, columnIndex), this.GetType().Name);
            Steps.Add(new Step(value, rowIndex, columnIndex));
            return res;
        }
    }
}
