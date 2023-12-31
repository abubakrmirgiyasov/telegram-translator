﻿#nullable disable

using WebHook.Translator.Models.Interfaces;

namespace WebHook.Translator.Models;

public class Game : IBase
{
    public string Type { get; set; }

    public string Ico { get; set; }

    public string Code { get; set; }

    public override string ToString()
    {
        return $"{Ico} {Type}";
    }
}
