using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput;
using ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Views.Extend;
using ContentTypeTextNet.Pe.Main.Views.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Views.Note;
using ContentTypeTextNet.Pe.Main.Views.StandardInputOutput;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Main.Models.Element.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Views.ExtendsExecute;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using ContentTypeTextNet.Pe.Main.Views.Setting;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.ViewModels.Command;
using ContentTypeTextNet.Pe.Main.Views.Command;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.ViewModels.Feedback;
using ContentTypeTextNet.Pe.Main.Views.Feedback;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public class OrderWindowParameter
    {
        public OrderWindowParameter(WindowKind windowKind, ElementBase element)
        {
            WindowKind = windowKind;
            Element = element;
        }

        #region property

        public WindowKind WindowKind { get; }
        public ElementBase Element { get; }

        #endregion
    }

    public enum UpdateTarget
    {
        Application,
    }
    public enum UpdateProcess
    {
        Download,
        Update,
    }

    /// <summary>
    /// アプリケーションに対して指示発行を受け付ける役所。
    /// </summary>
    public interface IOrderManager
    {
        #region function

        void StartUpdate(UpdateTarget target, UpdateProcess process);

        LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId);
        LauncherToolbarElement CreateLauncherToolbarElement(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups);
        LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId);
        LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, IScreen screen, LauncherIconElement iconElement);
        ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen);
        LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, IScreen screen);

        NoteElement CreateNoteElement(Guid noteId, IScreen? screen, NoteStartupPosition startupPosition);
        bool RemoveNoteElement(Guid noteId);
        NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind);
        SavingFontElement CreateFontElement(DefaultFontKind defaultFontKind, Guid fontId, ParentUpdater parentUpdater);

        StandardInputOutputElement CreateStandardInputOutputElement(string caption, Process process, IScreen screen);

        WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element);
        WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element);
        WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element);
        WindowItem CreateNoteWindow(NoteElement element);
        WindowItem CreateCommandWindow(CommandElement element);
        WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element);
        WindowItem CreateSettingWindow(SettingContainerElement element);

        #endregion
    }

    partial class ApplicationManager
    {
        class OrderManagerImpl : ManagerBase, IOrderManager
        {
            public OrderManagerImpl(IDiContainer diContainer, ILoggerFactory loggerFactory)
                : base(diContainer, loggerFactory)
            { }

            #region property

            ConcurrentDictionary<Guid, LauncherItemElement> LauncherItems { get; } = new ConcurrentDictionary<Guid, LauncherItemElement>();

            #endregion

            #region function
            #endregion

            #region IOrderManager

            public void StartUpdate(UpdateTarget target, UpdateProcess process)
            {
                throw new NotSupportedException();
            }

            public LauncherGroupElement CreateLauncherGroupElement(Guid launcherGroupId)
            {
                var element = DiContainer.Make<LauncherGroupElement>(new object[] { launcherGroupId });
                element.Initialize();
                return element;
            }

            public LauncherToolbarElement CreateLauncherToolbarElement(IScreen dockScreen, ReadOnlyObservableCollection<LauncherGroupElement> launcherGroups)
            {
                var element = DiContainer.Make<LauncherToolbarElement>(new object[] { dockScreen, launcherGroups });
                element.Initialize();
                return element;
            }

            public LauncherItemElement GetOrCreateLauncherItemElement(Guid launcherItemId)
            {
                return LauncherItems.GetOrAdd(launcherItemId, launcherItemIdKey => {

                    var launcherIconImageLoaders = EnumUtility.GetMembers<IconBox>()
                        .Select(i => DiContainer.Make<LauncherIconLoader>(new object[] { launcherItemId, i }))
                    ;
                    var iconImageLoaderPack = new IconImageLoaderPack(launcherIconImageLoaders);

                    var launcherIconElement = DiContainer.Make<LauncherIconElement>(new object[] { launcherItemId, iconImageLoaderPack });

                    var launcherItemElement = DiContainer.Make<LauncherItemElement>(new object[] { launcherItemIdKey, launcherIconElement });
                    launcherItemElement.Initialize();
                    return launcherItemElement;
                });
            }

            public LauncherItemCustomizeContainerElement CreateCustomizeLauncherItemContainerElement(Guid launcherItemId, IScreen screen, LauncherIconElement iconElement)
            {
                var customizeLauncherEditorElement = DiContainer.Build<LauncherItemCustomizeEditorElement>(launcherItemId);
                customizeLauncherEditorElement.Initialize();
                var customizeLauncherItemContainerElement = DiContainer.Build<LauncherItemCustomizeContainerElement>(screen, customizeLauncherEditorElement, iconElement);
                customizeLauncherItemContainerElement.Initialize();
                return customizeLauncherItemContainerElement;
            }

            public ExtendsExecuteElement CreateExtendsExecuteElement(string captionName, LauncherFileData launcherFileData, IReadOnlyList<LauncherEnvironmentVariableData> launcherEnvironmentVariables, IScreen screen)
            {
                var element = DiContainer.Build<ExtendsExecuteElement>(captionName, launcherFileData, launcherEnvironmentVariables, screen);
                element.Initialize();
                return element;
            }
            public LauncherExtendsExecuteElement CreateLauncherExtendsExecuteElement(Guid launcherItemId, IScreen screen)
            {
                var element = DiContainer.Build<LauncherExtendsExecuteElement>(launcherItemId, screen);
                element.Initialize();
                return element;
            }

            public NoteElement CreateNoteElement(Guid noteId, IScreen? screen, NoteStartupPosition startupPosition)
            {
                var element = screen == null
                    ? DiContainer.Build<NoteElement>(noteId, DiDefaultParameter.Create<IScreen>(), startupPosition)
                    : DiContainer.Build<NoteElement>(noteId, screen, startupPosition)
                ;
                element.Initialize();
                return element;
            }

            public bool RemoveNoteElement(Guid noteId)
            {
                throw new NotSupportedException($"{nameof(ApplicationManager)}.{nameof(RemoveNoteElement)}");
            }

            public NoteContentElement CreateNoteContentElement(Guid noteId, NoteContentKind contentKind)
            {
                var element = DiContainer.Build<NoteContentElement>(noteId, contentKind);
                element.Initialize();
                return element;
            }

            public SavingFontElement CreateFontElement(DefaultFontKind defaultFontKind, Guid fontId, ParentUpdater parentUpdater)
            {
                var element = DiContainer.Build<SavingFontElement>(defaultFontKind, fontId, parentUpdater);
                element.Initialize();
                return element;
            }

            public StandardInputOutputElement CreateStandardInputOutputElement(string id, Process process, IScreen screen)
            {
                var element = DiContainer.Build<StandardInputOutputElement>(id, process, screen);
                element.Initialize();
                return element;
            }


            public WindowItem CreateLauncherToolbarWindow(LauncherToolbarElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Make<LauncherToolbarViewModel>(new[] { element, });
                });
                var window = DiContainer.BuildView<LauncherToolbarWindow>();
                viewModel.AppDesktopToolbarExtend = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(viewModel);
                    return c.Make<AppDesktopToolbarExtend>(new object[] { window, element, });
                });
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherToolbar, element, window);
            }

            public WindowItem CreateCustomizeLauncherItemWindow(LauncherItemCustomizeContainerElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<LauncherItemCustomizeContainerViewModel>(element);
                });
                var window = DiContainer.BuildView<LauncherItemCustomizeWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.LauncherCustomize, element, window);
            }

            public WindowItem CreateExtendsExecuteWindow(ExtendsExecuteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<ExtendsExecuteViewModel>(element);
                });
                var window = DiContainer.BuildView<ExtendsExecuteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.ExtendsExecute, element, window);
            }

            public WindowItem CreateNoteWindow(NoteElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<NoteViewModel>(element);
                });
                var window = DiContainer.BuildView<NoteWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Note, element, window);
            }

            public WindowItem CreateCommandWindow(CommandElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<CommandViewModel>(element);
                });
                var window = DiContainer.BuildView<CommandWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Command, element, window);
            }


            public WindowItem CreateStandardInputOutputWindow(StandardInputOutputElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    //c.Register<ILoggerFactory, ILoggerFactory>(element);
                    return c.Build<StandardInputOutputViewModel>(element);
                });
                var window = DiContainer.BuildView<StandardInputOutputWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.StandardInputOutput, element, window);
            }

            public WindowItem CreateSettingWindow(SettingContainerElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<SettingContainerViewModel>(element);
                });
                var window = DiContainer.BuildView<SettingWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Setting, element, window);
            }

            public WindowItem CreateFeedbackWindow(FeedbackElement element)
            {
                var viewModel = DiContainer.UsingTemporaryContainer(c => {
                    return c.Build<FeedbackViewModel>(element);
                });
                var window = DiContainer.BuildView<FeedbackWindow>();
                window.DataContext = viewModel;

                return new WindowItem(WindowKind.Feedback, element, window);
            }

            #endregion

        }
    }
}
