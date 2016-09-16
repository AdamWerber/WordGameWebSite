using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Sdp_Net_proj.Services;
using Sdp_Net_proj.Models;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace Sdp_Net_proj
{
    public class Startup
    {

        private IHostingEnvironment _env;
        private IConfigurationRoot _config;
        private TaskQueue queue = new TaskQueue();
        private List<int> users = new List<int>();
        private int count = 0;

        //private IRipository

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json"); // a way to add environment variables
                                             //.AddEnvironmentVariables(); // allow you to override environment variables.

        _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development"))
            {
                services.AddScoped<IMailService,MailService>();
            }
            else
            {
                services.AddScoped<IMailService, MailService>();// Implement a real Mail Service - maybe will need changes.
            }

            services.AddDbContext<WorldContext>();

            services.AddLogging();
          

            services.AddMvc().
                AddJsonOptions(config =>
                {
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }

                    );


      //      services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
     //       services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
             ILoggerFactory factory)
        {

           

            if (env.IsDevelopment()) // display error data when in development.
            {
                app.UseDeveloperExceptionPage();
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                factory.AddDebug(LogLevel.Error);
            }

            app.UseStaticFiles();

            app.UseWebSockets();
         

           

            app.Use(async (http, next) =>
            {
                

                if (http.WebSockets.IsWebSocketRequest)
                {

                    users.Add(count++);
                    string username = users[users.Count - 1].ToString();
                    //Handle WebSocket Requests here.    
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();

                        var token = CancellationToken.None;
                        var buffer = new ArraySegment<Byte>(new Byte[4096]);
                        var type = WebSocketMessageType.Text;

                        var data = Encoding.UTF8.GetBytes("message from server : hello user :" + users[users.Count-1]);
                        buffer = new ArraySegment<Byte>(data);

                        await  webSocket.SendAsync(buffer, type, true, token);



                        while (webSocket.State == WebSocketState.Open)
                        {
                            /*
                            var token = CancellationToken.None;
                            var buffer = new ArraySegment<Byte>(new Byte[4096]);
                            var received = await webSocket.ReceiveAsync(buffer, token);

                            switch (received.MessageType)
                            {
                                case WebSocketMessageType.Text:
                                    var request = Encoding.UTF8.GetString(buffer.Array,
                                                            buffer.Offset,
                                                            buffer.Count);
                                    var type = WebSocketMessageType.Text;
                                    var data = Encoding.UTF8.GetBytes("message from server : the word is :" + request);
                                    buffer = new ArraySegment<Byte>(data);
                                    await webSocket.SendAsync(buffer, type, true, token);

                                    
                                    break;
                            }

       */

                            await Echo(webSocket);
                          
                        }
                    }

                  
                }
                else
                {
                    await next();
                }
                
            });
        
            //     app.UseSession();

            app.UseMvc(config =>
            {
                config.MapRoute( // this method take a pattern of a URL and map it to a method.
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { Controller = "App", action = "index" }
                    );
            });
        }


        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }

  

}
