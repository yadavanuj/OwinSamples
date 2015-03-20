using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;

namespace OwinSelfHost
{
    //AppFunc - takes IDictionary<string, object> as input and returns Task
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using System.IO;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var helloWorldMiddleware = new Func<AppFunc, AppFunc>(HelloWorldMiddleware);
            app.Use(helloWorldMiddleware);
        }

        private AppFunc HelloWorldMiddleware(AppFunc next)
        {
            AppFunc appFunc = async (IDictionary<string, object> environment) =>
            {
                var response = environment["owin.ResponseBody"] as Stream;

                using (var writer = new StreamWriter(response))
                {
                    await writer.WriteAsync("<h1>Hello World from Owin</h1>");
                }
                await next.Invoke(environment);
            };
            return appFunc;
        }
    }
}
