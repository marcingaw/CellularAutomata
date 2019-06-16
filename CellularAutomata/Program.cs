using System;

namespace CellularAutomata {

    class Cell {

        protected char state = ' ';
        public char State { get => state; }

        public virtual Cell NextState(
                char stateNW, char stateN, char stateNE,
                char stateW, char stateE,
                char stateSW, char stateS, char stateSE) => new Cell();

    }

    class ConwayCell : Cell {

        public ConwayCell(bool alive) { state = alive ? '@' : ' '; }

        public override Cell NextState(
                char stateNW, char stateN, char stateNE,
                char stateW, char stateE,
                char stateSW, char stateS, char stateSE) {
            int neighbors = (stateNW == '@' ? 1 : 0) +
                            (stateN == '@' ? 1 : 0) +
                            (stateNE == '@' ? 1 : 0) +
                            (stateW == '@' ? 1 : 0) +
                            (stateE == '@' ? 1 : 0) +
                            (stateSW == '@' ? 1 : 0) +
                            (stateS == '@' ? 1 : 0) +
                            (stateSE == '@' ? 1 : 0);
            return new ConwayCell(state == '@' ?
                                  ((neighbors == 2 || neighbors == 3) ? true : false) :
                                  (neighbors == 3 ? true : false));
        }
    }

    class Program {

        static void Main(string[] args) {
            Cell[][] field = new Cell[Console.WindowHeight - 1][];

            {
                Random rndGen = new Random();
                for (int k = 0; k < field.Length; k++) {
                    field[k] = new Cell[Console.WindowWidth - 1];
                    for (int l = 0; l < field[k].Length; l++) {
                        field[k][l] = new ConwayCell(rndGen.Next(5) == 0 ? true : false);
                    }
                }
            }

            do {

                Console.SetCursorPosition(0, 0);
                for (int k = 0; k < field.Length; k++) {
                    for (int l = 0; l < field[k].Length; l++) {
                        Console.Write(field[k][l].State);
                    }
                    Console.WriteLine();
                }

                Cell[][] newField = new Cell[field.Length][];
                for (int k = 0; k < newField.Length; k++) {
                    newField[k] = new Cell[field[k].Length];
                    int rowN = k > 0 ? k - 1 : newField.Length - 1;
                    int rowS = k < newField.Length - 1 ? k + 1 : 0;
                    for (int l = 0; l < newField[k].Length; l++) {
                        int colW = l > 0 ? l - 1 : newField[k].Length - 1;
                        int colE = l < newField[k].Length - 1 ? l + 1 : 0;
                        newField[k][l] = field[k][l].NextState(
                                field[rowN][colW].State,
                                field[rowN][l].State,
                                field[rowN][colE].State,
                                field[k][colW].State,
                                field[k][colE].State,
                                field[rowS][colW].State,
                                field[rowS][l].State,
                                field[rowS][colE].State);
                    }
                }
                field = newField;

            } while (Console.ReadKey(true).KeyChar == ' ');
        }

    }

}
