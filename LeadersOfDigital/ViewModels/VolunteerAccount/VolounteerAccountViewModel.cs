using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NoTryCatch.Core.Services;
using NoTryCatch.Xamarin.Portable.Services;
using NoTryCatch.Xamarin.Portable.ViewModels;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using LeadersOfDigital.ViewModels.Common;
using LeadersOfDigital.BusinessLayer;

namespace LeadersOfDigital.ViewModels.VolunteerAccount
{
    public class VolounteerAccountViewModel : PageViewModel
    {
        private readonly ExtendedUserContext _extendedUserContext;
        private readonly ICommand _menuItemTapCommand;

        private const string MAIN_MENU_ITEM = "Главное";
        private const string MARKET_MENU_ITEM = "Маркет";
        private const string MADE_HELP_MENU_ITEM = "Оказанная помощь";
        private const string SETTINGS_MENU_ITEM = "Настройки";

        private ObservableCollection<VolounteerHelpItem> _needHelpItemsCollection;
        private ObservableCollection<VolounteerHelpItem> _madeHelpItemsCollectiong;

        public VolounteerAccountViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IDebuggerService debuggerService,
            IExceptionHandler exceptionHandler,
            ExtendedUserContext extendedUserContext)
            : base(navigationService, dialogService, debuggerService, exceptionHandler)
        {
            _extendedUserContext = extendedUserContext;

            _menuItemTapCommand = BuildPageVmCommand<FloatingMenuItem>(
                item =>
                {
                    if (item.IsActive)
                    {
                        return Task.CompletedTask;
                    }

                    if (MenuItemsCollection.FirstOrDefault(x => x.IsActive) is FloatingMenuItem activeItem)
                    {
                        activeItem.IsActive = false;
                    }

                    item.IsActive = true;

                    OnPropertyChanged(nameof(IsMainVisible));
                    OnPropertyChanged(nameof(IsMarketVisible));
                    OnPropertyChanged(nameof(IsMadeHelpVisible));
                    OnPropertyChanged(nameof(IsSettingsVisible));

                    return Task.CompletedTask;
                });

            MenuItemsCollection = new List<FloatingMenuItem>
            {
                new FloatingMenuItem(MAIN_MENU_ITEM)
                {
                    IsActive = true,
                    TapCommand = _menuItemTapCommand,
                },
                new FloatingMenuItem(MARKET_MENU_ITEM)
                {
                    TapCommand = _menuItemTapCommand,
                },
                new FloatingMenuItem(MADE_HELP_MENU_ITEM)
                {
                    TapCommand = _menuItemTapCommand,
                },
                //new FloatingMenuItem(SETTINGS_MENU_ITEM)
                //{
                //    TapCommand = _menuItemTapCommand,
                //},
            };

            NeedHelpItemsCollection = new ObservableCollection<VolounteerHelpItem>
            {
                new VolounteerHelpItem
                {
                    FullName = "Иваненко Раиса",
                    Description = "Инвалид второй группы, колясочник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 77с1",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 24",
                    PlannedAt = DateTime.Now.AddDays(4),
                },
                new VolounteerHelpItem
                {
                    FullName = "Самуленков Игорь",
                    Description = "Инвалид первой группы, опорник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 33",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 50",
                    PlannedAt = DateTime.Now.AddDays(6),
                    IsLast = true,
                },
            };

            MarketItemsCollection = new List<Tuple<ImageSource, ImageSource>>
            {
                new Tuple<ImageSource, ImageSource>(AppImages.ImageMarket1, AppImages.ImageMarket2),
                new Tuple<ImageSource, ImageSource>(AppImages.ImageMarket3, AppImages.ImageMarket4),
                new Tuple<ImageSource, ImageSource>(AppImages.ImageMarket5, AppImages.ImageMarket6),
            };

            MadeHelpItemsCollection = new ObservableCollection<VolounteerHelpItem>
            {
                new VolounteerHelpItem
                {
                    FullName = "Иваненко Раиса",
                    Description = "Инвалид второй группы, колясочник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 77с1",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 24",
                    PlannedAt = DateTime.Now.AddDays(4),
                },
                new VolounteerHelpItem
                {
                    FullName = "Самуленков Игорь",
                    Description = "Инвалид первой группы, опорник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 33",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 50",
                    PlannedAt = DateTime.Now.AddDays(6),
                },
                new VolounteerHelpItem
                {
                    FullName = "Иваненко Раиса",
                    Description = "Инвалид второй группы, колясочник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 77с1",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 24",
                    PlannedAt = DateTime.Now.AddDays(4),
                },
                new VolounteerHelpItem
                {
                    FullName = "Самуленков Игорь",
                    Description = "Инвалид первой группы, опорник",
                    FromAddress = "Москва, Новогиреево, Зеленый просп., 33",
                    ToAddress = "Москва, Новогиреево, Свободный просп., 50",
                    PlannedAt = DateTime.Now.AddDays(6),
                    IsLast = true,
                },
            };

            FullName = $"{_extendedUserContext.FirstName} {_extendedUserContext.SecondName}";
        }

        public string FullName { get; }

        public IEnumerable<FloatingMenuItem> MenuItemsCollection { get; }

        public bool IsMainVisible => MenuItemsCollection.FirstOrDefault(x => x.Title == MAIN_MENU_ITEM)?.IsActive == true;

        public bool IsMarketVisible => MenuItemsCollection.FirstOrDefault(x => x.Title == MARKET_MENU_ITEM)?.IsActive == true;

        public bool IsMadeHelpVisible => MenuItemsCollection.FirstOrDefault(x => x.Title == MADE_HELP_MENU_ITEM)?.IsActive == true;

        public bool IsSettingsVisible => MenuItemsCollection.FirstOrDefault(x => x.Title == SETTINGS_MENU_ITEM)?.IsActive == true;

        public ObservableCollection<VolounteerHelpItem> NeedHelpItemsCollection
        {
            get => _needHelpItemsCollection;
            set => SetProperty(ref _needHelpItemsCollection, value);
        }

        public IEnumerable<Tuple<ImageSource, ImageSource>> MarketItemsCollection { get; }

        public ObservableCollection<VolounteerHelpItem> MadeHelpItemsCollection
        {
            get => _madeHelpItemsCollectiong;
            set => SetProperty(ref _madeHelpItemsCollectiong, value);
        }
    }
}
