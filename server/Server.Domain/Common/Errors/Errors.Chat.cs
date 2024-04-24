using ErrorOr;

namespace Server.Domain.Common.Errors;

public static partial class Errors
{
    public static class Chat
    {
        public static Error CannotFound => Error.NotFound(
            code: "Chat.PrivateChatCannotFound",
            description: "The conversation can not found."
        );

        public static Error SenderCannotFound => Error.NotFound(
          code: "Chat.SenderCannotFound",
          description: "The sender can not found."
      );

        public static Error ReceiverCannotFound => Error.NotFound(
          code: "Chat.ReceiverCannotFound",
          description: "The receiver can not found."
      );

    }
}