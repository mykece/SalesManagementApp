﻿using BaSalesManagementApp.Business.Utilities;
using BaSalesManagementApp.Dtos.CompanyDTOs;
using BaSalesManagementApp.MVC.Models.CategoryVMs;
using BaSalesManagementApp.MVC.Models.CompanyVMs;
using BaSalesManagementApp.MVC.Models.StudentVMs;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using X.PagedList;
using System.Text.RegularExpressions;
using BaSalesManagementApp.MVC.Models.OrderVMs;
using BaSalesManagementApp.Dtos.CityDTOs;
using BaSalesManagementApp.Entites.DbSets;
using BaSalesManagementApp.MVC.Models.CustomerVMs;
using BaSalesManagementApp.Dtos.AdminDTOs;
using Castle.Core.Resource;


namespace BaSalesManagementApp.MVC.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<Resource> _stringLocalizer;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        public CompanyController(ICompanyService companyService, IProductService productService, IStringLocalizer<Resource> stringLocalizer, ICityService cityService, ICountryService countryService)
        {
            _companyService = companyService;
            _productService = productService;
            _stringLocalizer = stringLocalizer;
            _cityService = cityService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int? page, string sortOrder)
        {
            try
            {
                int pageNumber = page ?? 1;
                int pageSize = 5;
                ViewBag.CurrentSort=sortOrder;
                
                var result = await _companyService.GetAllAsync();
                var companyListVMs = result.Data.Adapt<List<CompanyListVM>>();

                switch (sortOrder)
                {
                    case "name_asc":
                        companyListVMs = companyListVMs.OrderBy(x=>x.Name).ToList();
                        break;
                    case "name_desc":
                        companyListVMs=companyListVMs.OrderByDescending(x=>x.Name).ToList();
                        break;
                    case "date_asc":
                        companyListVMs=companyListVMs.OrderBy(x=>x.CreatedDate).ToList();
                        break;
                    case "date_desc":
                        companyListVMs=companyListVMs.OrderByDescending(x=>x.CreatedDate).ToList();
                        break;
                    default: 
                        companyListVMs=companyListVMs.OrderBy(x=>x.Name).ToList();
                        break;
                }

                if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.COMPANY_LISTED_ERROR]);
                    //NotifyError(result.Message);
                    return View(Enumerable.Empty<CompanyListVM>().ToPagedList(pageNumber, pageSize));
                }
                NotifySuccess(_stringLocalizer[Messages.COMPANY_LISTED_SUCCESS]);
                // NotifySuccess(result.Message);

                var paginatedList = companyListVMs.Adapt<List<CompanyListVM>>().ToPagedList(pageNumber, pageSize);

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                NotifyError(_stringLocalizer[Messages.COMPANY_LISTED_ERROR]);
                //Console.WriteLine(ex.Message);
                return View("Error");
            }            
        }

        

        public async Task<IActionResult> Details(Guid companyId)
        {
            try
            {


                var result = await _companyService.GetByIdAsync(companyId);


                if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.CUSTOMER_NOT_FOUND]);
                    //NotifyError(result.Message);
                    return RedirectToAction("Index");
                }

        

                var companyDetailVM = result.Data.Adapt<CompanyDetailsVM>();
               
                NotifySuccess(_stringLocalizer["CUSTOMER_FOUND_SUCCESS"]);

                return View(companyDetailVM);
            }
            catch (Exception ex)
            {
                var detailedMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                NotifyError($"An error occurred: {detailedMessage}");
                return View("Error");
            }
        }

        public async Task<IActionResult> Create()
        {
            
            var citiesRes = await _cityService.GetAllAsync();
            var countriesRes = await _countryService.GetAllAsync();
            var model = new CompanyCreateVM
            {
                
                Cities = citiesRes.Data.Adapt<List<City>>(),
                Countries = countriesRes.Data.Adapt<List<Country>>()
            };
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyCreateVM companyCreateVM)
        {
            try
            {


                companyCreateVM.Name = StringUtilities.CapitalizeFirstLetter(companyCreateVM.Name);
                companyCreateVM.Address = StringUtilities.CapitalizeEachWord(companyCreateVM.Address);

                var result = await _companyService.AddAsync(companyCreateVM.Adapt<CompanyCreateDTO>());

                if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.COMPANY_ADD_ERROR]);
                    // NotifyError(result.Message);
                    return View(companyCreateVM);
                }
                NotifySuccess(_stringLocalizer[Messages.COMPANY_ADD_SUCCESS]);
                //NotifySuccess(result.Message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                NotifyError(_stringLocalizer[Messages.COMPANY_ADD_ERROR]);
                // Console.WriteLine(ex.Message);
                return View("Error");
            }
        }




        public async Task<IActionResult> Update(Guid companyId)
        {
            try
            {
                var result = await _companyService.GetByIdAsync(companyId);

                if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.COMPANY_UPDATE_ERROR]);
                    // NotifyError(result.Message);
                    return RedirectToAction("Index");
                }

                NotifySuccess(_stringLocalizer[Messages.COMPANY_UPDATE_SUCCESS]);
                // NotifySuccess(result.Message);


                var companyUpdateVM = result.Data.Adapt<CompanyUpdateVM>();

                return View(companyUpdateVM);
            }
            catch (Exception ex)
            {
                NotifyError(_stringLocalizer[Messages.COMPANY_UPDATE_ERROR]);
                // Console.WriteLine(ex.Message);
                return View("Error");
            }            
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CompanyUpdateVM companyUpdateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(companyUpdateVM);
                }

                
                companyUpdateVM.Name=StringUtilities.CapitalizeFirstLetter(companyUpdateVM.Name);
                companyUpdateVM.Address=StringUtilities.CapitalizeEachWord(companyUpdateVM.Address);

                var result = await _companyService.UpdateAsync(companyUpdateVM.Adapt<CompanyUpdateDTO>());

                if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.COMPANY_UPDATE_ERROR]);
                    // NotifyError(result.Message);
                    return View(companyUpdateVM);
                }
                NotifySuccess(_stringLocalizer[Messages.COMPANY_UPDATE_SUCCESS]);
                // NotifySuccess(result.Message);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                NotifyError(_stringLocalizer[Messages.COMPANY_UPDATE_ERROR]);
                //Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(Guid companyId)
        {
            try
            {
                // Şirketi silme veya pasif duruma alma işlemi
                var result = await _companyService.DeleteAsync(companyId);

                // Şirkete bağlı ürünleri silme işlemi
                var resul1 = await _productService.GetAllAsync();
                foreach (var item in resul1.Data)
                {
                    if (item.CompanyId == companyId)
                    {
                        await _productService.DeleteAsync(item.Id);
                    }
                }

                var result2 = await _productService.DeleteAsync(companyId);

                // Şirket pasif duruma alındıysa farklı bir mesaj göster
                if (result.Message == Messages.COMPANY_PASSIVED_SUCCESS)
                {
                    NotifySuccess(_stringLocalizer[Messages.COMPANY_PASSIVED_SUCCESS]);
                }
                // Şirket silindiyse farklı bir mesaj göster
                else if (result.Message == Messages.COMPANY_DELETE_SUCCESS)
                {
                    NotifySuccess(_stringLocalizer[Messages.COMPANY_DELETE_SUCCESS]);
                }
                // Eğer işlem başarısızsa hata mesajı göster
                else if (!result.IsSuccess)
                {
                    NotifyError(_stringLocalizer[Messages.COMPANY_DELETE_ERROR]);
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda genel hata mesajı göster
                NotifyError(_stringLocalizer[Messages.COMPANY_DELETE_ERROR]);
                return View("Error");
            }
        }
        /// <summary>
        /// Belirtilen şirketin herhangi bir sipariş ile ilişkilendirilip ilişkilendirilmediğini asenkron olarak kontrol eder.
        /// </summary>
        /// <param name="companyId">Kontrol edilecek şirketin benzersiz kimliği.</param>
        /// <returns>
        /// Şirketin herhangi bir sipariş ile ilişkilendirilip ilişkilendirilmediğini belirten bir boolean değeri içeren JsonResult döner.
        /// Eğer şirket bir sipariş ile ilişkiliyse, dönen değerde "isInOrder" true olacaktır; aksi durumda false olacaktır.
        /// </returns>
        public async Task<JsonResult> CheckCompanyInOrder(Guid companyId)
        {
            var isInOrder = await _companyService.IsCompanyInOrderAsync(companyId);

            return Json(new { isInOrder});


        }


        /// <summary>
        /// Belirtilen ülkeId sine göre şehirleri getirir.
        /// </summary>
        /// <param name="countryId">Şehirlerin getirileceği ülkenin benzersiz ID'si</param>
        /// <returns>Belirtilen ülkeye ait şehirlerin listesini içeren bir JSON nesnesi.Eğer bir hata oluşursa, hata mesajı ile birlikte bad request döner</returns>

        [HttpGet]
        public async Task<IActionResult> GetCitiesByCountryId(Guid countryId)
        {
            try
            {
                var citiesResult = await _cityService.GetByCountryIdAsync(countryId);
                var cities = citiesResult.Data?.Adapt<List<CityDTO>>();
                return Json(cities);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error fetching cities.");
            }
        }
    }
}
