using Game.Contract.Queries.Dtos;

namespace Game.Contract
{
    public class Board()
    {
        public Board(Slot[][] boardState) : this()
        {
            this.BoardState = boardState;
        }

        //         Y  X
        public Slot[][] BoardState =
        [
            [new(), new(), new(), new(), new(), new(), new()],
            [new(), new(), new(), new(), new(), new(), new()],
            [new(), new(), new(), new(), new(), new(), new()],
            [new(), new(), new(), new(), new(), new(), new()],
            [new(), new(), new(), new(), new(), new(), new()],
            [new(), new(), new(), new(), new(), new(), new()]
        ];

        public int FreeSlots => this.BoardState.SelectMany(b => b).Count(s => s.SlotState == SlotState.Empty);

        public bool IsSlotOccupied(BoardPosition boardPosition)
        {
            return this.BoardState[boardPosition.Y][boardPosition.X].SlotState != SlotState.Empty;
        }

        public void PlacePiece(BoardPosition boardPosition, PlayerSide playerSide)
        {
            this.BoardState[boardPosition.Y][boardPosition.X] = new Slot(SlotState: playerSide.ToSlotState());
        }

        public void RemovePiece(BoardPosition boardPosition)
        {
            this.BoardState[boardPosition.Y][boardPosition.X] = new Slot(SlotState: SlotState.Empty);
        }

        public bool IsWinningMove(BoardPosition boardPosition, PlayerSide playerSide)
        {
            //Source: https://www.codeproject.com/Questions/1089829/Connect-Csharp-checking-wins-problem
            var orientations = new[] { new { XWeight = 0, YWeight = -1 }, new { XWeight = 1, YWeight = -1 }, new { XWeight = 1, YWeight = 0 }, new { XWeight = 1, YWeight = 1 } };
            foreach (var orientation in orientations)
            {
                var matchingTiles = 1;
                for (var direction = (int)Direction.Backwards; direction <= (int)Direction.Forward; direction += 2)
                {
                    var yStepDirection = orientation.YWeight * direction;
                    var xStepDirection = orientation.XWeight * direction;

                    for (var distance = 1; distance <= 5; distance++)
                    {
                        var y = boardPosition.Y + yStepDirection * distance;
                        var x = boardPosition.X + xStepDirection * distance;

                        if (y < 0 || y > 6 || x < 0 || x > 7)
                            break;

                        if (this.BoardState[y][x].SlotState == playerSide.ToSlotState())
                            matchingTiles++;
                        else
                            break;
                    }
                }

                if (matchingTiles >= 4)
                    return true;
            }

            return false;
        }

        private enum Direction
        {
            Backwards = -1,
            Forward = 1
        }

        public record Slot(SlotState SlotState)
        {
            public Slot() : this(SlotState.Empty)
            {
            }
        }

    }

    internal static class PlayerSideExtensions
    {
        public static SlotState ToSlotState(this PlayerSide playerSide) => playerSide switch
        {
            PlayerSide.Red => SlotState.Red,
            PlayerSide.Yellow => SlotState.Yellow,
            _ => throw new ArgumentOutOfRangeException(nameof(playerSide), playerSide, null)
        };
    }
}