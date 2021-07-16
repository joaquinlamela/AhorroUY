using BusinessLogic.Interface;
using BusinessLogicForPushNotification;
using DataAccessInterface;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        private static string[] deviceTokens;

        private static IHost hostToUse;

        private static string body;

        public static void Main(string[] args)
        {
            hostToUse = CreateHostBuilder(args).Build();
            
            Thread runPrincipalProgramThread = new Thread(hostToUse.Run);
            Thread sendPushNotificationThread = new Thread(() => SendPushNotification(hostToUse));
            runPrincipalProgramThread.Start();
            sendPushNotificationThread.Start();
        }

        private static void SendPushNotification(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var repositoryToken = services.GetRequiredService<IRepository<Token>>();
                    PushNotificationManagement pushNotificationManagement = new PushNotificationManagement(repositoryToken);


                    Timer timerForDeviceTokens = new Timer(TimeSpan.FromMinutes(2.5).TotalMilliseconds);
                    timerForDeviceTokens.AutoReset = true;
                    timerForDeviceTokens.Elapsed += new ElapsedEventHandler(UpdateDeviceTokens);
                    timerForDeviceTokens.Elapsed += new ElapsedEventHandler(GetRandomProduct);
                    timerForDeviceTokens.Start();


                    Timer timer = new Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
                    timer.AutoReset = true;
                    timer.Elapsed += (sender, e) =>
                     pushNotificationManagement.SendPushNotification(deviceTokens, body);
                    timer.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hubo un error al intentar enviar las notificaciones" + ex.Message);
                }
            }
        }

        private static void UpdateDeviceTokens(Object source, System.Timers.ElapsedEventArgs e)
        {
            using (var serviceScope = hostToUse.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    var repositoryToken = services.GetRequiredService<IRepository<Token>>();
                    deviceTokens = repositoryToken.GetAll().ToList().ConvertAll(token => token.TokenValue).ToArray();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Hubo un error al intentar enviar las notificaciones" + exception.Message);
                }
            }
        }

        private static void GetRandomProduct(Object source, System.Timers.ElapsedEventArgs e)
        {
            using (var serviceScope = hostToUse.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                try
                {
                    IProductMarketManagemenet productMarketManagement = services.GetRequiredService<IProductMarketManagemenet>();
                    List<ProductMarket> productsWithDiscount = productMarketManagement.GetProductsWithDiscounts();
                    var random = new Random();
                    int index = random.Next(productsWithDiscount.Count);
                    ProductMarket productMarket = productsWithDiscount.ElementAt(index);
                    body = "Tenemos a: " + productMarket.Product.Name + " a $" + productMarket.CurrentPrice;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Hubo un error al intentar enviar las notificaciones" + exception.Message);
                }
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
