using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using WebHook.Translator.Common;
using WebHook.Translator.Models;

namespace WebHook.Translator.Utils;

public static partial class Utilities
{
    public static InlineKeyboardMarkup ParseCollectionKeyboardMarkup(
        IEnumerable<IBase> models,
        int columns,
        int messageId,
        KeyboardDirection direction,
        MarkupType markupType,
        JsonSerializerOptions jsonSerializerOptions)
    {
        var buttonList = new List<IEnumerable<InlineKeyboardButton>>();
        var row = new List<InlineKeyboardButton>();

        foreach (var model in models)
        {
            var callbackData = new ChoiceResponse()
            {
                Code = $"{model.Code}_{(int)markupType}",
                Direction = direction,
                MessageId = messageId,
            };

            row.Add(InlineKeyboardButton.WithCallbackData(
                text: model.ToString()!,
                callbackData: JsonSerializer.Serialize(
                    value: callbackData,
                    options: jsonSerializerOptions)));

            if (row.Count == columns)
            {
                buttonList.Add(row.ToArray());
                row.Clear();
            }
        }

        if (row.Count > 0)
            buttonList.Add(row.ToArray());

        return new InlineKeyboardMarkup(buttonList);
    }
}
