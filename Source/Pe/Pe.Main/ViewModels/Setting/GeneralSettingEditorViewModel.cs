using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public interface IGeneralSettingEditor
    {
        #region property

        string Header { get; }
        bool IsInitialized { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion
    }

    public abstract class GeneralSettingEditorViewModelBase<TModel> : SettingItemViewModelBase<TModel>, IGeneralSettingEditor
        where TModel : GeneralSettingEditorElementBase
    {
        public GeneralSettingEditorViewModelBase(TModel model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region SingleModelViewModelBase
        #endregion

        #region IGeneralSettingEditor

        public abstract string Header { get; }

        #endregion

    }

    public sealed class AppExecuteSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppExecuteSettingEditorElement>
    {
        public AppExecuteSettingEditorViewModel(AppExecuteSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string UserId
        {
            get => Model.UserId;
            private set => SetModelValue(value);
        }
        public bool SendUsageStatistics
        {
            get => Model.SendUsageStatistics;
            set => SetModelValue(value);
        }

        #endregion

        #region command

        public ICommand CreateUserIdFromRandomCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var userIdManager = new UserIdManager(LoggerFactory);
                UserId = userIdManager.CreateFromRandom();
            }
        ));

        public ICommand CreateUserIdFromEnvironmentCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var userIdManager = new UserIdManager(LoggerFactory);
                UserId = userIdManager.CreateFromEnvironment();
            }
        ));

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Execute;

        #endregion
    }


    public sealed class AppGeneralSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppGeneralSettingEditorElement>
    {
        public AppGeneralSettingEditorViewModel(AppGeneralSettingEditorElement model, IReadOnlyCollection<string> cultureNames, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            CultureInfoItems = new ObservableCollection<CultureInfo>();
            var cultures = cultureNames.Select(i => CultureInfo.GetCultureInfo(i));
            CultureInfoItems.Add(CultureInfo.InvariantCulture);
            CultureInfoItems.AddRange(cultures);
        }

        #region property

        public RequestSender UserBackupDirectorySelecteRequest { get; } = new RequestSender();


        public ObservableCollection<CultureInfo> CultureInfoItems { get; }
        public CultureInfo SelectedCultureInfo
        {
            get => Model.CultureInfo;
            set => SetModelValue(value, nameof(Model.CultureInfo));
        }

        public string UserBackupDirectoryPath
        {
            get => Model.UserBackupDirectoryPath;
            set => SetModelValue(value);
        }

        public bool IsRegisterStartup
        {
            get => Model.IsRegisterStartup;
            set => SetModelValue(value);
        }

        public bool DelayStartup
        {
            get => Model.DelayStartup;
            set => SetModelValue(value);
        }

        public double MinimumStartupWaitTimeSeconds => TimeSpan.FromSeconds(1).TotalSeconds;
        public double MaximumStartupWaitTimeSeconds => TimeSpan.FromMinutes(1).TotalSeconds;
        public double StartupWaitTimeSeconds
        {
            get => Model.StartupWaitTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.StartupWaitTime));
        }

        #endregion

        #region command

        public ICommand UserBackupDirectorySelectCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var dialogRequester = new DialogRequester(LoggerFactory);
                dialogRequester.SelectDirectory(
                    UserBackupDirectorySelecteRequest,
                    dialogRequester.ExpandPath(UserBackupDirectoryPath),
                    r => {
                        UserBackupDirectoryPath = r.ResponseFilePaths[0];
                    }
                );
            }
        ));

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_General;

        #endregion
    }

    public sealed class AppUpdateSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppUpdateSettingEditorElement>
    {
        public AppUpdateSettingEditorViewModel(AppUpdateSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool IsCheckReleaseVersion
        {
            get => Model.IsCheckReleaseVersion;
            set => SetModelValue(value);
        }
        public bool IsCheckRcVersion
        {
            get => Model.IsCheckRcVersion;
            set => SetModelValue(value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Update;

        #endregion
    }


    public sealed class AppCommandSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppCommandSettingEditorElement>
    {
        public AppCommandSettingEditorViewModel(AppCommandSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
        }

        #region property

        IGeneralTheme GeneralTheme { get; }
        public Geometry BoldIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontBold, Bridge.Models.Data.IconBox.Small);
        public Geometry ItalicIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontItalic, Bridge.Models.Data.IconBox.Small);

        public FontViewModel? Font { get; private set; }
        public IconBox IconBox
        {
            get => Model.IconBox;
            set => SetModelValue(value);
        }

        //public TimeSpan HideWaitTime
        //{
        //    get => Model.HideWaitTime;
        //    set => SetModelValue(value);
        //}

        public double MinimumHideWaitSeconds => TimeSpan.FromMilliseconds(250).TotalSeconds;
        public double MaximumHideWaitSeconds => TimeSpan.FromSeconds(5).TotalSeconds;
        public double HideWaitMilliseconds
        {
            get => Model.HideWaitTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.HideWaitTime));
        }

        public bool FindTag
        {
            get => Model.FindTag;
            set => SetModelValue(value);
        }
        public bool FindFile
        {
            get => Model.FindFile;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Command;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }


        #endregion
    }


    public sealed class AppNoteSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppNoteSettingEditorElement>
    {
        public AppNoteSettingEditorViewModel(AppNoteSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
        }

        #region property

        IGeneralTheme GeneralTheme { get; }
        public Geometry BoldIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontBold, Bridge.Models.Data.IconBox.Small);
        public Geometry ItalicIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontItalic, Bridge.Models.Data.IconBox.Small);

        public FontViewModel? Font { get; private set; }
        public NoteCreateTitleKind TitleKind
        {
            get => Model.TitleKind;
            set => SetModelValue(value);
        }
        public NoteLayoutKind LayoutKind
        {
            get => Model.LayoutKind;
            set => SetModelValue(value);
        }

        public Color ForegroundColor
        {
            get => Model.ForegroundColor;
            set => SetModelValue(value);
        }

        public Color BackgroundColor
        {
            get => Model.BackgroundColor;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
            set => SetModelValue(value);
        }


        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Note;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        #endregion
    }


    public sealed class AppStandardInputOutputSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppStandardInputOutputSettingEditorElement>
    {
        public AppStandardInputOutputSettingEditorViewModel(AppStandardInputOutputSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;
        }

        #region property
        IGeneralTheme GeneralTheme { get; }
        public Geometry BoldIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontBold, Bridge.Models.Data.IconBox.Small);
        public Geometry ItalicIcon => GeneralTheme.GetGeometryImage(GeneralGeometryImageKind.FontItalic, Bridge.Models.Data.IconBox.Small);

        public FontViewModel? Font { get; private set; }
        public Color OutputForegroundColor
        {
            get => Model.OutputForegroundColor;
            set => SetModelValue(value);
        }
        public Color OutputBackgroundColor
        {
            get => Model.OutputBackgroundColor;
            set => SetModelValue(value);
        }
        public Color ErrorForegroundColor
        {
            get => Model.ErrorForegroundColor;
            set => SetModelValue(value);
        }
        public Color ErrorBackgroundColor
        {
            get => Model.ErrorBackgroundColor;
            set => SetModelValue(value);
        }
        public bool IsTopmost
        {
            get => Model.IsTopmost;
            set => SetModelValue(value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_StandardInputOutput;

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        #endregion
    }

    public sealed class AppWindowSettingEditorViewModel : GeneralSettingEditorViewModelBase<AppWindowSettingEditorElement>
    {
        public AppWindowSettingEditorViewModel(AppWindowSettingEditorElement model, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public bool IsEnabled
        {
            get => Model.IsEnabled;
            set => SetModelValue(value);
        }
        public int Count
        {
            get => Model.Count;
            set => SetModelValue(value);
        }
        public TimeSpan Interval
        {
            get => Model.Interval;
            set => SetModelValue(value);
        }

        public int CountMaximum => 100;
        public int CountMinimum => 3;

        public double IntervalMinutes
        {
            get => Interval.TotalMinutes;
            set
            {
                Interval = TimeSpan.FromMinutes(value);
                RaisePropertyChanged();
            }
        }
        public double IntervalMaximum => TimeSpan.FromMinutes(30).TotalMinutes;
        public double IntervalMinimum => TimeSpan.FromMinutes(1).TotalMinutes;

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_General_Header_Window;

        #endregion
    }

}
