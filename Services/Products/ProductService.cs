using System.Net;
using App.Repositories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductAsync(int count)
        {
            try
            {
                var products = await productRepository.GetTopPriceProductAsync(count);
                var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

                return ServiceResult<List<ProductDto>>.Success(productsAsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ProductDto>>.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            try
            {
                var products = await productRepository.GetAll().ToListAsync();
                var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

                return ServiceResult<List<ProductDto>>.Success(productsAsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<ProductDto>>.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }


        public async Task<ServiceResult<List<ProductDto>>>GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            // 1-10 => ilk 10 kayıt skip(0).Take(10)
            //2-10 => 11-20 kayıt   skip(10).Take(10) yani 10 tane atla ve oradaki 10 taneyi ver.

            //Kaç atlayacağımızı bulan fonksiyonu tanımlıyoruz.
            //int skip = (pageNumber - 1) * pageSize; ////bu mantıkta yani


            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take (pageSize).ToListAsync();

            var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);

        }

        public async Task<ServiceResult<ProductDto>> GetByIdAsync(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);

                if (product is null)
                {
                    return ServiceResult<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);
                }

                var productAsDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
                return ServiceResult<ProductDto>.Success(productAsDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<ProductDto>.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
            try
            {
                var product = new Product()
                {
                    Name = request.Name,
                    Price = request.Price,
                    Stock = request.Stock,
                };

                await productRepository.AddAsync(product);
                await unitOfWork.SaveChangesAsync();

                var response = new CreateProductResponse(product.Id);
                return ServiceResult<CreateProductResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ServiceResult<CreateProductResponse>.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);

                if (product is null)
                {
                    return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
                }

                product.Name = request.Name;
                product.Price = request.Price;
                product.Stock = request.Stock;

                productRepository.Update(product);
                await unitOfWork.SaveChangesAsync();

                return ServiceResult.Success(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var product = await productRepository.GetByIdAsync(id);

                if (product is null)
                {
                    return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
                }

                productRepository.Delete(product);
                await unitOfWork.SaveChangesAsync();

                return ServiceResult.Success(HttpStatusCode.NoContent); //Güncelleme veya silme işlemlerinde 204 No Content döndürüyoruz.
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Hata: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}

