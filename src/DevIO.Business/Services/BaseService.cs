using FluentValidation.Results;
using FluentValidation;
using DevIO.Business.Models;
using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        public BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }


        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool ExecuteValidation<TV, TE>(TV validation, TE entitiy) where TV: AbstractValidator<TE> where TE : Entity
        {
            var validator = validation.Validate(entitiy);

            if (validator.IsValid) return true;
            
            Notify(validator);
            return false;
        } 
    }
}
