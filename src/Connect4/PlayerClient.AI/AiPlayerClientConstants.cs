using PlayerClient.Contract;

namespace PlayerClient.AI
{
    public static class AiPlayerClientConstants
    {
        public const string AiPrefix = "ai";
        public static PlayerClientType EasyPlayerClientType = new($"{AiPrefix}-easy");
        public static PlayerClientType MediumPlayerClientType = new($"{AiPrefix}-medium");
        public static PlayerClientType HardPlayerClientType = new($"{AiPrefix}-hard");
    }
}