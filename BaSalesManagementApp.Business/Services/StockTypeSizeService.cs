using BaSalesManagementApp.Dtos.StockTypeSizeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaSalesManagementApp.Entites.DbSets;

namespace BaSalesManagementApp.Business.Services
{
    public class StockTypeSizeService : IStockTypeSizeService
    {
        private readonly IStockTypeSizeRepository _stockTypeSizeRepository;
        private readonly ICategoryRepository _categoryRepository;

        public StockTypeSizeService(IStockTypeSizeRepository stockTypeSizeRepository, ICategoryRepository categoryRepository)
        {
            _stockTypeSizeRepository = stockTypeSizeRepository;
            _categoryRepository = categoryRepository;

        }

        /// <summary>
        /// Yeni bir Stok Tipi Boyutu oluşturur.
        /// </summary>
        /// <param name="stockTypeSizeCreateDTO">Oluşturulacak Stok Tipi Boyutu bilgileri</param>
        /// <returns>Stok Tipi Boyutu oluşturma işlemi sonucu</returns>
        public async Task<IDataResult<StockTypeSizeDTO>> AddAsync(StockTypeSizeCreateDTO stockTypeSizeCreateDTO)
        {
            try
            {
                var stockTypeSize = stockTypeSizeCreateDTO.Adapt<StockTypeSize>();
                await _stockTypeSizeRepository.AddAsync(stockTypeSize);
                await _stockTypeSizeRepository.SaveChangeAsync();
                return new SuccessDataResult<StockTypeSizeDTO>(stockTypeSize.Adapt<StockTypeSizeDTO>(), Messages.STOCK_TYPE_SIZE_CREATED_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<StockTypeSizeDTO>(stockTypeSizeCreateDTO.Adapt<StockTypeSizeDTO>(), Messages.STOCK_TYPE_SIZE_CREATE_FAILED + ex.Message);
            }
        }

        /// <summary>
        /// Belirtilen ID'ye göre ilgili Stok Tipi Boyutunu getirir.
        /// </summary>
        /// <param name="stockTypeSizeId">Getirilecek Stok Tipi Boyutu ID'si</param>
        /// <returns>Belirtilen ID'li Stok Tipi Boyutu verileri</returns>
        public async Task<IDataResult<StockTypeSizeDTO>> GetByIdAsync(Guid stockTypeSizeId)
        {
            try
            {
                var stockTypeSize = await _stockTypeSizeRepository.GetByIdAsync(stockTypeSizeId);
                if (stockTypeSize == null)
                {
                    return new ErrorDataResult<StockTypeSizeDTO>(Messages.STOCK_TYPE_SIZE_NOT_FOUND);
                }
                var stockTypeSizeDTO = stockTypeSize.Adapt<StockTypeSizeDTO>();
                var category = await _categoryRepository.GetByIdAsync(stockTypeSize.CategoryId.GetValueOrDefault());
                if (category is not null)
                    stockTypeSizeDTO.CategoryName = category.Name;

                return new SuccessDataResult<StockTypeSizeDTO>(stockTypeSizeDTO, Messages.STOCK_TYPE_SIZE_FOUND_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<StockTypeSizeDTO>(Messages.STOCK_TYPE_SIZE_GET_FAILED + ex.Message);
            }
        }

        /// <summary>
        /// Belirtilen ID'ye ait Stok Tipi Boyutunu siler.
        /// </summary>
        /// <param name="stockTypeSizeId">Silinecek Stok Tipi Boyutu ID'si</param>
        /// <returns>Stok Tipi Boyutu silme işlemi sonucu</returns>
        public async Task<IResult> DeleteAsync(Guid stockTypeSizeId)
        {
            try
            {
                var stockTypeSize = await _stockTypeSizeRepository.GetByIdAsync(stockTypeSizeId);
                if (stockTypeSize == null)
                {
                    return new ErrorResult(Messages.STOCK_TYPE_SIZE_NOT_FOUND);
                }

                await _stockTypeSizeRepository.DeleteAsync(stockTypeSize);
                await _stockTypeSizeRepository.SaveChangeAsync();
                return new SuccessResult(Messages.STOCK_TYPE_SIZE_DELETED_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ErrorResult(Messages.STOCK_TYPE_SIZE_DELETE_FAILED + ex.Message);
            }
        }

        /// <summary>
        /// Tüm Stok Tipi Boyutlarını getirir.
        /// </summary>
        /// <returns>Tüm Stok Tipi Boyutları listesi</returns>
        public async Task<IDataResult<List<StockTypeSizeDTO>>> GetAllAsync()
        {
            try
            {
                var stockTypeSizes = await _stockTypeSizeRepository.GetAllAsync();
                var stockTypeSizeDTOs = stockTypeSizes.Adapt<List<StockTypeSizeDTO>>();

                if (stockTypeSizeDTOs == null || stockTypeSizeDTOs.Count == 0)
                {
                    return new ErrorDataResult<List<StockTypeSizeDTO>>(stockTypeSizeDTOs, Messages.STOCK_TYPE_SIZE_LIST_EMPTY);
                }
                var categories = await _categoryRepository.GetAllAsync();

                foreach (var stockTypeSizeDTO in stockTypeSizeDTOs)
                {

                    var category = categories.FirstOrDefault(b => b.Id == stockTypeSizeDTO.CategoryId);
                    stockTypeSizeDTO.CategoryName = category?.Name;
                }

                return new SuccessDataResult<List<StockTypeSizeDTO>>(stockTypeSizeDTOs, Messages.STOCK_TYPE_SIZE_LISTED_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<StockTypeSizeDTO>>(new List<StockTypeSizeDTO>(), Messages.STOCK_TYPE_SIZE_LIST_FAILED + ex.Message);
            }
        }

        /// <summary>
        /// Belirtilen Stok Tipi Boyutunu günceller.
        /// </summary>
        /// <param name="stockTypeSizeUpdateDTO">Güncellenecek Stok Tipi Boyutu bilgileri</param>
        /// <returns>Stok Tipi Boyutu güncelleme işlemi sonucu</returns>
        public async Task<IResult> UpdateAsync(StockTypeSizeUpdateDTO stockTypeSizeUpdateDTO)
        {
            try
            {
                var stockTypeSizeOnHand = await _stockTypeSizeRepository.GetByIdAsync(stockTypeSizeUpdateDTO.Id);
                if (stockTypeSizeOnHand == null)
                {
                    return new ErrorDataResult<StockTypeSizeDTO>(Messages.STOCK_TYPE_SIZE_NOT_FOUND);
                }

                stockTypeSizeOnHand = stockTypeSizeUpdateDTO.Adapt(stockTypeSizeOnHand);
                await _stockTypeSizeRepository.UpdateAsync(stockTypeSizeOnHand);
                await _stockTypeSizeRepository.SaveChangeAsync();
                return new SuccessDataResult<StockTypeSizeDTO>(stockTypeSizeOnHand.Adapt<StockTypeSizeDTO>(), Messages.STOCK_TYPE_SIZE_UPDATE_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<StockTypeSizeDTO>(Messages.STOCK_TYPE_SIZE_UPDATE_FAILED + ex.Message);
            }
        }

		public async Task<IDataResult<List<StockTypeSizeDTO>>> GetAllAsync(string searchQuery)
		{
			try
			{
				// Get all stock types
				var stockTypes = await _stockTypeSizeRepository.GetAllAsync();

				// Filter by size if search query is provided
				if (!string.IsNullOrEmpty(searchQuery))
				{
					stockTypes = stockTypes
						.Where(st => st.Size.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
						.ToList();
				}

				// Convert to DTO list
				var stockTypeSizeList = stockTypes.Adapt<List<StockTypeSizeDTO>>();

				// Check if the list is empty
				if (!stockTypeSizeList.Any())
				{
					return new ErrorDataResult<List<StockTypeSizeDTO>>(stockTypeSizeList, "No data found matching the search criteria.");
				}

				return new SuccessDataResult<List<StockTypeSizeDTO>>(stockTypeSizeList, "Data retrieved successfully.");
			}
			catch (Exception ex)
			{
				// Return error if an exception occurs
				return new ErrorDataResult<List<StockTypeSizeDTO>>(
					new List<StockTypeSizeDTO>(),
					$"{Messages.STOCK_TYPE_SIZE_NOT_FOUND} - {ex.Message}"
				);
			}
		}
	}
}
