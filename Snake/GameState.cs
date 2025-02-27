using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class GameState
    {
        public int Rows { get; }

        public int Columns { get; }

        public GridValue[,] Grid { get; }

        public Direction Direction { get; private set; }

        public int Score { get; private set; }

        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges= new LinkedList<Direction>();

        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();

        private readonly Random random = new Random();

        public GameState(int rows, int columns)
        {
            Rows = rows;

            Columns = columns;

            Grid = new GridValue[rows, columns];

            Direction = Direction.Right;

            AddSnake();
            AddFood();

        }

        private void AddSnake()
        {
            int r = Rows / 2;


            for (int i = 1; i <= 3; i++)
            {
                Grid[r, i] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, i));
            }

        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {

                    if (Grid[i, j] == GridValue.Empty)
                    {
                        yield return new Position(i, j);
                    }
                }

            }

        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Column] = GridValue.Food;

        }

        public Position HeadPosition()
        {

            return snakePositions.First.Value;
        }

        public Position TailPosition()
        {

            return snakePositions.Last.Value;

        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }
        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Column]=GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Column] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection( )
        {
            if ( dirChanges.Count==0 )

            {
                return Direction;
            }
            return dirChanges.Last.Value;
        }
        public void ChangeDirection(Direction direction)
        {
            if (CanChangeDirection(direction))
            {


                dirChanges.AddLast(direction);
            }
        }

        private bool CanChangeDirection(Direction newdir)
        {
            if (dirChanges.Count==2)
            {
                return false;

            }

            Direction lastdir = GetLastDirection();
            return newdir != lastdir && newdir != lastdir.Opposite();
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row<0 || pos.Row>=Rows||pos.Column<0 || pos.Column>=Columns;
        }

        private GridValue WillHit(Position newheadpos)
        {

            if (OutsideGrid(newheadpos))
            {
                return GridValue.Outside;
            }
            if (newheadpos==TailPosition())
            {
                return GridValue.Empty;
            }
            return Grid[newheadpos.Row, newheadpos.Column];


        }

        public void Move()
        {
            if (dirChanges.Count >0)
            {
                Direction = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }
            Position newheadpos=HeadPosition().Translate(Direction);
            GridValue hit=WillHit(newheadpos);
            if (hit == GridValue.Outside || hit==GridValue.Snake)

            {
                GameOver = true;
            }

            else if (hit== GridValue.Empty)
            {
                RemoveTail();
                AddHead(newheadpos);

            }
            else if (hit== GridValue.Food)
            {
                AddHead(newheadpos);
                Score++;
                AddFood();
            }
        }
    }
}
