using Common.Models.Statistic;

namespace ViewModel.ViewModels
{
    public class ThemeInformationViewModel : BaseViewModel
    {
        public ThemeInformationViewModel(StatisticByTheme statistic)
        {
            selectedThemeStatistic = statistic;
        }

        private StatisticByTheme selectedThemeStatistic;
        public StatisticByTheme SelectedThemeStatistic
        {
            get
            {
                return selectedThemeStatistic;
            }
            set
            {
                selectedThemeStatistic = value;
                OnPropertyChanged("selectedThemetStatistic");
            }
        }

    }
}