using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace Trips.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
    {
        protected BaseViewModel()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}
