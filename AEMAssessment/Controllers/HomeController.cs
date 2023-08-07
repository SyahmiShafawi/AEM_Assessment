using AEMAssessment.Data;
using AEMAssessment.Models;
using AEMWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using static AEMWebApplication.Models.PlatformWellActual;
using Microsoft.EntityFrameworkCore;

namespace AEMAssessment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemoryCache _memoryCache;

        public static IConfiguration _configuration;
        public string bearerTokenFilter;



        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _logger = logger;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }


        public async Task<IActionResult> Index()
        {

            bearerTokenFilter = GetDataFromCache("BearerTokenFilter");
            if (bearerTokenFilter == null)
            {
                //-----Start post request Login-----

                using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                {
                    var login = new Login(_configuration.GetValue<string>("LoginUserName"),
                                          _configuration.GetValue<string>("LoginPassword"));

                    var json = JsonConvert.SerializeObject(login);
                    string postResponse = await myHttpClient.PostDataAsync("/api/Account/Login", json);

                    bearerTokenFilter = postResponse.Trim(new Char[] { '/', '"' });
                    Console.WriteLine(bearerTokenFilter);

                    SetDataInCache("BearerTokenFilter", bearerTokenFilter);

                }

                //-----End post request Login-----
            }


            //-----Get request GetPlatformWellActual-----

            using (var myHttpClient = new MyHttpClientWrapper(_configuration))
            {
                var getResponse = await myHttpClient.GetResponseAsync("/api/PlatformWell/GetPlatformWellActual", bearerTokenFilter);

                if (getResponse != null)
                {
                    List<PlatformWellActual> platformWellActualList = JsonConvert.DeserializeObject<List<PlatformWellActual>>(getResponse);

                    using (var dbContext = new PlatformWellActualContext(_configuration))
                    {
                        for (int i = 0; i < platformWellActualList.Count; i++)
                        {
                            PlatformWellActual platformWellActuallToUpdate = dbContext.PlatformWellActual
                                .Include(u => u.well).Where(u => u.Id == platformWellActualList[i].Id).FirstOrDefault();                    

                            if (platformWellActuallToUpdate != null) 
                            {
                                platformWellActuallToUpdate.Longitude = platformWellActualList[i].Longitude;
                                platformWellActuallToUpdate.Latitude = platformWellActualList[i].Latitude;
                                platformWellActuallToUpdate.UniqueName = platformWellActualList[i].UniqueName;
                                platformWellActuallToUpdate.CreatedAt = platformWellActualList[i].CreatedAt;
                                platformWellActuallToUpdate.UpdatedAt = platformWellActualList[i].UpdatedAt;

                                for (int x = 0; x < platformWellActualList[i].well.Count; x++)
                                {
                                    Well well = dbContext.Well.Where(y => y.Id == platformWellActualList[i].well[x].Id).FirstOrDefault();

                                    if (well != null)
                                    {
                                        well.Longitude = platformWellActualList[i].well[x].Longitude;
                                        well.Latitude = platformWellActualList[i].well[x].Latitude;
                                        well.UniqueName = platformWellActualList[i].well[x].UniqueName; // + "(" + i + "," + x + ")"
                                        well.CreatedAt = platformWellActualList[i].well[x].CreatedAt;
                                        well.UpdatedAt = platformWellActualList[i].well[x].UpdatedAt;
                                        well.PlatformId = platformWellActualList[i].well[x].PlatformId;
                                    }
                                    else 
                                    {
                                        platformWellActuallToUpdate.well.Add(platformWellActualList[i].well[x]);
                                    }

                                }

                            }
                            else 
                            {
                                dbContext.PlatformWellActual.Add(platformWellActualList[i]);
                            }

                            try
                            {
                                dbContext.SaveChanges();
                                Console.WriteLine("User inserted successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                    }

                }
            }
            //-----End request GetPlatformWellActual-----


            return View();
        }


        public async Task<IActionResult> Privacy()
        {

            bearerTokenFilter = GetDataFromCache("BearerTokenFilter");
            if (bearerTokenFilter == null)
            {
                //-----Start post request Login-----
                bearerTokenFilter = GetDataFromCache("BearerTokenFilter");
                if (bearerTokenFilter == null)
                {
                    using (var myHttpClient = new MyHttpClientWrapper(_configuration))
                    {
                        var login = new Login(_configuration.GetValue<string>("LoginUserName"),
                                              _configuration.GetValue<string>("LoginPassword"));

                        var json = JsonConvert.SerializeObject(login);
                        string postResponse = await myHttpClient.PostDataAsync("/api/Account/Login", json);

                        bearerTokenFilter = postResponse.Trim(new Char[] { '/', '"' });
                        Console.WriteLine(bearerTokenFilter);

                    }
                }
            }
            //-----End post request Login-----


            //-----Get request GetPlatformWellDummy-----

            using (var myHttpClient = new MyHttpClientWrapper(_configuration))
            {
                string getResponse = await myHttpClient.GetResponseAsync("/api/PlatformWell/GetPlatformWellDummy", bearerTokenFilter);

                if (getResponse != null)
                {
                    List<PlatformWellActual> platformWellActualList = JsonConvert.DeserializeObject<List<PlatformWellActual>>(getResponse);

                    using (var dbContext = new PlatformWellActualContext(_configuration))
                    {
                        for (int i = 0; i < platformWellActualList.Count; i++)
                        {
                            PlatformWellActual platformWellActuallToUpdate = dbContext.PlatformWellActual
                                .Include(u => u.well).Where(u => u.Id == platformWellActualList[i].Id).FirstOrDefault();

                            if (platformWellActuallToUpdate != null)
                            {
                                platformWellActuallToUpdate.Longitude = platformWellActualList[i].Longitude;
                                platformWellActuallToUpdate.Latitude = platformWellActualList[i].Latitude;
                                platformWellActuallToUpdate.UniqueName = platformWellActualList[i].UniqueName;
                                platformWellActuallToUpdate.CreatedAt = platformWellActualList[i].CreatedAt;
                                platformWellActuallToUpdate.UpdatedAt = platformWellActualList[i].UpdatedAt;

                                for (int x = 0; x < platformWellActualList[i].well.Count; x++)
                                {
                                    Well well = dbContext.Well.Where(y => y.Id == platformWellActualList[i].well[x].Id).FirstOrDefault();

                                    if (well != null)
                                    {
                                        well.Longitude = platformWellActualList[i].well[x].Longitude;
                                        well.Latitude = platformWellActualList[i].well[x].Latitude;
                                        well.UniqueName = platformWellActualList[i].well[x].UniqueName; // + "(" + i + "," + x + ")"
                                        well.CreatedAt = platformWellActualList[i].well[x].CreatedAt;
                                        well.UpdatedAt = platformWellActualList[i].well[x].UpdatedAt;
                                        well.PlatformId = platformWellActualList[i].well[x].PlatformId;
                                    }
                                    else
                                    {
                                        platformWellActuallToUpdate.well.Add(platformWellActualList[i].well[x]);
                                    }

                                }

                            }
                            else
                            {
                                dbContext.PlatformWellActual.Add(platformWellActualList[i]);
                            }

                            try
                            {
                                dbContext.SaveChanges();
                                Console.WriteLine("User update successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                    }

                }
            }

            //-----End get request GetPlatformWellDummy-----

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void SetDataInCache(string key, string value)
        {
            try
            {
                // Set data in cache with options (e.g., expiration time)
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10), // Cache expires after 10 minutes
                    Priority = CacheItemPriority.Normal // Set cache priority
                };

                _memoryCache.Set(key, value, cacheOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public string GetDataFromCache(string key)
        {
            try
            {
                if (_memoryCache.TryGetValue(key, out string cachedData))
                {
                    return cachedData; // Data found in cache
                }

                return null; // Data not found in cache
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}