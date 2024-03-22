using DoiFApp.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace DoiFApp.Services
{
    public class NotifyBuilder : IBuilder<NotifyViewModel>
    {
        private NotifyViewModel viewModel = new();

        public NotifyBuilder WithTitle(string title)
        {
            viewModel.Title = title;
            return this;
        }

        public NotifyBuilder WithDescription(string desc)
        {
            viewModel.Description = desc;
            return this;
        }

        public NotifyBuilder WithColor(SolidColorBrush colorBrush)
        {
            viewModel.Color = colorBrush;
            return this;
        }

        public NotifyBuilder WithColor(Color color)
            => WithColor(new SolidColorBrush(color));

        public NotifyBuilder WithColor(NotifyColorType color)
            => WithColor(color switch
            {
                NotifyColorType.Error => new Color { R = 50, G = 5, B = 5 },
                NotifyColorType.Warning => new Color { R = 50, G = 5, B = 50 },
                NotifyColorType.Info => new Color { R = 5, G = 5, B = 50 },
                _ => new Color { R = 50, G = 50, B = 50 },
            });

        public NotifyBuilder WithRemove(Action action)
        {
            viewModel.OnRemove += (vm) => action();
            return this;
        }

        public NotifyBuilder WithRemove(Action<NotifyViewModel> action)
        {
            viewModel.OnRemove += (vm) => action(vm);
            return this;
        }


        public NotifyBuilder WithRemove(Func<NotifyViewModel, bool> action)
        {
            viewModel.OnRemove += (vm) => action(vm);
            return this;
        }


        public NotifyViewModel Build()
        {
            (var vm, viewModel) = (viewModel, new NotifyViewModel());
            return vm;
        }
    }

    public enum NotifyColorType
    {
        None,
        Info,
        Error,
        Warning,
    }
}
