using System.Globalization;

namespace CashFlow.Api.Middleware
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var supporteedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();


            var cultureInfo = new CultureInfo("pt-BR");

            if (!string.IsNullOrEmpty(requestedCulture) && supporteedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
            {
                cultureInfo = new CultureInfo(requestedCulture);
            }
        
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}
