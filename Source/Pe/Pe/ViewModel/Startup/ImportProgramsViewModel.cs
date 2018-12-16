using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Startup;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Startup
{
    public class ImportProgramsViewModel : SingleModelViewModelBase<ImportProgramsElement>
    {
        public ImportProgramsViewModel(ImportProgramsElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            ProgramCollection = new ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel>(Model.ProgramItems, Logger) {
                ToViewModel = m => new ProgramViewModel(m, Logger.Factory),
            };
        }

        #region property

        public InteractionRequest<Notification> CloseRequest { get; } = new InteractionRequest<Notification>();

        ActionModelViewModelObservableCollectionManager<ProgramElement, ProgramViewModel> ProgramCollection { get; }
        public ObservableCollection<ProgramViewModel> ProgramItems => ProgramCollection.ViewModels;



        #endregion

        #region command

        public ICommand ViewLoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.LoadProgramsAsync().ConfigureAwait(false);
            }
        ));

        public ICommand CloseCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Raise(new Notification());
            }
        ));

        public ICommand ImportCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.ImportAsync().ConfigureAwait(false);
            }
        ));

        #endregion

    }
}
