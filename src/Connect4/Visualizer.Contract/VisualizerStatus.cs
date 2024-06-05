using System.Text.Json.Serialization;

namespace Visualizer.Contract
{
    public class VisualizerStatus
    {
        public static VisualizerStatus Idle = new VisualizerStatus(0, "Idle", StatusType.Ready);
        public static VisualizerStatus Processing = new VisualizerStatus(1, "Verarbeitet Anfrage", StatusType.Busy);
        public static VisualizerStatus PlacingPiece = new VisualizerStatus(2, "Spielstein wird platziert", StatusType.Busy);
        public static VisualizerStatus PiecePlaced = new VisualizerStatus(3, "Spielstein platziert", StatusType.Ready);
        public static VisualizerStatus SortingPieces = new VisualizerStatus(4, "Sortiere Spielsteine", StatusType.Busy);
        public static VisualizerStatus PiecesSorted = new VisualizerStatus(5, "Spielsteine sortiert", StatusType.Ready);
        public static VisualizerStatus Faulty = new VisualizerStatus(8, "Fehlerhaft", StatusType.Faulty);
        public static VisualizerStatus Unknown = new VisualizerStatus(16, "Unbekannt", StatusType.Faulty);
        public static VisualizerStatus NotInitialized = new VisualizerStatus(32, "Nicht Initialisiert", StatusType.Faulty);

        public int Id { get; }
        public string DisplayText { get; }

        public StatusType StatusType { get; }

        [JsonConstructor]
        private VisualizerStatus(int id, string displayText, StatusType statusType)
        {
            this.Id = id;
            this.DisplayText = displayText;
            this.StatusType = statusType;
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