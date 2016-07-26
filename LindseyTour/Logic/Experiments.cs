using LindseyTour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LindseyTour.Logic
{
   public static class Experiments
   {
      public static Experiment GetExperimentByUrl(HttpContext context, string Url)
      {
         var exp = new Experiment();
         exp.Code = "yourCode";
         var cookieName = string.Format("exp_{0}_variation", exp.Code);
         HttpCookie experimentCookie = context.Request.Cookies[cookieName];
         int variationIndex = 0;

         if (experimentCookie != null && experimentCookie.Values["exp_variation"] != null)
         {
            int.TryParse(experimentCookie.Values["exp_variation"], out variationIndex);
            exp.Variation = variationIndex;
         }else{
            var random = new Random();
            var random_spin = random.NextDouble();
            var cumulative_weights = 0.0;
            var variations = new List<Variation>();
            variations.Add(new Variation{ Index = 0, Weight = 0.5M});
            variations.Add(new Variation{ Index = 1, Weight = 0.5M});

            foreach (var variation in variations)
            {
               cumulative_weights += (Double)variation.Weight;

                if (random_spin < cumulative_weights)
                {
                   variationIndex = variation.Index;
                   SetVariation(context, exp.Code, variationIndex);
                   break;
                }
            }   
         }
         exp.Variation = variationIndex;
         return exp;
      }

      private static void SetVariation(HttpContext currentContext, string code, int variation, int durationDays = 30)
      {
         var cookieName = string.Format("exp_{0}_variation", code);
         HttpCookie experimentCookie = currentContext.Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
         experimentCookie.Values["exp_id"] = code;
         experimentCookie.Values["exp_variation"] = variation.ToString();
         experimentCookie.Expires = DateTime.Now.AddDays(durationDays);
         HttpContext.Current.Response.Cookies.Add(experimentCookie);
      }
   }
}