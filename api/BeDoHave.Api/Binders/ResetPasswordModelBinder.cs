using BeDoHave.Shared.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeDoHave.Api.Binders
{
    public class ResetPasswordModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var model = await JsonSerializer.DeserializeAsync<ResetPasswordDto>(bindingContext.HttpContext.Request.Body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            model.Token = bindingContext
                .HttpContext
                .Request
                .Query["token"];

            bindingContext.Result = ModelBindingResult.Success(model);
        }
    }
}
