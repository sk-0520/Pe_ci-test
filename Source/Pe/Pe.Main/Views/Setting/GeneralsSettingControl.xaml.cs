using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.ViewModels.Setting;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// GeneralsSettingControl.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneralsSettingControl: UserControl
    {
        public GeneralsSettingControl()
        {
            InitializeComponent();
            //DialogRequestReceiver = new DialogRequestReceiver(this);
            CommandStore = new ElementCommandStore(this);
        }

        #region Editor

        public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
            nameof(Editor),
            typeof(GeneralsSettingEditorViewModel),
            typeof(GeneralsSettingControl),
            new FrameworkPropertyMetadata(
                default(GeneralsSettingEditorViewModel),
                new PropertyChangedCallback(OnEditorChanged)
            )
        );

        public GeneralsSettingEditorViewModel Editor
        {
            get { return (GeneralsSettingEditorViewModel)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        private static void OnEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is GeneralsSettingControl control) {
            }
        }

        #endregion

        #region property

        //DialogRequestReceiver DialogRequestReceiver { get; }
        private ElementCommandStore CommandStore { get; }

        #endregion

        #region command

        //ICommand? _fileSelectCommand;
        public ICommand FileSelectCommand => CommandStore.GetOrCreate(() => new DelegateCommand<RequestEventArgs>(
                    o => {
                        var DialogRequestReceiver = new DialogRequestReceiver(this);

                        DialogRequestReceiver.ReceiveFileSystemSelectDialogRequest(o);
                    }
        ));

        #endregion
    }
}
