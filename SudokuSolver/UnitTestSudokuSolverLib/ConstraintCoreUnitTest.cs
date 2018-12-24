using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolverLib;

namespace UnitTestSudokuSolverLib
{
    [TestClass]
    public class ConstraintCoreUnitTest
    {
        private void CheckGrids(int size, int[,] expectedGrid, int[,] gridToCheck)
        {
            for (int currentRowIndex = 0; currentRowIndex < size; currentRowIndex++)
            {
                for (int currentColumnIndex = 0; currentColumnIndex < size; currentColumnIndex++)
                {
                    Assert.AreEqual(expectedGrid[currentRowIndex, currentColumnIndex],
                        gridToCheck[currentRowIndex, currentColumnIndex],
                        $"Location : [{currentRowIndex},{currentColumnIndex}]");
                }
            }
        }

        private void CheckLayer(int size, Constraint[,] expectedLayer, Constraint[,,] coreToCheck, int layerIndex)
        {
            for (int currentRowIndex = 0; currentRowIndex < size; currentRowIndex++)
            {
                for (int currentColumnIndex = 0; currentColumnIndex < size; currentColumnIndex++)
                {
                    Assert.AreEqual(expectedLayer[currentRowIndex, currentColumnIndex],
                        coreToCheck[layerIndex, currentRowIndex, currentColumnIndex],
                        $"Location : [{layerIndex},{currentRowIndex},{currentColumnIndex}]");
                }
            }
        }

        private void CheckCoreLayer(Constraint[,] expectedLayer, ConstraintCore constraintCoreToCheck, int layerIndex)
        {
            CheckLayer(constraintCoreToCheck.Size, expectedLayer, constraintCoreToCheck.Core, layerIndex);
        }


        [TestMethod]
        [Description("Check nominal case when setting a value")]
        public void TestSetSuccess()
        {
            ConstraintCore target = new ConstraintCore(9);

            int[,] expectedGrid = new int[,]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            int[,] expectedPossibilityGrid = new int[,]
            {
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9},
                {9, 9, 9, 9, 9, 9, 9, 9, 9}
            };

            Constraint[,] expectedLayer = new Constraint[,]
            {
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
            };

            CheckGrids(target.Size, expectedGrid, target.Grid);
            CheckGrids(target.Size, expectedPossibilityGrid, target.PossibilityGrid);
            CheckLayer(target.Size, expectedLayer, target.Core, 0);
            Assert.AreEqual(81, target.LeftCell);
            Assert.AreEqual(0,target.Steps.Count);

            Assert.IsTrue(target.Set(0, 0, 1,true));

            expectedGrid = new int[,]
            {
                {1, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            expectedPossibilityGrid = new int[,]
            {
                {0, 8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 9, 9, 9, 9, 9, 9},
                {8, 8, 8, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9},
                {8, 9, 9, 9, 9, 9, 9, 9, 9}
            };

            expectedLayer = new Constraint[,]
            {
                {
                    Constraint.Setted, Constraint.Locked, Constraint.Locked, Constraint.Locked, Constraint.Locked,
                    Constraint.Locked, Constraint.Locked, Constraint.Locked, Constraint.Locked
                },
                {
                    Constraint.Locked, Constraint.Locked, Constraint.Locked, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Locked, Constraint.Locked, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
                {
                    Constraint.Locked, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown,
                    Constraint.Unknown, Constraint.Unknown, Constraint.Unknown, Constraint.Unknown
                },
            };

            CheckGrids(target.Size, expectedGrid, target.Grid);
            CheckGrids(target.Size, expectedPossibilityGrid, target.PossibilityGrid);
            CheckLayer(target.Size, expectedLayer, target.Core, 0);
            Assert.AreEqual(80, target.LeftCell);
            Assert.AreEqual(1, target.Steps.Count);
        }

        [TestMethod]
        [Description("Detect Row constraint violation")]
        public void TestSetFail_RowViolation()
        {
            ConstraintCore target = new ConstraintCore(9);
            Assert.IsTrue(target.Set(5, 0, 1, true), "Failed to init first value in empty Core");
            Assert.IsFalse(target.Set(5, 5, 1, true));
            Assert.IsFalse(target.Set(5, 8, 1, true));
            Assert.AreEqual(80, target.LeftCell);

            //target = new ConstraintCore(25);
            //Assert.IsTrue(target.Set(5, 0, 1, true), "Failed to init first value in empty Core");
            //Assert.IsFalse(target.Set(5, 16, 1, true));
            //Assert.IsFalse(target.Set(5, 23, 1, true));
            //// 25 * 25 - 1 = 624
            //Assert.AreEqual(624, target.LeftCell);
        }

        [TestMethod]
        [Description("Detect Column constraint violation")]
        public void TestSetFail_ColumnViolation()
        {
            ConstraintCore target = new ConstraintCore(9);
            Assert.IsTrue(target.Set(0, 5, 1, true), "Failed to init first value in empty Core");
            Assert.IsFalse(target.Set(5, 5, 1, true));
            Assert.IsFalse(target.Set(8, 5, 1, true));
            Assert.AreEqual(80, target.LeftCell);

            //target = new ConstraintCore(25);
            //Assert.IsTrue(target.Set(0, 5, 1, true), "Failed to init first value in empty Core");
            //Assert.IsFalse(target.Set(16, 5, 1, true));
            //Assert.IsFalse(target.Set(23, 5, 1, true));
            //// 25 * 25 - 1 = 624
            //Assert.AreEqual(624, target.LeftCell);
        }

        [TestMethod]
        [Description("Detect Square constraint violation")]
        public void TestSetFail_SquareViolation()
        {
            ConstraintCore target = new ConstraintCore(9);
            Assert.IsTrue(target.Set(0, 0, 1, true), "Failed to init first value in empty Core");
            Assert.IsFalse(target.Set(1, 1, 1, true));
            Assert.IsFalse(target.Set(2, 2, 1, true));
            Assert.AreEqual(80, target.LeftCell);

            //target = new ConstraintCore(4);
            //Assert.IsTrue(target.Set(0, 0, 1, true), "Failed to init first value in empty Core");
            //Assert.IsFalse(target.Set(1, 1, 1, true));
            //Assert.AreEqual(15, target.LeftCell);
        }

        [TestMethod]
        public void TestSetSteps()
        {
            List<Step> expectedSteps = new List<Step>()
            {
                new Step(1,0,0),
                new Step(2,1,1),
                new Step(3,2,2),
                new Step(4,1,0),
                new Step(5,0,1),
                new Step(6,5,5)
            };

            ConstraintCore target = new ConstraintCore(9);
            Assert.IsTrue(target.Set(0, 0, 1, true));
            Assert.IsTrue(target.Set(1, 1, 2, true));
            Assert.IsTrue(target.Set(2, 2, 3, true));
            Assert.IsTrue(target.Set(1, 0, 4, true));
            Assert.IsTrue(target.Set(0, 1, 5, true));
            Assert.IsTrue(target.Set(5, 5, 6, true));

            Assert.AreEqual(expectedSteps.Count, target.Steps.Count);
            for (int i = 0; i < expectedSteps.Count; i++)
            {
                Assert.AreEqual(expectedSteps.ElementAt(i), target.Steps.ElementAt(i));
            }
        }

    }
}
