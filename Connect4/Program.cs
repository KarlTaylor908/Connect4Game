using GameLogicLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;

WelcomeMessage();

UserModel user1 = new UserModel();
UserModel user2 = new UserModel();

user1.UserName = AskForUserName("Player One's (circles)");
user2.UserName = AskForUserName("Player Two's (crosses)");

Console.Clear();

Logic.IntaliseGrid(user1);
Logic.IntaliseGrid(user2);


int xShot;
bool isValidShot;
bool isEmpty;
int yShot;
int isUser1;
bool isDraw = false;
HashSet<(int, int)> winnerShots = new HashSet<(int, int)>();

UserModel currentUser = new UserModel();
UserModel opponent = new UserModel();

do
{
    (currentUser, opponent, isUser1) = Logic.UserTurn(user1, user2);

    DisplayGrid(currentUser, winnerShots);

    do
    {
        xShot = PickShot(currentUser);

        isValidShot = Logic.ValidateShot(currentUser, xShot);

        if (isValidShot == false)
        {
            Console.WriteLine("This shot is invalid, please try again.");
        }


        (isEmpty, yShot) = Logic.EmptyCheck(currentUser, xShot);

        if (!isEmpty)
        {
            Console.WriteLine("This column is full, please choose another column.");
        }

    }
    while (isValidShot == false || isEmpty == false);

    Logic.UpdateGridShot(currentUser, opponent, isUser1, xShot, yShot);

    isDraw = Logic.CheckIfDraw(currentUser);

    winnerShots = Logic.CheckAllWinDirections(currentUser, isUser1);



} while (winnerShots == null &&  isDraw == false);

if (isDraw)
{
    DisplayGrid(currentUser, winnerShots);
    DrawMessage(currentUser);
}

else
{
    DisplayGrid(currentUser, winnerShots);
    Congratulations(currentUser);
}
void DrawMessage(UserModel currentUser)
{
    Console.WriteLine();
    Console.WriteLine($"All spaces are filled, the game is a Draw!!");
}



void Congratulations(UserModel currentUser)
{
    Console.WriteLine();
    Console.WriteLine($"Well done user {currentUser.UserName}, you won the game!!");
}
int PickShot(UserModel user1)
{
    int shotNumber = 0;
    bool isNumeric = false;

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine($"{user1.UserName} it is your turn");

    do
    {

        Console.Write("Enter a number of (1-6) to enter your shot: ");
        string shotString = Console.ReadLine();


        (isNumeric, shotNumber) = CheckNumeric(user1, shotString);

    } while (isNumeric == false);


    return shotNumber;

}


(bool, int) CheckNumeric(UserModel user1, string shotString)
{
    bool isNumeric = int.TryParse(shotString, out int shotNumber);

    if (!isNumeric)
    {
        Console.WriteLine();
        Console.WriteLine("Your number is in an incorrect format.");
    }

    return (isNumeric, shotNumber);
}

void DisplayGrid(UserModel currentUser, HashSet<(int, int)> winnerShots)
{


    foreach (var getY in currentUser.GridLocation)
    {
        bool checkIfProcessed = false;

        foreach (var previousColumn in currentUser.GridLocation)
        {
            if (previousColumn == getY)
            {

                break;
            }


            if (previousColumn.YAxis == getY.YAxis)
            {
                checkIfProcessed = true;
                break;
            }
        }

        if (!checkIfProcessed && getY.YAxis >= getY.XAxis)
        {
            Console.WriteLine();
            Console.WriteLine("-------------------------");
            Console.Write("|");

            foreach (var getX in currentUser.GridLocation)
            {
                if (getY.YAxis == getX.YAxis)
                {
                    if (getX.XAxis >= getY.XAxis)
                    {
                        if (getX.Status == EnumStatus.Empty)
                        {

                            // Console.Write($"X: {getX.YAxis}");
                            Console.Write($"   ");
                            Console.Write("|");
                        }

                        if (getX.Status == EnumStatus.Circle)
                        {
                            if (winnerShots != null && winnerShots.Contains((getX.YAxis, getX.XAxis)))
                            {
                                Console.Write(" O ");
                                Console.Write("|");
                            }
                            else 
                            {
                                Console.Write($" o ");
                                Console.Write("|");
                            }
                        }

                        if (getX.Status == EnumStatus.Crosses)
                        {
                            if (winnerShots != null && winnerShots.Contains((getX.YAxis, getX.XAxis)))
                            {
                                Console.Write(" X ");
                                Console.Write("|");
                            }
                            else 

                            {
                                Console.Write($" x ");
                                Console.Write("|");
                            }
                        }
                    }
                }
            }
        }
    }
    Console.WriteLine();
    Console.WriteLine("-------------------------");
    Console.Write("|");
    for (int i = 1; i <= 6; i++)
    {
        Console.Write($" {i} |");
    }
}


string AskForUserName(string player)
{
    Console.Write($"Enter {player} Name: ");
    string nameOne = Console.ReadLine();



    return nameOne;
}

void WelcomeMessage()
{
    Console.WriteLine("Hi, welcome to the Connect 4 Game");
    Console.WriteLine("The aim of the game is to get 4 Circles or Crosses in a row!");
    Console.WriteLine("Created By Karl Taylor");
    Console.WriteLine();
}

