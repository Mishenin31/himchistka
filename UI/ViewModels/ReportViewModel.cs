using System.Collections.ObjectModel;
using System.Windows.Input;
using Himchistka.Services;
using Himchistka.Services.Dto;

namespace Himchistka.UI.ViewModels;

public class ReportViewModel : ViewModelBase
{
    private readonly ReportService _reportService;
    private DateTime _dateFrom = DateTime.Today.AddDays(-30);
    private DateTime _dateTo = DateTime.Today;
    private DailyReport? _dailyReport;
    private MonthlyReport? _monthlyReport;

    public DateTime DateFrom { get => _dateFrom; set => Set(ref _dateFrom, value); }
    public DateTime DateTo { get => _dateTo; set => Set(ref _dateTo, value); }

    public DailyReport? DailyReport { get => _dailyReport; set => Set(ref _dailyReport, value); }
    public MonthlyReport? MonthlyReport { get => _monthlyReport; set => Set(ref _monthlyReport, value); }

    public ObservableCollection<PopularServiceReport> PopularServices { get; } = new();
    public ObservableCollection<ClientReport> Clients { get; } = new();

    public ICommand LoadReportsCommand { get; }
    public ICommand ExportExcelCommand { get; }
    public ICommand ExportPdfCommand { get; }

    public ReportViewModel(ReportService reportService)
    {
        _reportService = reportService;
        LoadReportsCommand = new RelayCommand(async _ => await Load());
        ExportExcelCommand = new RelayCommand(_ => { /* TODO */ });
        ExportPdfCommand = new RelayCommand(_ => { /* TODO */ });
    }

    private async Task Load()
    {
        DailyReport = await _reportService.GenerateDailyReport();
        MonthlyReport = await _reportService.GenerateMonthlyReport();

        PopularServices.Clear();
        foreach (var item in await _reportService.GeneratePopularServicesReport())
            PopularServices.Add(item);

        Clients.Clear();
        foreach (var item in await _reportService.GenerateClientReport())
            Clients.Add(item);
    }
}
