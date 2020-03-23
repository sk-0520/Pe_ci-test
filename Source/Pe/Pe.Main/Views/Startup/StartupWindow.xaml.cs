using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Views.Startup
{
    /// <summary>
    /// StartupWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        #region property

        [Injection]
        ILogger? Logger { get; set; }
        CommandStore CommandStore { get; } = new CommandStore();

        #endregion

        #region command
        public ICommand CloseCommand => CommandStore.GetOrCreate(() => new DelegateCommand(
            () => Close()
        ));
        #endregion

        private void root_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsVisibleChanged -= root_IsVisibleChanged;

            Dispatcher.BeginInvoke(new Action(() => {
                Activate();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
    }
}
