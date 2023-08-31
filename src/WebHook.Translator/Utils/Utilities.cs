using System.Text;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using WebHook.Translator.Common;
using WebHook.Translator.Models;
using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Utils;

public static partial class Utilities
{
    public static InlineKeyboardMarkup ParseCollectionKeyboardMarkup(
        int columns,
        int messageId,
        MarkupType markupType,
        JsonSerializerOptions jsonSerializerOptions,
        IEnumerable<IBase>? models = null)
    {
        var buttonList = new List<IEnumerable<InlineKeyboardButton>>();
        var row = new List<InlineKeyboardButton>();

        var list = models?.ToList();

        for (int i = 0; i < list?.Count; i++)
        {
            string text = list[i].ToString()!;
            var response = new ChoiceResponse() { Id = messageId };

            if (list[i] is TestViewModel tsm)
                response.Code = $"{tsm.OptionId}_{(int)markupType}_{list[i].Code}";
            else
                response.Code = $"{(int)markupType}_{list[i].Code}";

            var json = JsonSerializer.Serialize(response, jsonSerializerOptions);

            row.Add(InlineKeyboardButton.WithCallbackData(
                text: text,
                callbackData: json));

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
