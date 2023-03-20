using DataAccess;
using DataAccess.Repositories;

namespace RTK_HMI.ViewModels
{
    internal class UserVm:PropertyChangedBase
    {
        private readonly IRepository<>
        internal UserVm(MainViewModel mainView)
        {
            MainView = mainView;
        }

        public MainViewModel MainView { get; }
    }
}
