using AEMAssessment.Data;
using AEMAssessment.Models;
using AEMWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AEMAssessment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static IConfiguration _configuration;
        public string bearerTokenFilter;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<IActionResult> Index()
        {

            try
            {
                //-----start post request Login-----

                using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                {
                    var login = new Login(_configuration.GetValue<string>("LoginUserName"),
                                          _configuration.GetValue<string>("LoginPassword"));

                    var json = JsonConvert.SerializeObject(login);
                    string postResponse = await myHttpClient.PostDataAsync("/api/Account/Login", json);

                    bearerTokenFilter = postResponse.Trim(new Char[] { '/', '"' });
                    Console.WriteLine(bearerTokenFilter);

                }

                //-----end post request Login-----


                //-----get request GetPlatformWellActual-----

                using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                {
                    string getResponse = await myHttpClient.GetResponseAsync("/api/PlatformWell/GetPlatformWellActual", bearerTokenFilter);

                    if (getResponse != null)
                    {
                        List<PlatformWellActual> platformWellActualList = JsonConvert.DeserializeObject<List<PlatformWellActual>>(getResponse);

                        //Create
                        using (var dbContext = new PlatformWellActualContext(_configuration))
                        {
                            for (int i = 0; i < platformWellActualList.Count; i++)
                            {
                                dbContext.PlatformWellActuals.Add(platformWellActualList[i]);

                                try
                                {
                                    Console.WriteLine("User inserted successfully.");
                                    dbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred: {ex.Message}");
                                }
                            }
                        }

                    }
                }

                //-----end get request GetPlatformWellActual-----

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

            return View();
        }


        public async Task<IActionResult> Privacy()
        {

            try
            {
                //-----start post request Login-----

                using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                {
                    var login = new Login(_configuration.GetValue<string>("LoginUserName"),
                                          _configuration.GetValue<string>("LoginPassword"));

                    var json = JsonConvert.SerializeObject(login);
                    string postResponse = await myHttpClient.PostDataAsync("/api/Account/Login", json);

                    bearerTokenFilter = postResponse.Trim(new Char[] { '/', '"' });
                    Console.WriteLine(bearerTokenFilter);

                }

                //-----end post request Login-----


                //-----get request GetPlatformWellDummy-----

                using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                {
                    string getResponse = await myHttpClient.GetResponseAsync("/api/PlatformWell/GetPlatformWellDummy", bearerTokenFilter);

                    if (getResponse != null)
                    {
                        List<PlatformWellActual> platformWellActualList = JsonConvert.DeserializeObject<List<PlatformWellActual>>(getResponse);

                        //Create
                        using (var dbContext = new PlatformWellActualContext(_configuration))
                        {
                            for (int i = 0; i < platformWellActualList.Count; i++)
                            {
                                dbContext.PlatformWellActuals.Add(platformWellActualList[i]);

                                try
                                {
                                    Console.WriteLine("User update successfully.");
                                    dbContext.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred: {ex.Message}");
                                }
                            }
                        }

                    }
                }

                //-----end get request GetPlatformWellDummy-----

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}