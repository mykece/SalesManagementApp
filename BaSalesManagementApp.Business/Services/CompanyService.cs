using BaSalesManagementApp.Business.Interfaces;
using BaSalesManagementApp.Dtos.CompanyDTOs;

namespace BaSalesManagementApp.Business.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IBranchService _branchService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        public CompanyService(ICompanyRepository companyRepository, IBranchService branchService, IOrderRepository orderRepository, ICityRepository cityRepository, ICountryRepository countryRepository)
        {
            _companyRepository = companyRepository;
            _branchService = branchService;
            _orderRepository = orderRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }

        //Yeni bir firma ekler ve işlem başarılıysa eklenen firmayı döndürür. Eğer bir hata oluşursa, uygun bir hata mesajıyla birlikte hata durumunu döndürür.
        public async Task<IDataResult<CompanyDTO>> AddAsync(CompanyCreateDTO companyCreateDTO)
        {          
            try
            {
                var newBranch = companyCreateDTO.Adapt<Company>();

                await _companyRepository.AddAsync(newBranch);
                await _companyRepository.SaveChangeAsync();

                return new SuccessDataResult<CompanyDTO>(newBranch.Adapt<CompanyDTO>(), Messages.COMPANY_ADD_SUCCESS);
            }
            catch (Exception)
            {
                return new ErrorDataResult<CompanyDTO>(Messages.COMPANY_ADD_ERROR);
            }            
        }

        //Belirtilen bir firmayı siler ve işlem başarılıysa başarılı bir mesaj döndürür.Herhangi bir hata oluşursa uygun bir hata mesajıyla birlikte hata durumunu döndürür.

        public async Task<IResult> DeleteAsync(Guid companyId)
        {
            try
            {
                var deletingCompany = await _companyRepository.GetByIdAsync(companyId);
                var branches = await _branchService.GetBranchesByCompanyIdAsync(companyId);

                foreach (var branch in branches)
                {
                    await _branchService.DeleteAsync(branch.Id);
                }
                var companyInOrders = await _orderRepository.AnyAsync(o => o.Product.CompanyId == companyId);
                if (companyInOrders)
                {
                    var result = await _companyRepository.SetCompanyToPassiveAsync(companyId);
                    if (!result.IsSuccess)
                    {
                        return result;

                    }
                    await _companyRepository.SaveChangeAsync();
                    return new SuccessResult((Messages.COMPANY_PASSIVED_SUCCESS));
                }

                await _companyRepository.DeleteAsync(deletingCompany);
                await _companyRepository.SaveChangeAsync();

                return new SuccessResult(Messages.COMPANY_DELETE_SUCCESS);
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.COMPANY_DELETE_ERROR);
            }            
        }

        //Tüm firmaları getirir ve işlem başarılıysa firma listesini döndürür.Eğer hiç şube bulunamazsa uygun bir mesajla birlikte hata durumunu döndürür.

        public async Task<IDataResult<List<CompanyListDTO>>> GetAllAsync()
        {            
            try
            {
                IEnumerable<Company> companies;
                companies = await _companyRepository.GetAllAsync();

                if (companies.Count() == 0)
                {
                    return new ErrorDataResult<List<CompanyListDTO>>(new List<CompanyListDTO>(), Messages.COMPANY_LISTED_NOTFOUND);
                }

                return new SuccessDataResult<List<CompanyListDTO>>(companies.Adapt<List<CompanyListDTO>>(),Messages.COMPANY_LISTED_SUCCESS);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<CompanyListDTO>>(new List<CompanyListDTO>(), Messages.COMPANY_LISTED_ERROR);
            }            
        }

		public async Task<IDataResult<List<CompanyListDTO>>> GetAllAsync(string searchQuery)
		{
			try
			{
				// Fetch all companies
				var companies = await _companyRepository.GetAllAsync();

				// Filter companies by name if searchQuery is not empty or null
				if (!string.IsNullOrWhiteSpace(searchQuery))
				{
					companies = companies
						.Where(c => c.Name != null && c.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
				}

				if (!companies.Any())
				{
					return new ErrorDataResult<List<CompanyListDTO>>(new List<CompanyListDTO>(), Messages.COMPANY_LISTED_NOTFOUND);
				}

				var companyListDTOs = companies.Adapt<List<CompanyListDTO>>();
				return new SuccessDataResult<List<CompanyListDTO>>(companyListDTOs, Messages.COMPANY_LISTED_SUCCESS);
			}
			catch (Exception ex)
			{
				// Log exception details if necessary
				return new ErrorDataResult<List<CompanyListDTO>>(new List<CompanyListDTO>(), Messages.COMPANY_LISTED_ERROR + ex.Message);
			}
		}

		//Belirli bir firma kimliğine göre firmayı getirir.Firma bulunamazsa uygun bir hata mesajıyla birlikte hata durumunu döndürür.

		public async Task<IDataResult<CompanyDTO>> GetByIdAsync(Guid companyId)
        {
            CompanyDTO companyDTO = new CompanyDTO();

            try
            {
                var company = await _companyRepository.GetByIdAsync(companyId);
                if (company == null)
                {
                    return new ErrorDataResult<CompanyDTO>(Messages.COMPANY_GETBYID_ERROR);
                }

                var city = await _cityRepository.GetByIdAsync(company.CityID.GetValueOrDefault());
                var country = await _countryRepository.GetByIdAsync(city?.CountryId ?? Guid.Empty);

                // Map company to CompanyDTO
                companyDTO = company.Adapt<CompanyDTO>();

                // Add city and country data if available
                if (city != null)
                {
                    companyDTO.CityName = city.Name;
                }
                if (country != null)
                {
                    companyDTO.CountryName = country.Name;
                }

                return new SuccessDataResult<CompanyDTO>(companyDTO, Messages.COMPANY_GETBYID_SUCCESS);
            }
            catch
            {
                return new ErrorDataResult<CompanyDTO>(Messages.COMPANY_GETBYID_ERROR);
            }
        }

        public async Task<bool> IsCompanyInOrderAsync(Guid companyId)
        {
           return await _orderRepository.AnyAsync(o=>o.Product.CompanyId == companyId);
        }

        //Belirli bir firma kimliğine göre firma bilgilerini günceller.Güncelleme başarılıysa güncellenen firma bilgilerini döndürür.Herhangi bir hata oluşursa uygun bir hata mesajıyla birlikte hata durumunu döndürür.

        public async Task<IDataResult<CompanyDTO>> UpdateAsync(CompanyUpdateDTO companyUpdateDTO)
        {      
            try
            {
                var updatingCompany = await _companyRepository.GetByIdAsync(companyUpdateDTO.Id);

                if (updatingCompany == null)
                {
                    return new ErrorDataResult<CompanyDTO>(Messages.COMPANY_GETBYID_ERROR);
                }

                await _companyRepository.UpdateAsync(companyUpdateDTO.Adapt(updatingCompany));
                await _companyRepository.SaveChangeAsync();

                return new SuccessDataResult<CompanyDTO>(updatingCompany.Adapt<CompanyDTO>(), Messages.COMPANY_UPDATE_SUCCESS);
            }
            catch (Exception)
            {
                return new ErrorDataResult<CompanyDTO>(Messages.COMPANY_UPDATE_ERROR);
            }            
        }
    }
}
