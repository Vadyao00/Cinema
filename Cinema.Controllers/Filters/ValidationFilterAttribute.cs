using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Cinema.Domain.DataTransferObjects;

namespace Cinema.Controllers.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
            var param = context.ActionArguments
            .SingleOrDefault(x => x.Value!.ToString()!.Contains("Dto")).Value;
            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"Object is null. Controller: {controller}, action: {action}");
                return;
            }
            if (param is EventForManipulationDto eevent)
            {
                if (eevent.StartTime >= eevent.EndTime)
                    context.Result = new BadRequestObjectResult($"Object has incorrect parameters. StartTime must be less than EndTime. Controller: {controller}, action: {action}");
            }
            if (param is ShowtimeForManipulationDto showtime)
            {
                if (showtime.StartTime >= showtime.EndTime)
                    context.Result = new BadRequestObjectResult($"Object has incorrect parameters. StartTime must be less than EndTime. Controller: {controller}, action: {action}");
            }
            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
}