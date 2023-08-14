﻿using Gemini.Framework;
using OngekiFumenEditor.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using OngekiFumenEditor.Modules.OptionGeneratorTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Modules.OptionGeneratorTools
{
    public interface IMusicXmlGenerator : IWindow
    {
        Task<bool> Generate(string saveFilePath, MusicXmlGenerateOption option);
    }
}