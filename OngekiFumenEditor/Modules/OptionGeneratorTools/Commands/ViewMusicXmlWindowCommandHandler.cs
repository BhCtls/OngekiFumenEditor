﻿using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace OngekiFumenEditor.Modules.OptionGeneratorTools.Commands
{
    [CommandHandler]
    public class ViewMusicXmlWindowCommandHandler : CommandHandlerBase<ViewMusicXmlWindowCommandDefinition>
    {
        private readonly IWindowManager _windowManager;

        [ImportingConstructor]
        public ViewMusicXmlWindowCommandHandler(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public override async Task Run(Command command)
        {
            await _windowManager.ShowWindowAsync(IoC.Get<IMusicXmlGenerator>());
        }
    }
}