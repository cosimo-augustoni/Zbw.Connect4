using System.Text.Json.Serialization;

namespace Visualizer.Contract
{
    public class VisualizerStatus
    {
        public static VisualizerStatus Idle = new VisualizerStatus(0, "Idle");
        public static VisualizerStatus Processing = new VisualizerStatus(1, "Verarbeitet Anfrage");
        public static VisualizerStatus PlacingPiece = new VisualizerStatus(2, "Spielstein wird platziert");
        public static VisualizerStatus PiecePlaced = new VisualizerStatus(3, "Spielstein platziert");
        public static VisualizerStatus SortingPieces = new VisualizerStatus(4, "Sortiere Spielfeld");
        public static VisualizerStatus PiecesSorted = new VisualizerStatus(5, "Spielfeld sortiert");
        public static VisualizerStatus Faulty = new VisualizerStatus(8, "Fehlerhaft");
        public static VisualizerStatus Unknown = new VisualizerStatus(16, "Unbekannt");

        public int Id { get; }
        public string DisplayText { get; }

        [JsonConstructor]
        private VisualizerStatus(int id, string displayText)
        {
            this.Id = id;
            this.DisplayText = displayText;
        }

        private static IEnumerable<VisualizerStatus> List()
        {
            return new[] { Idle, Processing, PlacingPiece, PiecePlaced, SortingPieces, PiecesSorted, Faulty, Unknown };
        }

        public static VisualizerStatus GetById(int id)
        {
            return List().First(s => s.Id == id);
        }
    }
}