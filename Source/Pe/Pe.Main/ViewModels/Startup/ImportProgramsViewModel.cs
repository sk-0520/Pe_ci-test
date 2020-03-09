using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Core.Views;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Startup;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Startup
{
    public class ImportProgramsViewModel : ElementViewModelBase<ImportProgramsElement>
    {
        public ImportProgramsViewModel(ImportProgramsElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            ProgramCollection = new ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel>(Model.ProgramItems) {
                ToViewModel = m => new ProgramViewModel(m, UserTracker, DispatcherWrapper, LoggerFactory),
            };
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel> ProgramCollection { get; }
        public ReadOnlyObservableCollection<ProgramViewModel> ProgramItems => ProgramCollection.ViewModels;

        #endregion

        #region command

        public ICommand ViewLoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.LoadProgramsAsync().ConfigureAwait(false);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        ));

        public ICommand ImportCommand => GetOrCreateCommand(() => new DelegateCommand(
            async () => {
                var _ = UserTracker.TrackAsync(nameof(ImportCommand), new TrackProperties() {
                    ["TotalCount"] = Model.ProgramItems.Count.ToString(),
                    ["ImportCount"] = Model.ProgramItems.Count(i => i.IsImport).ToString(),
                });
                //TODO: 入力制限が必要
                await Model.ImportAsync();
                CloseRequest.Send();
            }
        ));

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var vm in ProgramCollection.ViewModels) {
                        vm.Dispose();
                    }
                    ProgramCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
