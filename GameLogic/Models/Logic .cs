using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicLibrary.Models
{
    public static class Logic
    {
        public static void IntaliseGrid(UserModel user)
        {
            List<int> column = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7,
            };
            List<int> row = new List<int>()
            {
                1,2,3,4,5,6
            };

            foreach (int i in column)
            {
                foreach (int j in row)
                {
                    CreateGrid(user, i, j);
                }
            }

        }

        public static void CreateGrid(UserModel Player, int column, int row)
        {
            GridSpot spot = new GridSpot();
            {
                spot.YAxis = column;
                spot.XAxis = row;
            }
            Player.GridLocation.Add(spot);

        }

        public static bool ValidateShot(UserModel currentUser, int shot)
        {
            bool isValid = false;

            foreach (var xPos in currentUser.GridLocation)
            {
                if (xPos.XAxis == shot)
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        public static void UpdateGridShot(UserModel currentUser, UserModel opponent, int isUser1, int xShot, int yShot)
        {
            bool isProcessed = false;
            GridSpot newShot = new GridSpot();


            if (isUser1 == 1)
            {
                newShot.YAxis = yShot;
                newShot.XAxis = xShot;
                newShot.Status = EnumStatus.Circle;

            }
            else if (isUser1 == 2)
            {
                newShot.YAxis = yShot;
                newShot.XAxis = xShot;
                newShot.Status = EnumStatus.Crosses;
            }



            foreach (var yPos in currentUser.GridLocation)
            {
                if (yPos.YAxis == yShot)
                {
                    foreach (var xPos in currentUser.GridLocation)
                    {
                        if (xPos.XAxis == xShot)
                        {
                            int indexSpot = currentUser.GridLocation.FindIndex(spot => spot.XAxis == xShot && spot.YAxis == yShot);
                            int indexSpot2 = opponent.GridLocation.FindIndex(spot => spot.XAxis == xShot && spot.YAxis == yShot);

                            currentUser.GridLocation[indexSpot] = newShot;
                            opponent.GridLocation[indexSpot] = newShot;

                            isProcessed = true;
                            break;

                        }

                    }

                }
                if (isProcessed == true)
                {
                    break;
                }

            }
        }



        public static (bool, int) EmptyCheck(UserModel currentUser, int xShot)
        {
            bool isEmpty = false;
            int yShot = 0;
            bool isProcessed = false;


            List<GridSpot> gridX = new List<GridSpot>();
            List<GridSpot> gridY = new List<GridSpot>();

            foreach (var xPos in currentUser.GridLocation)
            {

                gridX.Add(xPos);

            }
            gridX.Reverse();

            foreach (var xPos in currentUser.GridLocation)
            {

                gridY.Add(xPos);

            }
            gridY.Reverse();


            foreach (var xPos in gridX)
            {
                // if (xPos.XAxis == yPos.XAxis)

                if (xPos.XAxis == xShot && xPos.Status == EnumStatus.Empty)
                {
                    yShot = xPos.YAxis;
                    isEmpty = true;
                    isProcessed = true;
                    break;
                }

            }

            return (isEmpty, yShot);
        }

        public static (UserModel, UserModel, int) UserTurn(UserModel user1, UserModel user2)
        {
            int counterUser1 = 0;
            int counterUser2 = 1;

            int isUser1 = 0;

            foreach (var shotCheck in user1.GridLocation)
            {
                if (shotCheck.Status == EnumStatus.Circle)
                {
                    counterUser1 += 1;
                }
            }

            foreach (var shotCheck2 in user2.GridLocation)
            {
                if (shotCheck2.Status == EnumStatus.Crosses)
                {
                    counterUser2 += 1;

                }
            }
            if (counterUser1 >= counterUser2)
            {
                isUser1 = 2;

                return (user2, user1, isUser1);
            }
            else
            {
                isUser1 = 1;

                return (user1, user2, isUser1);
            }
        }



        public static HashSet<(int, int)> CheckRowWinner(UserModel currentUser, int isUser1)
        {
            int winCounter = 4;

            var groupedByRow = currentUser.GridLocation
               .GroupBy(g => g.YAxis)
               .OrderByDescending(g => g.Key); ;

            HashSet<(int, int)> winningShotsCircles = new HashSet<(int, int)>();
            HashSet<(int, int)> winningShotsCrosses = new HashSet<(int, int)>();
            HashSet<(int, int)> defaultError = new HashSet<(int, int)>();

            foreach (var row in groupedByRow)
            {
                var sortedRow = row.OrderBy(g => g.XAxis).ToList();

                int countCircles = 0;
                int countCrosses = 0;

                for (int i = 0; i < sortedRow.Count(); i++)
                {
                    if (sortedRow[i].Status == EnumStatus.Circle)
                    {
                        winningShotsCircles.Add((sortedRow[i].YAxis, sortedRow[i].XAxis));
                        countCircles++;
                        winningShotsCrosses.Clear();
                        countCrosses = 0;

                        if (countCircles == winCounter)
                        {
                            return winningShotsCircles;
                        }
                    }
                    else if (sortedRow[i].Status == EnumStatus.Crosses)
                    {
                        winningShotsCrosses.Add((sortedRow[i].YAxis, sortedRow[i].XAxis));
                        countCrosses++;
                        winningShotsCircles.Clear();
                        countCircles = 0;

                        if (countCrosses == winCounter)
                        {
                            return winningShotsCrosses;
                        }

                    }
                    else
                    {
                        winningShotsCircles.Clear();
                        winningShotsCrosses.Clear();
                        countCircles = 0;
                        countCrosses = 0;
                    }

                }
            }

            return defaultError;

        }

        public static HashSet<(int, int)> CheckColumnWinner(UserModel currentUser, int isUser1)
        {
            int winCounter = 4;


            var groupedByColumn = currentUser.GridLocation
                .GroupBy(g => g.XAxis)
                .OrderBy(g => g.Key);

            HashSet<(int, int)> winningShotsCircles = new HashSet<(int, int)>();
            HashSet<(int, int)> winningShotsCrosses = new HashSet<(int, int)>();
            HashSet<(int, int)> defaultError = new HashSet<(int, int)>();

            foreach (var row in groupedByColumn)
            {
                var sortedRow = row.OrderByDescending(g => g.YAxis).ToList();
                int countCircles = 0;
                int countCrosses = 0;


                for (int i = 0; i < sortedRow.Count(); i++)
                {
                    if (sortedRow[i].Status == EnumStatus.Circle)
                    {
                        winningShotsCircles.Add((sortedRow[i].YAxis, sortedRow[i].XAxis));
                        countCircles++;
                        winningShotsCrosses.Clear();
                        countCrosses = 0;

                        if (countCircles == winCounter)
                        {
                            return winningShotsCircles;
                        }
                    }
                    else if (sortedRow[i].Status == EnumStatus.Crosses)
                    {
                        winningShotsCrosses.Add((sortedRow[i].YAxis, sortedRow[i].XAxis));
                        countCrosses++;
                        winningShotsCircles.Clear();
                        countCircles = 0;

                        if (countCrosses == winCounter)
                        {
                            return winningShotsCrosses;
                        }
                    }
                    else
                    {
                        winningShotsCircles.Clear();
                        winningShotsCrosses.Clear();
                        countCircles = 0;
                        countCrosses = 0;
                    }
                }
            }

            return defaultError;

        }

        public static (HashSet<(int, int)>, HashSet<(int, int)>) CheckDiagonalWinner(UserModel currentUser, int isUser1)
        {
            int winCounter = 4;

            var diagonal1 = currentUser.GridLocation
                .GroupBy(g => g.XAxis - g.YAxis)
                .OrderByDescending(g => g.Key); ;

            var diagonal2 = currentUser.GridLocation
                .GroupBy(g => g.YAxis + g.XAxis)
                .OrderByDescending(g => g.Key);

            var winningShots1 = CheckDiagonalGroup(diagonal1, currentUser, winCounter);
            var winningShots2 = CheckDiagonalGroup(diagonal2, currentUser, winCounter);

            return (winningShots1, winningShots2);
        }



        private static HashSet<(int, int)> CheckDiagonalGroup(IEnumerable<IGrouping<int, GridSpot>> diagonalGroup, UserModel currentUser, int winCounter)
        {

            HashSet<(int, int)> winningShotsCircles = new HashSet<(int, int)>();
            HashSet<(int, int)> winningShotsCrosses = new HashSet<(int, int)>();
            HashSet<(int, int)> defaultError = new HashSet<(int, int)>();

            foreach (var diagonal in diagonalGroup)
            {
                var sorteddiagonal = diagonal.OrderBy(g => g.XAxis).ToList();
                int countCircles = 0;
                int countCrosses = 0;


                for (int i = 0; i < sorteddiagonal.Count(); i++)
                {
                    if (sorteddiagonal.Count() >= 4 && sorteddiagonal[i].Status == EnumStatus.Circle)
                    {
                        winningShotsCircles.Add((sorteddiagonal[i].YAxis, sorteddiagonal[i].XAxis));
                        countCircles++;
                        winningShotsCrosses.Clear();
                        countCrosses = 0;

                        if (countCircles == winCounter)
                        {

                            return winningShotsCircles;
                        }
                    }
                    else if (sorteddiagonal.Count() >= 4 && sorteddiagonal[i].Status == EnumStatus.Crosses)
                    {
                        winningShotsCrosses.Add((sorteddiagonal[i].YAxis, sorteddiagonal[i].XAxis));
                        countCrosses++;
                        winningShotsCircles.Clear();
                        countCircles = 0;

                        if (countCrosses == winCounter)
                        {

                            return winningShotsCrosses;
                        }

                    }
                    else
                    {
                        winningShotsCircles.Clear();
                        winningShotsCrosses.Clear();
                        countCircles = 0;
                        countCrosses = 0;
                    }
                }
            }

            return (defaultError);
        }

        public static HashSet<(int, int)> CheckAllWinDirections(UserModel currentUser, int isUser1)
        {
            HashSet<(int, int)> winnerRow = new HashSet<(int, int)>();
            HashSet<(int, int)> winnerColumn = new HashSet<(int, int)>();

            (HashSet<(int, int)>, HashSet<(int, int)>) winnerDiaganol = (new HashSet<(int, int)>(), new HashSet<(int, int)>());


            winnerRow = CheckRowWinner(currentUser, isUser1);

            winnerColumn = CheckColumnWinner(currentUser, isUser1);

            winnerDiaganol = CheckDiagonalWinner(currentUser, isUser1);

            HashSet<(int, int)> unifiedDiganol = new HashSet<(int, int)>(winnerDiaganol.Item1);
            unifiedDiganol.UnionWith(winnerDiaganol.Item2);

            if (winnerRow.Count >= 4)
            {
                return winnerRow;
            }
            else if (winnerColumn.Count >= 4)
            {
                return winnerColumn;
            }
            else if (unifiedDiganol.Count >= 4)
            {
                return unifiedDiganol;
            }
            return null;

        }

        public static bool CheckIfDraw(UserModel currentUser)
        {
            int counter = 0;
            bool isDraw = false;


            foreach (var shot in currentUser.GridLocation)
            {
                if (shot.Status == EnumStatus.Empty)
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                isDraw = true;
                return isDraw;
            }
            return isDraw;
        }
    }
}


