using PrintHub.WPF.Endpoints.SurveysEndpoints.ShowAll;
using PrintHub.WPF.Shared.ViewModels;

namespace PrintHub.WPF.Pages.Survey;

public class SurveyViewModel(
    SurveyShowAllFormViewModel showAllFormViewModel)
    : ViewModelBase
{
    public SurveyShowAllFormViewModel SurveyShowAllFormViewModel { get; } = showAllFormViewModel;
}