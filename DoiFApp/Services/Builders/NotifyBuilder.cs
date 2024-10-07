using DoiFApp.ViewModels;
using System.Windows.Media;

namespace DoiFApp.Services.Builders
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

        public NotifyBuilder WithColor(Color color)
        {
            viewModel.Color = color;
            return this;
        }

        public NotifyBuilder WithColor(NotifyColorType color)
            => WithColor(color switch
            {
                NotifyColorType.Error => new Color { A = 255, R = 155, G = 5, B = 5 },
                NotifyColorType.Warning => new Color { A = 255, R = 155, G = 5, B = 155 },
                NotifyColorType.Info => new Color { A = 255, R = 5, G = 5, B = 155 },
                _ => new Color { A = 255, R = 155, G = 155, B = 155 },
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
